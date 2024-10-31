using System;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    [SerializeField] private float _speed; // Скорость центровки камеры

    private GameObject _enemy; // Объект противника (герой)
    private GameObject[] _player; // Наши корабли

    void Start() {
        _enemy =  GameObject.FindGameObjectWithTag("Enemy");
        _player = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate() {
        Vector3 nowPosition = transform.position;

        Vector3 nextPosition = CalculateNextPosition();

        float distance = nextPosition.x - nowPosition.x;

        if (distance != 0) {
            float step = Math.Clamp(_speed, 0f, Math.Abs(distance)); // 0f - нижний предел,  Math.Abs(distance) - верхний предел

            if (distance < 0) {
                step = -step;
            }

            transform.position = new Vector3(nowPosition.x + step * Time.deltaTime, nowPosition.y, nowPosition.z);
        }
    }

    // Находим центр между нашими кораблями и героем
    public Vector3 CalculateNextPosition() {
        float sumX = 0f;

        if (_enemy != null) {
            sumX += _enemy.transform.position.x;
        }

        int quantity = 0;
        float sumBuffer = 0f;

        foreach (GameObject player in _player) {
            if (player != null && player.activeSelf) { // Если ссылка не null и если объект активен (object pooling)
                quantity++;
                sumBuffer += player.transform.position.x;
            }
        }

        if (quantity != 0) { // Если quantity = 0, то центруемся на героя. Исключительная ситуация
            sumX += (float) sumBuffer / quantity;

            sumX /= 2f; // Среднее между средним наших кораблей и героем

        }

        return new Vector3(sumX, transform.position.y, transform.position.z);
    } 
}
