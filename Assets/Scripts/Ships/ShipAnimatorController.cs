using System.Linq;
using UnityEngine;

/**
* Этот класс служит базой для управления анимациями кораблей.
* Аниматор должен иметь две булевые переменные: isDestroyed и isArmor
*/

public class ShipAnimatorController : MonoBehaviour
{
    [SerializeField] GameObject[] _childObjects;

    private Animator _animator;

    void Awake() {
        _animator = GetComponent<Animator>();

        if (!_animator) {
            Debug.LogError(gameObject.name + " has no animator component!");
        }
    }

    public void DestroyShip(float delay) {
        Destroy(gameObject, delay);

        foreach(GameObject child in _childObjects) {
            ShipAnimatorController controller =  child?.GetComponent<ShipAnimatorController>();

            if (controller) {
                controller.SetAnimatorIsDestroyedValue(true);
            } else if (child) {
                Destroy(child, delay);
            }
        }
    }

    public void SetAnimatorIsDestroyedValue(bool value) {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();

        if (polygonCollider != null){
            Destroy(polygonCollider);
        }

        var skills = GetComponents<Skill>();

        foreach(var skill in skills) {
            skill.SetIsSkillActive(false);
        }

        _animator.SetBool("isDestroyed", value);
    }

    public void SetAnimatorIsArmorValue(bool value) {
        _animator.SetBool("isArmor", value);
    }

    public void AddChildObject(ref GameObject child) {
        _childObjects.Append(child);
    }
}
