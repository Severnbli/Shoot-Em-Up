using System.Collections.Generic;
using UnityEngine;

public class WeaponDetectorController : MonoBehaviour
{
    [SerializeField] List<string> _detectingTags;
    [SerializeField] GameObject _objectThatContainsWeaponTriggerable;
    WeaponTriggerable _triggerable;

    void Awake() {
        if (_objectThatContainsWeaponTriggerable == null) {
            Debug.LogError($"Weapon Detector Controller: {gameObject.name} _objectThatContainsWeaponTriggerable not found!");
        } else {
            var components = _objectThatContainsWeaponTriggerable.GetComponents<Component>();

            foreach (var component in components) {
                if (component is WeaponTriggerable weaponTriggerable) {
                    _triggerable = weaponTriggerable;
                }
            }

            if (_triggerable == null) {
                Debug.LogError($"Weapon Detector Controller: {gameObject.name} _objectThatContainsWeaponTriggerable has no such component!");
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider) {
        if (_triggerable == null) {
            return;
        }

        var targetsTags = collider.GetComponent<WeaponController>()?.GetTargetsTags();

        if (targetsTags != null) {
            foreach (var targetTag in targetsTags) {
                if (_detectingTags.Contains(targetTag)) {
                    _triggerable.OnWeaponTrigger(collider.gameObject, targetTag);
                    break;
                }
            }
        }
    }
}