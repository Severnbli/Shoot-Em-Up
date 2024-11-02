using System.Collections.Generic;
using UnityEngine;

public class FireballControl : MonoBehaviour
{
    [SerializeField] private float _damageCount;

    // Объекты, на которые не распространяется действие пули. Важно: необходим тег на объектах.
    [SerializeField] private GameObject[] _friendlyObjects;
    [SerializeField] private float _destroyDelayOutsideCamera = 1f; // Задержка уничтожения пули, когда она выходит за пределы камеры
    [SerializeField] private bool _isItSelfFriendly = false; // Дружественен к объектам с таким же тегом, как у него

    private List<string> _friendlyTags = new List<string>();

    void Awake() {
        if (_isItSelfFriendly) {
            _friendlyTags.Add(gameObject.tag);
        }

        foreach (GameObject frienflyObject in _friendlyObjects) {
            _friendlyTags.Add(frienflyObject.tag);

            if (frienflyObject.tag == "Untagged") { // У объекта отсутствует тег
                Debug.LogWarning("FireballControll: firendly object has no tag!");
            }
        }
    }

    void Update() {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < 0 || viewportPosition.y > 1) {
            Destroy(gameObject, _destroyDelayOutsideCamera);
        }
    }

    void OnTriggerEnter2D(Collider2D  collider) {
        if (!_friendlyTags.Contains(collider.gameObject.tag)) {
            HealthController healthController = collider.gameObject.GetComponent<HealthController>();

            if (healthController) {
                healthController.AddDamage(_damageCount);

                Destroy(gameObject);
            }
        }
    }
}
