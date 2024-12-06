using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Include UI namespace

public class StartMenuManager : MonoBehaviour
{
    public Button startButton; // Reference to the Start button
    public Button quitButton; // Reference to the Quit button

    void Start()
    {
        SetButtonNames("Begin Game", "Exit Game");
    }

    // Method for starting the game
    public void StartGame()
    {
        // Load the main game scene (replace "GameScene" with the name of your main scene)
        SceneManager.LoadScene("Dungeon");
    }

    // Method for quitting the game
    public void QuitGame()
    {
        // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Only for testing in the Unity Editor
#else
            Application.Quit(); // Quits the application in a build
#endif
    }

    // Method to change the text of the buttons
    public void SetButtonNames(string startText, string quitText)
    {
        if (startButton != null && startButton.GetComponentInChildren<Text>() != null)
        {
            startButton.GetComponentInChildren<Text>().text = startText; // Change Start button text
        }

        if (quitButton != null && quitButton.GetComponentInChildren<Text>() != null)
        {
            quitButton.GetComponentInChildren<Text>().text = quitText; // Change Quit button text
        }
    }
}