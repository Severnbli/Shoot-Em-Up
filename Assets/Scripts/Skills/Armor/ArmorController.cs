using System.Collections;
using UnityEngine;

public class ArmorController : Skill
{
    [SerializeField] private float _armorAddAmount;
    [SerializeField] private ArmorSetupType _setupType = ArmorSetupType.SET; 

    public enum ArmorSetupType {
        ADD,
        SET
    }

    private HealthController _healthController;

    void Awake() {
        _healthController = GetComponent<HealthController>();

        if (_healthController == null) {
            Debug.LogWarning($"{gameObject.name} has no Health Controller component!");
        }
    }

    void FixedUpdate() {
        if (_isKeyActive) {
            if (_inputKey.IsEventTrigger()) {
                if (_isUsingAllowed && _isSkillActive) {
                    StartCoroutine(UseSkill());
                }
            }
        }
    }

    public override IEnumerator UseSkill(){
        if (!_isSkillActive) {
            yield break;
        }

        _isUsingAllowed = false;

        if (_healthController != null) {
            switch (_setupType) {
                case ArmorSetupType.ADD: {
                    _healthController.AddArmor(_armorAddAmount);
                    break;
                }

                case ArmorSetupType.SET: {
                    _healthController.SetArmor(_armorAddAmount);
                    break;
                }

                default: {
                    Debug.LogError("Undefined armor setup type!");
                    break;
                }
            }
        }

        yield return new WaitForSeconds(_delay);
        _isUsingAllowed = true;
    }
}
