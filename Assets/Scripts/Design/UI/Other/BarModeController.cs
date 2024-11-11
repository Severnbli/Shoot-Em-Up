using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarModeController : MonoBehaviour
{
    [SerializeField] private Image _nowModeImage;
    [SerializeField] private Image _nextModeImage;
    [SerializeField] List<ModeSpritePair> _modeSpritePairs;
    [SerializeField] private AIController _aiController;
    private Dictionary<AIMode.Modes, Sprite> _sprites;

    void Awake() {
        _sprites = new Dictionary<AIMode.Modes, Sprite>();
        foreach (var pair in _modeSpritePairs) {
            if (!_sprites.ContainsKey(pair._mode)) {
                _sprites.Add(pair._mode, pair._sprite);
            }
        }
    }

    void Update() {
        _nowModeImage.sprite = _sprites[_aiController.GetNowMode().Mode];

        _nextModeImage.sprite = _sprites[_aiController.GetNextMode().Mode];
    }
}
