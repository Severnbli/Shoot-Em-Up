using System;
using System.Collections;
using UnityEngine;

public class ShootController : Skill
{
    [SerializeField] private GameObject _projectile; // Префаб снаряда
    [SerializeField] private float _speed; // Скорость пули
    [SerializeField] private float _damageAmount = 10f;
    [SerializeField] private float _localScaleFactor = 1f;
    [SerializeField] private Transform _projectileStartTransform; // Начальное положение пули
    [SerializeField] private string[] _targetsTags;

    void FixedUpdate() {
        if(_isKeyActive) {
            if (_inputKey.IsEventTrigger()) {
                if (_isUsingAllowed && _isSkillActive) {
                    StartCoroutine(UseSkill());
                }
            }
        }
    }

    public override IEnumerator UseSkill() {
        if (!_isSkillActive) {
            yield break;
        }

        _isUsingAllowed = false;

        GameObject projectile = Instantiate(_projectile, _projectileStartTransform.position, Quaternion.identity);

        var weaponController = projectile.GetComponent<WeaponController>();

        if (weaponController) {
            foreach (var target in _targetsTags) {
                weaponController.AddTargetTag(target);
            }

            weaponController.SetStartTransform(_projectileStartTransform);
            
            projectile.GetComponent<ProjectileTypeWeaponController>()?.SetSpeed(_speed);

            weaponController.SetActualScale();
            weaponController.SetPhysics();
            weaponController.SetDamageAmount(_damageAmount);
            weaponController.SetLocalScaleFactor(_localScaleFactor);
        }

        yield return new WaitForSeconds(_delay);
        _isUsingAllowed = true;
    }
}
