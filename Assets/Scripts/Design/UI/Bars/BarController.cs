using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Image _fillLine;

    protected virtual void Start() {
        if (_sprites.Count < 1) {
            Debug.LogWarning("No images for fill line bar!");
        }
    }

    protected virtual void Update() {}

    protected virtual void FixedUpdate() {}
    
    protected void UpdateBar(float percentage) { // minimum = 0; maximum = 1
        if (percentage < 0 || percentage > 1) {
            Debug.LogWarning("UpdateBar: percentage may not be < 0 or > 1!");
            return;
        }

        int index = Mathf.CeilToInt(percentage * _sprites.Count) - 1; // На основе percentage высчитывается индекс спрайта

        _fillLine.sprite = _sprites[Mathf.Clamp(index, 0, _sprites.Count - 1)];
    }
}
