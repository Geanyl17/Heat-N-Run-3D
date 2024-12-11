using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI; // Required for Image and CanvasGroup

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu; // Reference to the settings menu UI panel
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public string nextSceneName = "Level1"; // The next scene to load after the video
    public PlayerMovement playerMovement; // Reference to PlayerMovement script
    public GameObject uiElements; // Reference to the UI elements to hide during video playback
    public CanvasGroup fadeCanvasGroup; // Reference to the CanvasGroup for fade effect (set in the inspector)
    public float fadeDuration = 1f; // Duration of the fade effect

    private bool isSettingsActive = false; // Track the state of the settings menu

    public void Play()
    {
        // Hide UI elements when video starts
        if (uiElements != null)
        {
            uiElements.SetActive(false); // Deactivate the UI
        }

        // Ensure the video player is set to play the video
        if (videoPlayer != null)
        {
            // Play the video
            videoPlayer.Play();

            // Start a coroutine to wait for the video to finish and then load the next scene
            StartCoroutine(WaitForVideoToEnd());
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned.");
        }
    }

    private IEnumerator WaitForVideoToEnd()
    {
        // Wait until the video is no longer playing
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        // Start the fade-out effect before transitioning to the next scene
        yield return StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        // Perform fade-out
        yield return StartCoroutine(Fade(1f)); // Fade to fully opaque (black)

        // Once the fade-out is complete, load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Lerp the alpha value over time
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach the target alpha at the end
        fadeCanvasGroup.alpha = targetAlpha;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        Time.timeScale = 1f; // Ensure the game is not paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f; // Ensure the game is not paused before going to the menu
        SceneManager.LoadScene("Menu"); // Load the menu scene
    }

    public void Settings()
    {
        // Toggle the settings menu visibility
        if (settingsMenu != null)
        {
            isSettingsActive = !isSettingsActive;
            settingsMenu.SetActive(isSettingsActive);

            // Pause the game when settings are opened
            if (isSettingsActive)
            {
                playerMovement.SetCanMove(false); // Disable player movement
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                playerMovement.SetCanMove(true); // Enable player movement
                Time.timeScale = 1f; // Resume the game
            }
        }
        else
        {
            Debug.LogWarning("Settings menu is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings(); // Toggle the settings menu with Esc key
        }
    }
}
