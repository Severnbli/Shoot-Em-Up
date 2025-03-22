using System.Collections;
using UnityEngine;

public class ArmorController : Skill
{
    [SerializeField] private float _armorAddAmount;
    [SerializeField] private ArmorSetupType _setupType = ArmorSetupType.SET; 

    public enum ArmorSetupType {
        ADD, // Постепенное добавление брони
        SET // Установка брони. При попадании сразу снимается
    }

    private HealthController _healthController;

    protected override void Awake() {
        base.Awake();

        _healthController = GetComponent<HealthController>();

        if (_healthController == null) {
            Debug.LogWarning($"{gameObject.name} has no Health Controller component!");
        } else {
            _healthController.SetArmorSetupType(_setupType);
        }
    }

    protected override void Update() {
        base.Update();

        if (_isKeyActive) {
            if (_inputKey.IsEventTrigger()) {
                if (_isUsingAllowed && _isSkillActive) {
                    StartCoroutine(UseSkill());
                }
            }

            if (_setupType == ArmorSetupType.SET && Input.GetKeyUp(_inputKey.GetKey())) {
                if (_healthController) {
                    _healthController.SetArmor(0);
                }
            }
        }
    }

    public override IEnumerator UseSkill(){
        if (!_isSkillActive) {
            yield break;
        }

        if (_energyController) {
            if (!_energyController.IsEnoughEnergyAndWasteIfEnough(_energyWaste)) {
                yield break;
            }
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
