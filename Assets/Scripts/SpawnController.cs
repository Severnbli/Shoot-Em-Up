using System;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _obj; // Объект для спавна
    
    [SerializeField] private int _quantity; // Количество объектов для спавна

    [SerializeField] private float _interval; // Обычный интервал между объектами
    
    [SerializeField] private float _centerInterval; // Центральный интервал (используется при симметричном спавне (нечётное число объектов))
    [SerializeField] private Vector3 _spawnStartPosition; // Центральное положение области спавна

    private Vector3 _intervalVector; // Интервал в векторной форме, для уменьшения копипаста
    private Vector3 _centerIntervalVector; // Центральный интервал в векторной форме, -//-

    void Start() {
        _quantity = Math.Abs(_quantity);
        
        if (_quantity > 0) {
            _interval = Math.Abs(_interval);
        _centerInterval = Math.Abs(_centerInterval);
        
        _intervalVector = new Vector3(_interval, 0, 0);
        _centerIntervalVector = new Vector3(_centerInterval, 0, 0);

        if (_quantity % 2 == 0) {
            SymmetricalSpawn();
        } else {
            AsymmetricalSpawn();
        }
        }
    }

    void SymmetricalSpawn() {
        int halfQuantity = _quantity / 2 - 1;

        // Берём центр и половину от центрального интервала, а затем перемещаемся в левое положение
        Vector3 position = _spawnStartPosition - _centerIntervalVector / 2f - _intervalVector * halfQuantity; 

        for (int i = 0; i < _quantity; i++) {
            SpawnObject(position, i);

            if (halfQuantity == i) {
                position = _spawnStartPosition + _centerIntervalVector / 2f;
            } else {
                position += _intervalVector;
            }
        }
    }

    void AsymmetricalSpawn() {
        // Двигаемся в левое положение
        Vector3 position = _spawnStartPosition - _intervalVector * (_quantity / 2);

        for (int i = 0; i < _quantity; i++) {
            SpawnObject(position, i);

            position += _intervalVector;
        }
    }

    void SpawnObject(Vector3 position, int number) {
        GameObject obj = Instantiate(_obj, position, Quaternion.identity);

        obj.name += "_" + number;
    }
}
