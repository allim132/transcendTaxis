using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using static ScoreManager;

public static class PassengerTypeExtensions
{
    public static string ToString(this PassengerType passengerType)
    {
        switch (passengerType)
        {
            case PassengerType.Cautious:
                return "Cautious";
            case PassengerType.Normal:
                return "Normal";
            case PassengerType.InAHurry:
                return "In a Hurry";
            default:
                return passengerType.ToString();
        }
    }
}

public class ScoreManager : MonoBehaviour
{
    // Public Variables

    // Score holder
    public static int currentScore = 0;

    [Space(10)]
    public PrometeoCarController prometeoCarController;

    [Space(10)]
    public Text currentScoreHolder;
    public TMP_Text currentPassengerHolder;

    // Private Variables

    // Scoring range
    private const int MIN_SCORE = 0;
    private const int MAX_SCORE = 1000;

    // Time scoring parameters
    private const float MAX_TIME = 300f; // 5 minutes
    private const float TIME_SCORE_WEIGHT = 500f;
    private const float HURRY_TIME_BONUS = 200f;
    private const float HURRY_TIME_THRESHOLD = 60f; // 1 minute

    // Collision scoring parameters
    private const float COLLISION_SCORE_WEIGHT = 500f;
    private const int COLLISION_THRESHOLD = 5;

    // Passenger type multipliers
    private const float CAUTIOUS_MULTIPLIER = 2f;
    private const float HURRY_MULTIPLIER = 0.5f;

    private int collisionCount;
    private float deliveryTime;
    private PassengerType currentPassengerType;

    // Timer for calculating when passenger was picked up
    private float startTime;
    private float endTime;

    // Collision counter to determine how many collisions occured for the passenger
    private int startCollisions;
    private int endCollisions;

    // Keep the status for if you have a passenger
    private bool hasPassenger = false;




    public enum PassengerType
    {
        Cautious,
        Normal,
        InAHurry
    }



    private void Start()
    {
        ResetDelivery();
    }

    private void LateUpdate()
    {
        if (currentPassengerHolder != null)
        {
            if (hasPassenger)
            {
                currentPassengerHolder.SetText("Passenger Type: " + currentPassengerType.ToString());
            } else
            {
                currentPassengerHolder.SetText("No Passenger");
            }
            
        }
    }

    public void ResetDelivery()
    {
        collisionCount = 0;
        deliveryTime = 0f;
        hasPassenger = false;
        currentPassengerHolder.SetText("No Passenger");
    }

    public void SetPassengerType(PassengerType type)
    {
        currentPassengerType = type;
        currentPassengerHolder.SetText("Passenger Type: " + currentPassengerType.ToString());

        Debug.Log("Passenger Type: " + currentPassengerType.ToString());
    }

    // To be called when picking up a passenger to randomize passenger type.
    public void GetRandomPassenger()
    {
        // 1/3 chance to get either passenger
        float randomizer = Random.Range(0, 1);
        if (randomizer > 0.66)
        {
            SetPassengerType(PassengerType.Normal);
        }
        else if (randomizer > 0.33)
        {
            SetPassengerType(PassengerType.Cautious);
        }
        else
        {
            SetPassengerType(PassengerType.InAHurry);
        }
    }

    public void AddCollision()
    {
        collisionCount++;
    }

    public void SetDeliveryTime(float time)
    {
        deliveryTime = time;
    }

    public int CalculateFinalScore()
    {
        float timeScore = CalculateTimeScore();
        float collisionScore = CalculateCollisionScore();

        float finalScore = timeScore + collisionScore;
        return Mathf.Clamp(Mathf.RoundToInt(finalScore), MIN_SCORE, MAX_SCORE);
    }

    private float CalculateTimeScore()
    {
        float normalizedTime = Mathf.Clamp01(1f - (deliveryTime / MAX_TIME));
        float timeScore = normalizedTime * TIME_SCORE_WEIGHT;

        if (currentPassengerType == PassengerType.InAHurry)
        {
            if (deliveryTime <= HURRY_TIME_THRESHOLD)
            {
                timeScore += HURRY_TIME_BONUS;
            }
            else
            {
                timeScore *= 0.8f; // 20% penalty for late delivery
            }
        }

        return timeScore;
    }

    private float CalculateCollisionScore()
    {
        float collisionScore = 0f;

        switch (currentPassengerType)
        {
            case PassengerType.Cautious:
                collisionScore = -collisionCount * CAUTIOUS_MULTIPLIER * (COLLISION_SCORE_WEIGHT / COLLISION_THRESHOLD);
                break;
            case PassengerType.Normal:
                collisionScore = -Mathf.Max(0, collisionCount - 2) * (COLLISION_SCORE_WEIGHT / COLLISION_THRESHOLD);
                break;
            case PassengerType.InAHurry:
                collisionScore = -collisionCount * HURRY_MULTIPLIER * (COLLISION_SCORE_WEIGHT / COLLISION_THRESHOLD);
                break;
        }

        return collisionScore;
    }

    public void DisplayScore()
    {
        // Debug.Log($"Final Score: {currentScore}");
        // Debug.Log($"Passenger Type: {currentPassengerType}");
        // Debug.Log($"Delivery Time: {deliveryTime} seconds");
        // Debug.Log($"Collisions: {collisionCount}");
        // TODO: Implement UI to display score and details to the player
        currentScoreHolder.text = "Score: " + currentScore;
    }

    // Call this method when a delivery is completed
    public void CompleteDelivery()
    {
        int scoreForDeliveryInstance = CalculateFinalScore();
        currentScore += scoreForDeliveryInstance;
        DisplayScore();

        ResetDelivery();
    }


    public void StartDelivery()
    {
        ResetDelivery();

        hasPassenger = true;

        GetRandomPassenger();
        startTime = Time.time;
        startCollisions = prometeoCarController.GetCollisionCount();
    }

    public void EndDelivery()
    {
        // Calculate time took for delivery
        endTime = Time.time;
        deliveryTime = endTime - startTime;

        // Calculate collisions during delivery
        endCollisions = prometeoCarController.GetCollisionCount();
        collisionCount = endCollisions - startCollisions;

        CompleteDelivery();
    }
}

