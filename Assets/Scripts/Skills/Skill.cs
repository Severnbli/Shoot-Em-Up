using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected Utils.InputKey _inputKey;
    [SerializeField] protected bool _isKeyActive = true;
    [SerializeField] protected bool _isSkillActive = true;
    [SerializeField] protected float _delay;
    [SerializeField] protected float _energyWaste;
    protected bool _isRepeatUsing = false;
    protected bool _isUsingAllowed = true;

    public virtual IEnumerator UseSkill() {
        yield return null;
    }

    public virtual IEnumerator RepeatUsing() {
        while (_isUsingAllowed) {
            yield return UseSkill();
        }
    }

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
