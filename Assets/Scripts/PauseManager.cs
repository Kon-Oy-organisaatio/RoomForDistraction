using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused = false;
    [HideInInspector]
    public bool isGameOver = false;
    [Tooltip("Canvas to enable when paused")]
    public Canvas pauseMenuCanvas;

    private void LateUpdate ()
    {
        if (isGameOver) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        //pauseMenuCanvas.enabled = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        //pauseMenuCanvas.enabled = false;
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        pauseMenuCanvas.enabled = false;
        isGameOver = true;
    }

    private void OnDisable()
    {
        ResumeGame();
    }

    private void OnDestroy()
    {
        ResumeGame();
    }
}   