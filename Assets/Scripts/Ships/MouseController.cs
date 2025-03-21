using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
* Этот класс призван обеспечить возможность перемещения для сил игрока
* Для правильной работы этого скрипта у объекта должен быть компонент Rigidbody2D
*/

public class MouseController : MonoBehaviour
{
    [SerializeField] private List<string> _movingObjectsTags;
    [SerializeField] private float _maxSpeed = 0; // Скорость перемещения
    [SerializeField] private ModificationTypes _modificationType;
    [SerializeField] private int _quantityOfCursorZones = 9; // Количество зон для курсора. Минимально 3
    [SerializeField] private int _quantityOfCenterZones = 3; // Количество зон для бездействия. Минимально 1
    [SerializeField] private float _degreeGround = 2f; // Основание степени (для ModificationTypes.DEGREE)
    [SerializeField] protected bool _isConnectToEnergySystem = true;
    [SerializeField] private float _energyWaste;
    
    protected EnergyEntityController _energyController;
    private List<Rigidbody2D> _rbs = new List<Rigidbody2D>();

    private List<int> _centerZonesSpecificValues; // Специальные значения центральных зон
    [SerializeField] private float _increaseFactorForSpeed = 10f;
    private int _nowValue;

    private enum ModificationTypes { // Вид функции, по которой будет считаться текущая скорость
        DIVISION,
        DEGREE
    }

    void Awake() {
        if (_quantityOfCursorZones < 3 || _quantityOfCenterZones < 1 || _quantityOfCursorZones - _quantityOfCenterZones < 2 ||
            (_quantityOfCursorZones % 2 == 0 && _quantityOfCenterZones % 2 != 0) ||
            (_quantityOfCursorZones % 2 != 0 && _quantityOfCenterZones % 2 == 0)) {
            Debug.LogError(gameObject.name + " component: ShipMouseController: zones values!");
        }

        if (_isConnectToEnergySystem) {    
            _energyController = GetComponent<EnergyEntityController>();

            if (!_energyController) {
                Debug.LogError($"Mouse Controller: {gameObject.name} has no component Energy Entity Controller!");
            }
        }
    }

    void Start() {
        foreach (var movingObjectTag in _movingObjectsTags) {
            var movingObjects = GameObject.FindGameObjectsWithTag(movingObjectTag);

            foreach (var movingObject in movingObjects) {
                var rb = movingObject.GetComponent<Rigidbody2D>();

                if (rb == null) {
                    Debug.LogWarning($"Mouse Controller: {movingObject.name} has no component Rigidbody2D!");
                } else {
                    _rbs.Add(rb);
                }
            }
        }
    }

    void Update() {
        _centerZonesSpecificValues = GetCenterZonesValues();

        int specificValue = GetNowSpecificValue();

        if (specificValue < 0) {
            Debug.LogError("Bad cursor zone detect!");
        } else {
            ProcessSpecificValue(specificValue);
        }
    }

    public int GetNowSpecificValue() {
        float mousePosition = Input.mousePosition.x;
        
        if (mousePosition < 0) {
            return 1;
        }

        if (mousePosition > Screen.width) {
            return _quantityOfCursorZones;
        }

        float step = Screen.width / (float) _quantityOfCursorZones; // Единичный шаг ширины (относительно количества зон)

        for (int i = 1; i <= _quantityOfCursorZones; i++) {
            if ((float) step * i / mousePosition >= 1) { // Если текущее положение находится левее текущего "шага"
                return i;
            }
        }

        return -1;
    }

    private void ProcessSpecificValue(int value) {
        foreach (int val in _centerZonesSpecificValues) {
            if (value == val) { // Значение попадает в центральные значения
                SetVelocity(0); // Прекратить движение
                _nowValue = 0;
                return;
            }
        }

        int maximumCenterValue = _centerZonesSpecificValues.Max();

        // Максимальный коэфициент воздействия
        int maximumInfluenceValue = (_quantityOfCursorZones - _quantityOfCenterZones) / 2;

        if (value > maximumCenterValue) { // Правее центральных
            value -= maximumCenterValue;
        } else if (value < maximumCenterValue) { // Левее центральных
            value = value - maximumInfluenceValue - 1;
        }

        _nowValue = value;

        if (_energyController && !_energyController.IsEnoughEnergyAndWasteIfEnough(_energyWaste * Mathf.Abs(value) / maximumInfluenceValue)) {
            SetVelocity(0);
            return;
        }

        switch(_modificationType) {
            case ModificationTypes.DIVISION: {
                DivisionMovement(value, maximumInfluenceValue);
                break;
            }

            case ModificationTypes.DEGREE: {
                DegreeMovement(value, maximumInfluenceValue);
                break;
            }

            default: {
                Debug.LogWarning("No valid modification type!");
                break;
            }
        }
    }

    public List<int> GetCenterZonesValues() {
        List<int> values = new List<int>();

        // Количество зон, в которых действует эффект. Всегда чётное (правила в void Start())
        int quantityOfAffectedZones = _quantityOfCursorZones - _quantityOfCenterZones; 

        // Значение первой зоны бездействия (начиная отсчёт слева)
        int value = quantityOfAffectedZones / 2 + 1;

        // Определяем все "специфические" значения центральных зон
        for (int i = 1; i <= _quantityOfCenterZones; i++, value++) {
            values.Add(value);
        } 

        return values;
    }

    private void DegreeMovement(int value, int maximumValue) {
        float singleStep = _maxSpeed / (float) Mathf.Pow(_degreeGround, maximumValue);

        if (value < 0) {
            singleStep = -singleStep;
        }

        SetVelocity(singleStep * (float) Mathf.Pow(_degreeGround, Mathf.Abs(value)));
    }

    private void DivisionMovement(int value, int maximumValue) {
        float singleStep = _maxSpeed / maximumValue;

        SetVelocity(singleStep * value);
    }

    private void SetVelocity(float velocityX) {
        foreach (var rb in _rbs) {
            if (rb) {
                rb.velocity = new Vector2(velocityX * _increaseFactorForSpeed * Time.deltaTime, 0);
            }
        }
    }

    public int GetNowValue() {
        return _nowValue;
    }
}
