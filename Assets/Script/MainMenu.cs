using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Village");
    }

    public void QuitGame()
    {
        // Exit the application in a built game
        Application.Quit();
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Optional: Ensure the cursor is visible and unlocked for menu interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    

    // Update is called once per frame
    void Update()
    {
        // Optional: Add keyboard shortcut for quick testing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
}
