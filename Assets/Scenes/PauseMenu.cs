using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button mainMenuButton;

    private bool isPaused = false;

    void Start()
    {
        // Set the initial button text
        UpdateButtonText();

        // Add listeners to the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        pauseMenuUI.SetActive(false); // Hide pause menu at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scenes
        SceneManager.LoadScene("StartingMenu");
    }

    void UpdateButtonText()
    {
        resumeButton.GetComponentInChildren<Text>().text = "Resume";
        mainMenuButton.GetComponentInChildren<Text>().text = "Main Menu";
    }
}