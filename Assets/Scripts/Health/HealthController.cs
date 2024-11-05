using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] protected float _healthAmount = 100f;
    [SerializeField] private float _armorAmount = 0f;
    [SerializeField] private bool _isHealthBoundedOnTop = true;

    protected float _maxHealth;
    private ShipAnimatorController _shipController;

    void Awake() {
        _maxHealth = _healthAmount;

        _shipController = GetComponent<ShipAnimatorController>();

        if (_shipController == null) {
            Debug.LogWarning($"{gameObject.name} has no component Ship Animator Controller!");
        }
    }

    public void AddDamage(float amount) {
        float newHealth = _healthAmount + _armorAmount - amount;

        if (newHealth >= _healthAmount) {
            _armorAmount = newHealth - _healthAmount;
        } else {
            _armorAmount = 0;

            _shipController.SetAnimatorIsArmorValue(false);

            _healthAmount = newHealth;
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
        }
    }

    public void AddArmor(float amount) {
        _armorAmount += amount;

        if (_armorAmount > 0) {
            _shipController.SetAnimatorIsArmorValue(true);
        }
    }

    public void SetArmor(float amount) {
        _armorAmount = amount;

        if (_armorAmount > 0) {
            _shipController.SetAnimatorIsArmorValue(true);
        }
    }
}
