using UnityEngine;

public class FireballControl : MonoBehaviour
{
    void Update() {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < 0 || viewportPosition.y > 1) {
            Destroy(gameObject, 1f);
        }
    }
}
