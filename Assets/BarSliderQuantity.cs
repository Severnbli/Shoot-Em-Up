using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarSliderQuantity : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;

    void Update() {
        _text.text = _slider.value.ToString();
    }    
}
