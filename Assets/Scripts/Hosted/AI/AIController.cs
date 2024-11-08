using UnityEngine;

public class AIController : HostController
{
    [SerializeField] private OperatingModes _operationMode = OperatingModes.RANDOM;

    public enum OperatingModes {
        PURSUIT,
        WHOLE_LINE,
        DOUBLE_PENETRATION,
        RANDOM
    }

    protected override void Start()
    {
        base.Start();

        ConnectToSubordinate();
    }

    protected override void ConnectToSubordinate() {
        base.ConnectToSubordinate();

        foreach (var subordinateObject in _subordinateObjects) {
            var aiEntityController = subordinateObject.GetComponent<AIEntityController>();

            if (aiEntityController) {
                aiEntityController.Connect(this);
            } else {
                Debug.LogWarning($"AI Controller: {subordinateObject.name} has no component AI Entity Controller!");
            }
        }
    }

    public OperatingModes GetOperatingMode() {
        return _operationMode;
    }
}
