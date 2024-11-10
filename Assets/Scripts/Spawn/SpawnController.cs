using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _object; // Объект для спавна
    [SerializeField] private int _quantity; // Количество объектов для спавна
    [SerializeField] private float _interval; // Обычный интервал между объектами
    [SerializeField] private float _centerInterval; // Центральный интервал (используется при симметричном спавне (нечётное число объектов))
    [SerializeField] private Vector3 _spawnStartPosition; // Центральное положение области спавна
    [SerializeField] private bool _isRealignmentModeOn = true;
    
    private Vector3 _intervalVector; // Интервал в векторной форме, для уменьшения копипаста
    private Vector3 _centerIntervalVector; // Центральный интервал в векторной форме, -//-
    private List<GameObject> _activeObjects = new List<GameObject>();

    void Awake() {
        _quantity = Mathf.Abs(_quantity);
        
        if (_quantity > 0) {
            CountIntervals();

            SpawnObjects();

            transform.position = _spawnStartPosition;

            AlignObjects();
        }
    }

    void FixedUpdate() {
        if (_isRealignmentModeOn) {
            for (int i = 0; i < _activeObjects.Count; i++) {
                if (_activeObjects[i] == null) {
                    _activeObjects.RemoveAt(i);
                    i--;

                    if (_activeObjects.Count > 0) {
                        AlignObjects();
                    }
                }
            }
        }
    }

    void AlignObjects() {
        int quantity = _activeObjects.Count;

        if (quantity <= 0) {
            return;
        }
        
        if (quantity % 2 == 0) {
            int halfQuantity = quantity / 2 - 1;

            // Берём центр и половину от центрального интервала, а затем перемещаемся в левое положение
            Vector3 position = transform.position - _centerIntervalVector / 2f - _intervalVector * halfQuantity; 

            for (int i = 0; i < quantity; i++) {
                _activeObjects[i].transform.position = position;
                if (halfQuantity == i) {
                    position = transform.position + _centerIntervalVector / 2f;
                } else {
                    position += _intervalVector;
                }
            }
        } else {
            // Двигаемся в левое положение
            Vector3 position = transform.position - _intervalVector * (quantity / 2);

            for (int i = 0; i < quantity; i++) {
                _activeObjects[i].transform.position = position;        

                position += _intervalVector;
            }
        }
    }

    void SpawnObjects() {
        for (int i = 0; i < _quantity; i++) {
            GameObject obj = Instantiate(_object, _spawnStartPosition, Quaternion.identity);

            int index = obj.name.IndexOf("(Clone)");

            string newName = obj.name.Substring(0, index);
            newName += obj.name.Substring(index + "(Clone)".Length);

            obj.name = newName + "_" + i.ToString();

            _activeObjects.Add(obj);
        }
    }

    public void Expansion(float expansionParameter) {
        if (expansionParameter != 0) {
            _interval += expansionParameter;
            _centerInterval += expansionParameter;

            CountIntervals();

            AlignObjects();
        }
    }

    private void CountIntervals() {
        _interval = Mathf.Abs(_interval);
        _centerInterval = Mathf.Abs(_centerInterval);
        
        _intervalVector = new Vector3(_interval, 0, 0);
        _centerIntervalVector = new Vector3(_centerInterval, 0, 0);       
    }

    public Vector3 GetIntervalVector() {
        return _intervalVector;
    }

    public Vector2 GetVelocity() {
        var rb = GetComponent<Rigidbody2D>();

        if (rb) {
            return rb.velocity;
        } else {
            return Vector2.zero;
        }
    }
}
