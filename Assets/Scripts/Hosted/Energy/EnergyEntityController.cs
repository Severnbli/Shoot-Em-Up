using UnityEngine;

public class EnergyEntityController : MonoBehaviour
{
    [SerializeField] private string _energyGroup;
    private EnergyController _bossObject;

    public void Connect(EnergyController boss) {
        _bossObject = boss;
    }

    public bool IsEnoughEnergyAndWasteIfEnough(float amount) {
        if (_bossObject) {
            return _bossObject.IsEnoughEnergyAndWasteIfEnough(amount, _energyGroup);
        } else {
            Debug.LogWarning($"Energy Entity Controller: {gameObject.name} has no boss object!");
            return false;
        }
    }
}
