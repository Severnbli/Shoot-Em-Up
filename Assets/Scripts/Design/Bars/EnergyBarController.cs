using UnityEngine;

public class EnergyBarController : BarController
{
    [SerializeField] private EnergyController _energyController;

    protected override void Update()
    {
        base.Update();

        if (_energyController) {
            UpdateBar(_energyController.GetPercentage());
        }
    }
}
