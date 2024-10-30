using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject[] _removeParts; // Части, которые нужно удалить при начале анимации взрыва

    [SerializeField] private ExplosionController[] _childrenExplosion;

    public void DestroyPart(int index) {
        if (_removeParts != null && index >= 0 && index < _removeParts.Length)
                Destroy(_removeParts[index]);
            else
                Debug.LogWarning("Index is out of range in DestroyPart. index: " + index);
    }

    public void DestroyParts() {
        if (_removeParts != null && _removeParts.Length > 0) {
            foreach (GameObject child in _removeParts) {
                Destroy(child);
            }
        }
    }

    public void StartExplosion() {
        Animator animator = GetComponent<Animator>();

        if (animator == null) {
            Debug.LogWarning("Object that you tries explode doesn't have Animator component");
            return;
        }

        DestroyParts();

        animator.SetBool("isDestroyed", true);

        ChildsExplosion();
    }

    // Вызывается в анимации последним кадром
    public void DestroyObject() {
        Destroy(gameObject);
    }

    public void ChildExplosion(int index) { 
        if (_childrenExplosion != null && index >= 0 && index < _childrenExplosion.Length) {
            _childrenExplosion[index].StartExplosion();
        }
    }

    public void ChildsExplosion() {
        if (_childrenExplosion != null && _childrenExplosion.Length > 0) {
            foreach (ExplosionController child in _childrenExplosion) {
                child.StartExplosion();
            }
        }
    }
}
