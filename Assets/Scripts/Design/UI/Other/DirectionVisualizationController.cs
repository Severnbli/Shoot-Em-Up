using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionVisualiseController : MonoBehaviour
{
    [SerializeField] private GameObject _prefabForVisualization;
    [SerializeField] private bool _isPrefabForRightSide = true;
    [SerializeField] private GameObject _objectThatContainMouseController;
    [SerializeField] private GameObject _objectWhereSpawn;
    [SerializeField] private float _threshHold = 20f;
    private MouseController _mouseController;
    private List<GameObject> _directions = new List<GameObject>();
    private Vector2 _size = Vector2.zero;
    private int _oldValue;

    void Awake() {
        if (!_prefabForVisualization) {
            Debug.LogError($"Direction Visualise Controller: {gameObject.name} _prefabForVisualization is null!");
        } else {
            var imageComponent = _prefabForVisualization.GetComponent<Image>();

            if (imageComponent == null) {
                Debug.LogError($"Direction Visualise Controller: {gameObject.name} _prefabForVisualization has no component Image!");
            } else {
                _size = imageComponent.sprite.rect.size;
            }
        }

        if (!_objectThatContainMouseController) {
            Debug.LogError($"Direction Visualise Controller: {gameObject.name} _objectThatContainMouseController is null"); 
        } else {
            _mouseController = _objectThatContainMouseController.GetComponent<MouseController>();

            if (!_mouseController) {
                Debug.LogError($"Direction Visualise Controller: {gameObject.name} {_objectThatContainMouseController.name} has no component Mouse Controller!");
            }
        }

        if (!_objectWhereSpawn) {
            Debug.LogError($"Direction Visualise Controller: {gameObject.name} _objectWhereSpawn is null!");
        }
    }

    void Start() {
        _oldValue = _mouseController.GetNowValue();
    }

    void Update() {
        int newValue = _mouseController.GetNowValue();
        
        if (newValue != _oldValue) {
            HandleValue(newValue);
        }
    }

    void HandleValue(int value) {
        // Очистка старых объектов
        for (int i = _directions.Count - 1; i >= 0; i--) {
            ObjectPooling.PushObject(_directions[i]);
            _directions.RemoveAt(i);
        }

        // Создание и настройка новых объектов
        for (int i = 0; i < Mathf.Abs(value); i++) {
            var image = ObjectPooling.PopObject(_prefabForVisualization.tag, Vector3.zero);

            image.transform.SetParent(_objectWhereSpawn.transform, false); // Привязка к контейнеру

            // Проверка направления и установка масштаба для переворота
            if ((value > 0 && !_isPrefabForRightSide) || (value < 0 && _isPrefabForRightSide)) {
                image.transform.localScale = new Vector3(
                    -Mathf.Abs(image.transform.localScale.x), // Отражение по оси X
                    image.transform.localScale.y,
                    image.transform.localScale.z
                );
            } else {
                image.transform.localScale = new Vector3(
                    Mathf.Abs(image.transform.localScale.x), // Стандартный масштаб по оси X
                    image.transform.localScale.y,
                    image.transform.localScale.z
                );
            }

            // Вычисление локальной позиции объекта на основе индекса и направления
            image.transform.localPosition = new Vector3(
                Mathf.Sign(value) * 900 - Mathf.Sign(value) * i * (_size.x + _threshHold), // Смещение вдоль X для каждого объекта
                0, // Установка Y в 0, так как это в центре контейнера
                0
            );

            _directions.Add(image);
        }

        // Обновляем старое значение
        _oldValue = value;
    }
}
