using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _speed = 0; // Скорость перемещения
    private Vector3 _movement; // Для упрощения кода (инкапсулирует в себе скорость и Time.deltaTime)

    void Start() {
        _movement = new Vector3(_speed * Time.deltaTime, 0, 0);
    }

    void FixedUpdate() {
        float mouseXMovement = Input.GetAxis("Mouse X"); // Получаем единицу движения мышкой
        
        if (mouseXMovement > 0) {
            gameObject.transform.position += _movement; 
        } else if (mouseXMovement < 0) {
            gameObject.transform.position -= _movement;
        }
    }
}
