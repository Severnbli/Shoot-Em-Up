using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private GameObject _fillLine;
    [SerializeField] private GameObject _background;
    private SpriteRenderer _fillLineSpriteRenderer;

    void Start() {
        if ((_fillLineSpriteRenderer = _fillLine?.GetComponent<SpriteRenderer>()) == null) {
            Debug.LogError("Error with fill line object!");
        }

        if (_sprites.Count < 1) {
            Debug.LogError("No images for fill line bar!");
        } else {
            if (_fillLineSpriteRenderer) {
                _fillLineSpriteRenderer.sprite = _sprites[0];
            } 
        }
    }
    
    public void UpdateBar(float percentage) { // minimum = 0; maximum = 1
        if (percentage < 0 || percentage > 1) {
            Debug.LogWarning("UpdateBar: percentage may not be < 0 or > 1!");
            return;
        }

        int index = Mathf.CeilToInt(percentage * _sprites.Count) - 1; // На основе percentage высчитывается индекс спрайта

        if (_fillLineSpriteRenderer) {
            _fillLineSpriteRenderer.sprite = _sprites[index];

            Vector3 newScale = new Vector3(percentage, _fillLine.transform.localScale.y, _fillLine.transform.localScale.z);

            _fillLine.transform.localScale = newScale;
            _background.transform.localScale = newScale;
        }
    }
}
