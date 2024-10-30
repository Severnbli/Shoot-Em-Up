using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _runValue;

    void FixedUpdate()
    {
        if (Input.GetKey("w") && Input.GetKey(KeyCode.LeftShift)) {
            transform.position += transform.TransformDirection(Vector3.up) * Time.deltaTime * _runValue;
        }
        else if (Input.GetKey("w")) {
            transform.position += transform.TransformDirection(Vector3.up) * Time.deltaTime * _speed;
        } else if (Input.GetKey("s")) {
            transform.position += transform.TransformDirection(Vector3.down) * Time.deltaTime * _speed;
        } else if (Input.GetKey("a")) {
            transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * _speed;
        } else if (Input.GetKey("d")) {
            transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * _speed;
        }
    }
}
