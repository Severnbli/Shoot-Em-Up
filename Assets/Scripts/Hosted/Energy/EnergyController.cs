using System.Collections;
using UnityEngine;

public class EnergyController : HostController
{
    [SerializeField] private float _energyAmount = 100f;
    [SerializeField] private bool _isEnergyBoundOnTop = true;
    [SerializeField] private bool _isEnergyCanRecover = true;
    [SerializeField] private float _recoveryQuantity;
    [SerializeField] private float _recoveryDelay;

    private float _maxEnergy = Mathf.Infinity;
    private float _startEnergy;

    protected override void Start() {
        base.Start();

        ConnectToSubordinate();

        if (_isEnergyBoundOnTop) {
            _maxEnergy = _energyAmount;
        }

        _startEnergy = _energyAmount;
        
        StartCoroutine(RecoverEnergy());
    }

    protected override void ConnectToSubordinate() {
        base.ConnectToSubordinate();

        foreach (var subordinateObject in _subordinateObjects) {
            var energyEntityController = subordinateObject.GetComponent<EnergyEntityController>();

            if (energyEntityController) {
                energyEntityController.Connect(this);
            } else {
                Debug.LogWarning($"Energy Controller: {subordinateObject.name} has no component Energy Entity Controller!");
            }
        }
    }

    private IEnumerator RecoverEnergy() {
        while (_isEnergyCanRecover) {
            _energyAmount += _recoveryQuantity;

            if (_isEnergyBoundOnTop && _energyAmount > _maxEnergy) {
                _energyAmount = _maxEnergy;
            }

            yield return new WaitForSeconds(_recoveryDelay); 
        }
    }

    public void SetStatusOfEnergyRecoverCapability(bool isEnergyCanRecover) {
        if (_isEnergyCanRecover != isEnergyCanRecover) {
            _isEnergyCanRecover = isEnergyCanRecover;

            if (_isEnergyCanRecover) {
                StartCoroutine(RecoverEnergy());
            }
        }  
    }

    public bool IsEnoughEnergyAndWasteIfEnough(float amount) {
        if (_energyAmount - amount >= 0) {
            _energyAmount -= amount;

            return true;
        } else {
            return false;
        }
    }

    public float GetPercentage() {
        if (_isEnergyBoundOnTop) {
            return _energyAmount / _maxEnergy;
        } else {
            return _energyAmount / _startEnergy;
        }
    }
}
