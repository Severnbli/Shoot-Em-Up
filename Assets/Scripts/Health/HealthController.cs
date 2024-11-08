using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] protected float _healthAmount = 100f;
    [SerializeField] private float _armorAmount = 0f;
    [SerializeField] private bool _isHealthBoundedOnTop = true;
    [SerializeField] private bool _isHealthCanRestore = false;
    [SerializeField] private float _restoreHealthCount = 0f;
    [SerializeField] private float _restoreHealthDelay = 0f;

    private ArmorController.ArmorSetupType _armorSetupType = ArmorController.ArmorSetupType.ADD; 
    protected float _maxHealth;
    private ShipAnimatorController _shipController;

    void Awake() {
        _maxHealth = _healthAmount;

        _shipController = GetComponent<ShipAnimatorController>();

        if (_shipController == null) {
            Debug.LogWarning($"{gameObject.name} has no component Ship Animator Controller!");
        }

        if (_isHealthCanRestore) {
            StartCoroutine(RestoreHealth());
        }
    }

    public void AddDamage(float amount) {
        if (_armorAmount > 0) {
            switch (_armorSetupType) {
                case ArmorController.ArmorSetupType.ADD: {
                    _armorAmount -= amount;

                    if (_armorAmount <= 0) {
                        _healthAmount += _armorAmount;
                        _armorAmount = 0;

                        _shipController.SetAnimatorIsArmorValue(false);
                    }

                    break;
                }

                case ArmorController.ArmorSetupType.SET: {
                    _armorAmount = 0;
                    
                    _shipController.SetAnimatorIsArmorValue(false);

                    break;
                }

                default: {
                    Debug.LogError("Undefined armor setup type!");
                    break;
                }
            }
        } else {
            _healthAmount -= amount;
        }

        if (_healthAmount <= 0) {
            DeathController.Death(gameObject);
        }
    }

    public void AddHeal(float amount) {
        float newHealth = _healthAmount + amount;
        
        if (_isHealthBoundedOnTop) {
            _healthAmount = newHealth > _maxHealth ? _maxHealth : newHealth;
        } else {
            _healthAmount = newHealth;

            if (_healthAmount > _maxHealth) {
                _maxHealth = _healthAmount;
            }
        }
    }

    public void AddArmor(float amount) {
        _armorAmount += amount;

        _shipController.SetAnimatorIsArmorValue(_armorAmount > 0);
    }

    public void SetArmor(float amount) {
        _armorAmount = amount;
   
        _shipController.SetAnimatorIsArmorValue(_armorAmount > 0);
    }

    public void SetArmorSetupType(ArmorController.ArmorSetupType armorSetupType) {
        _armorSetupType = armorSetupType;
    }

    private IEnumerator RestoreHealth() {
        while (_isHealthCanRestore) {
            AddHeal(_restoreHealthCount);

            yield return new WaitForSeconds(_restoreHealthDelay);
        } 
    }

    public void SetIsHealthCanRestore(bool isHealthCanRestore) {
        if (_isHealthCanRestore != isHealthCanRestore) {
            _isHealthCanRestore = isHealthCanRestore;

            if (_isHealthCanRestore) {
                StartCoroutine(RestoreHealth());
            }
        }        
    }

    public float GetHealthPercentage() {
        return _healthAmount / _maxHealth;
    }
}
