using UnityEngine;

public class EnemyHealthController : HealthController
{
    [SerializeField] private GameObject _healthBarObject;

    private Bar _healthBar;

    void Start() {
        _healthBar = _healthBarObject?.GetComponent<Bar>();

        if (!_healthBar) {
            Debug.LogError("EnemyHealthController: no component Bar!");
        }
    }

    void FixedUpdate() {
        if(_healthAmount >= _maxHealth) {
            _healthBarObject.SetActive(false);
        } else {
            _healthBarObject.SetActive(true);

            _healthBar.UpdateBar(_healthAmount / _maxHealth);
        }
    }
}
