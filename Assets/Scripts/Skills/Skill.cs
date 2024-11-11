using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected Utils.InputKey _inputKey;
    [SerializeField] protected bool _isKeyActive = true;
    [SerializeField] protected bool _isSkillActive = true;
    [SerializeField] protected float _delay;
    [SerializeField] protected bool _isConnectToEnergySystem = true;
    [SerializeField] protected float _energyWaste;
    protected bool _isRepeatUsing = false;
    protected bool _isUsingAllowed = true;
    protected EnergyEntityController _energyController;
    protected Rigidbody2D _rb;

    protected virtual void Awake() {
        if (_isConnectToEnergySystem) {    
            _energyController = GetComponent<EnergyEntityController>();

            if (!_energyController) {
                Debug.LogError($"Skill: {gameObject.name} has no component Energy Entity Controller!");
            }
        }

        _rb = GetComponent<Rigidbody2D>();

        if (!_rb) {
            Debug.LogWarning($"Skill: {gameObject.name} has no component Rigidbody2D!");
        }
    }

    public virtual IEnumerator UseSkill() {
        yield return null;
    }

    public virtual IEnumerator RepeatUsing() {
        while (_isRepeatUsing) {
            yield return UseSkill();
        }
    }

    protected virtual void Update() {}

    public void StartRepeatUsing() {
        if (!_isRepeatUsing) {
            _isRepeatUsing = true;

            StartCoroutine(RepeatUsing());
        }
    }

    void OnDestroy() {
        StopRepeatUsing();
    }

    public void StopRepeatUsing() {
        _isRepeatUsing = false;
    }

    public void SetIsSkillActive(bool isSkillActive) {
        _isSkillActive = isSkillActive;
    }
}
