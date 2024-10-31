using System;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    [SerializeField] private float _speed; // Скорость центровки камеры

    private GameObject _enemy; // Объект противника (герой)
    private GameObject[] _playerShips; // Наши корабли

    void Start() {
        _enemy =  GameObject.FindGameObjectWithTag("Enemy");
        _playerShips = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate() {
        Utils.horizontalSmoothlyMove(_speed, CalculateNextPosition().x, transform);
    }

    // Находим центр между нашими кораблями и героем
    public Vector3 CalculateNextPosition() {
        float sumX = 0f;

        if (_enemy != null) {
            sumX += _enemy.transform.position.x;
        }

        int quantity = 0;
        float sumBuffer = 0f;

        foreach (GameObject ship in _playerShips) {
            if (ship != null && ship.activeSelf) { // Если ссылка не null и если объект активен (object pooling)
                quantity++;
                sumBuffer += ship.transform.position.x;
            }
        }

        if (quantity != 0) { // Если quantity = 0, то центруемся на героя. Исключительная ситуация
            sumX += (float) sumBuffer / quantity;

            sumX /= 2f; // Среднее между средним наших кораблей и героем

        }

        return new Vector3(sumX, transform.position.y, transform.position.z);
    } 
}
