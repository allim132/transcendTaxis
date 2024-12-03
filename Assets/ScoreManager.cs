using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using static ScoreManager;

/*
 * This component was developed by Lead Developer Alex
 * 
 * Lead Developer Note:
 * This script is responsible for the "Score" component outlined in the SRA document.
 */
public class ScoreManager : MonoBehaviour
{
    // Public Variables

    // Score holder
    public static int currentScore = 0;

    [Space(10)]
    public PrometeoCarController prometeoCarController;

    [Space(10)]
    public TMP_Text currentScoreHolder;
    public TMP_Text currentPassengerHolder;

    // Private Variables

    // Scoring range
    private const int MIN_SCORE = 200;
    private const int MAX_SCORE = 2000;

    // Time scoring parameters
    private const float MAX_TIME = 420; // 7 minutes
    private const float TIME_SCORE_WEIGHT = 500f;
    private const float HURRY_TIME_BONUS = 200f;
    private const float HURRY_TIME_THRESHOLD = 60f; // 1 minute

    // Collision scoring parameters
    private const float COLLISION_SCORE_WEIGHT = 500f;
    private const int COLLISION_THRESHOLD = 5;

    // Passenger type multipliers
    private const float CAUTIOUS_MULTIPLIER = 1.05f;
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

    // Ensures that PassengerType is updated
    private void LateUpdate()
    {
        if (currentPassengerHolder != null)
        {
            if (hasPassenger)
            {
                currentPassengerHolder.SetText("Passenger Type: " + currentPassengerType.ToString());
            }
            else
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
        int randomIndex = Random.Range(0, 3); // Returns 0, 1, or 2

        switch (randomIndex)
        {
            case 0:
                SetPassengerType(PassengerType.Normal);
                break;
            case 1:
                SetPassengerType(PassengerType.Cautious);
                break;
            case 2:
                SetPassengerType(PassengerType.InAHurry);
                break;
        }
    }

    // Calculates the final score by calculating time and collision score seperately
    public int CalculateFinalScore()
    {
        float timeScore = CalculateTimeScore();
        float collisionScore = CalculateCollisionScore();

        float finalScore = timeScore + collisionScore;
        return Mathf.Clamp(Mathf.RoundToInt(finalScore), MIN_SCORE, MAX_SCORE);
    }

    // Time score calculation
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

    // Collision score calculation
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

        // Add bonus for low collisions and penalize high collisions
        if (collisionCount == 0)
        {
            collisionScore += 400;
        } else if (collisionCount <= 5){
            collisionScore += 200;
        } else if (collisionCount > 10)
        {
            collisionScore -= 100;
            switch (currentPassengerType)
            {
                case PassengerType.Cautious:
                    collisionScore -= 100;
                    break;
                case PassengerType.InAHurry:
                    if (collisionCount <= 15)
                    {
                        collisionScore += 100;
                    }
                    break;
            }
        }
        return collisionScore;
    }

    public void DisplayScore()
    {
        // Debug.Log($"Final Score: {currentScore}");
        // Debug.Log($"Passenger Type: {currentPassengerType}");
        Debug.Log($"Delivery Time: {deliveryTime} seconds");
        Debug.Log($"Collisions: {collisionCount}");
        currentScoreHolder.SetText("Score: " + currentScore);
    }

    // Called by EndDeliver whenever a delivery is completed
    public void CompleteDelivery()
    {
        int scoreForDeliveryInstance = CalculateFinalScore();
        Debug.Log("Score for delivery: " + scoreForDeliveryInstance);
        currentScore += scoreForDeliveryInstance;
        DisplayScore();

        ResetDelivery();
    }

    // Call this method when a passenger is picked up
    public void StartDelivery()
    {
        ResetDelivery();

        hasPassenger = true;

        GetRandomPassenger();
        startTime = Time.time;
        startCollisions = prometeoCarController.GetCollisionCount();
    }

    // Call this method whenever a passenger is dropped off
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

    // Getter method for another function
    public int getScore()
    {
        return currentScore;
    }
}


