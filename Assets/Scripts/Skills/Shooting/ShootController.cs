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
    [SerializeField] private bool _isProjectileHasHorizontalMovement = true; 

    protected override void Update() {
        base.Update();

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

        if (_energyController) {
            if (!_energyController.IsEnoughEnergyAndWasteIfEnough(_energyWaste)) {
                yield break;
            }
        }

        _isUsingAllowed = false;

        GameObject projectile = ObjectPooling.PopObject(_projectile.tag, _projectileStartTransform.position);

        var weaponController = projectile.GetComponent<WeaponController>();

        if (weaponController) {
            weaponController.PlayMusic();
            
            foreach (var target in _targetsTags) {
                weaponController.AddTargetTag(target);
            }

            weaponController.SetStartTransform(_projectileStartTransform);
            
            var projectileController = projectile.GetComponent<ProjectileTypeWeaponController>();

            if (projectileController) {
                projectileController.SetSpeed(_speed);

                if (_isProjectileHasHorizontalMovement) {
                    projectileController.SetVelocityX(_rb.velocity.x);
                }
            }

            weaponController.SetActualScale();
            weaponController.SetPhysics();
            weaponController.SetDamageAmount(_damageAmount);
            weaponController.SetLocalScaleFactor(_localScaleFactor);
        }

        yield return new WaitForSeconds(_delay);
        _isUsingAllowed = true;
    }

    public void SetSpeed(float speed) {
        _speed = speed;
    }

    public void SetDelay(float delay) {
        _delay = delay;
    }

    public void SetProjectile(GameObject projectile) {
        _projectile = projectile;
    }
}
