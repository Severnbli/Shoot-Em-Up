using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private KeyCode _pauseButton;
    [SerializeField] private LevelChanger _levelChanger;

    private bool _isPaused = false;

    private void Update() {
        if (Input.GetKeyDown(_pauseButton)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }

    public void ResumeGame() {
        _isPaused = false;
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu() {
        PlayerPrefs.SetString("nextScene", "MainMenu");

        _levelChanger.LoadNextLevel();

        Time.timeScale = 1;
    }

    public void RetryGame() {
        PlayerPrefs.SetString("nextScene", "Level");

        _levelChanger.LoadNextLevel();

        Time.timeScale = 1;
    }
}
