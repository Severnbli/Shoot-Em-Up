using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : HostController
{
    [SerializeField] private float _energyAmount = 100f;
    [SerializeField] private bool _isEnergyBoundOnTop = true;
    [SerializeField] private bool _isEnergyCanRecover = true;
    [SerializeField] private float _recoveryQuantity;
    [SerializeField] private float _recoveryDelay;
    private List<string> _energyGroups = new List<string>();
    private List<string> _groupsThatUseEnergy = new List<string>();

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

    protected void LateUpdate() {
        _groupsThatUseEnergy.RemoveRange(0, _groupsThatUseEnergy.Count);
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

    public bool IsEnoughEnergyAndWasteIfEnough(float amount, string group) {
        if (group == null || group == "") {
            return isWasteSuccess(amount);
        }

        if (!_energyGroups.Contains(group)) {
            _energyGroups.Add(group);
        }

        if (_groupsThatUseEnergy.Contains(group)) {
            return true;
        } else {
            if (isWasteSuccess(amount)) {
                _groupsThatUseEnergy.Add(group);
                return true;
            } else {
                return false;
            }
        }
    }

    private bool isWasteSuccess(float amount) {
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
