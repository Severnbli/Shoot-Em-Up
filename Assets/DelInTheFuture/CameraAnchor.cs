using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    private GameObject _object;
    private Vector3 correctiveValue = new Vector3(0, 3, -10);

    void Start() {
        _object =  GameObject.FindGameObjectWithTag("Enemy");
    }

    void FixedUpdate() {
        transform.position = _object.transform.position + correctiveValue; 
    }
}
