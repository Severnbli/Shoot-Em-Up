using System.Collections.Generic;
using UnityEngine;

public class WeaponDetectorController : MonoBehaviour
{
    [SerializeField] List<string> _detectingTags; // Цели, которые назначены в снаряде (для детекта - наш тег)
    private bool _isTriggered = false;
    
    void LateUpdate() {
        _isTriggered = false;
    }

    protected void OnTriggerEnter2D(Collider2D collider) {
        var targetsTags = collider.GetComponent<WeaponController>()?.GetTargetsTags();

        if (targetsTags != null) {
            foreach (var targetTag in targetsTags) {
                if (_detectingTags.Contains(targetTag)) {
                    _isTriggered = true;
                }
            }
        }
    }

    public bool IsTriggered() {
        return _isTriggered;
    }
}
