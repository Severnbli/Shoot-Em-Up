using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] LevelChanger _levelChanger;

    public void PlayButton() {
        PlayerPrefs.SetString("nextScene", "Level");

        _levelChanger.ChangeLevel();
    }

    public void MenuButton() {
        PlayerPrefs.SetString("nextScene", "MainMenu");

        _levelChanger.ChangeLevel();
    }

    public void SettingsButton() {

    }

    public void ExitButton() {
        Application.Quit();
    }
}
