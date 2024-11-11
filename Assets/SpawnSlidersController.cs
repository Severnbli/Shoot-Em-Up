using UnityEngine;
using UnityEngine.UI;

public class SpawnSlidersController : MonoBehaviour
{
    [SerializeField] private Slider _battlecruiserSlider;
    [SerializeField] private Slider _frigateSlider;
    [SerializeField] private Slider _bomberSlider;
    [SerializeField] private string _battlecruiserKey = "battlecruiserQuantity";
    [SerializeField] private string _frigateKey = "frigateQuantity";
    [SerializeField] private string _bomberKey = "bomberQuantity";

    void Start()
    {
        _battlecruiserSlider.value = PlayerPrefs.GetInt(_battlecruiserKey, 1);
        _frigateSlider.value = PlayerPrefs.GetInt(_frigateKey, 1);
        _bomberSlider.value = PlayerPrefs.GetInt(_bomberKey, 1);

        _battlecruiserSlider.onValueChanged.AddListener(OnBattlecruiserSliderChanged);
        _frigateSlider.onValueChanged.AddListener(OnFrigateSliderChanged);
        _bomberSlider.onValueChanged.AddListener(OnBomberSliderChanged);
    }

    private void OnBattlecruiserSliderChanged(float value)
    {
        PlayerPrefs.SetInt(_battlecruiserKey, Mathf.RoundToInt(value));
        PlayerPrefs.Save(); // Сохранение значений
    }

    private void OnFrigateSliderChanged(float value)
    {
        PlayerPrefs.SetInt(_frigateKey, Mathf.RoundToInt(value));
        PlayerPrefs.Save();
    }

    private void OnBomberSliderChanged(float value)
    {
        PlayerPrefs.SetInt(_bomberKey, Mathf.RoundToInt(value));
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Отписка от событий при уничтожении объекта
        _battlecruiserSlider.onValueChanged.RemoveListener(OnBattlecruiserSliderChanged);
        _frigateSlider.onValueChanged.RemoveListener(OnFrigateSliderChanged);
        _bomberSlider.onValueChanged.RemoveListener(OnBomberSliderChanged);
    }
}
