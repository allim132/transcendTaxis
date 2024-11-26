using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameCanvas;

    // Call this method to show the main menu
    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

    // Call this method to show the game UI
    public void ShowGameUI()
    {
        mainMenuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
}