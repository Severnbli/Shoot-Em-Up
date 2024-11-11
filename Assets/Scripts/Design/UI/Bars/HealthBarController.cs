using UnityEngine;

public class HealthBarController : BarController
{
    [SerializeField] private string _targetTag;

    private HealthController _healthController;

    protected override void Start()
    {
        base.Start();

        _healthController = GameObject.FindGameObjectWithTag(_targetTag)?.GetComponent<HealthController>();

        if (_healthController == null) {
            Debug.LogError($"{gameObject.name}: Health Bar Controller: target object not found or not has Health Controller!");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_healthController) {
            UpdateBar(_healthController.GetHealthPercentage());
        }
    }
}
