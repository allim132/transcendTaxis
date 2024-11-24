using UnityEngine;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour
{
    public Button interactButton;
    private pasNPCSpawner spawner;

    public void Initialize(pasNPCSpawner spawner)
    {
        this.spawner = spawner;
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteract);
        }
        else
        {
            Debug.LogError("InteractButton is missing on InteractableNPC");
        }
    }

    void OnInteract()
    {
        // Handle interaction here
        Debug.Log("NPC Interacted!");
        Despawn();
    }

    void Despawn()
    {
        spawner.RemoveNPC(this);
        Destroy(gameObject);
    }
}