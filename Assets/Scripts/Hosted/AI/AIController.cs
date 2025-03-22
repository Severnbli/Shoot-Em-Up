using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : HostController
{
    [SerializeField] private List<AIMode> _modes;
    [SerializeField] private bool _isModeCanRepeatLast = true;
    [SerializeField] private float _startAlertDelay = 3f;
    [SerializeField] private GameObject _alertableObject;
    
    private AIMode _nowMode;
    private AIMode _nextMode;
    private AIMode _lastRandomingMode;
    private Alertable _alertable;

    void Awake() {
        if (_modes.Count > 0) {
            _nowMode = GetRandomMode();
            _nextMode = GetRandomMode();

            StartCoroutine(PrepareWorking());
        }

        _alertable = _alertableObject.GetComponent<Alertable>();

        if (_alertable == null) {
            Debug.LogWarning($"AI Controller: {gameObject.name} has no Alertable object.");
        }
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

    public AIMode GetNowMode() {
        return _nowMode;
    }

    public AIMode GetNextMode() {
        return _nextMode;
    }

    private IEnumerator PrepareWorking() {
        while (true) {
            Debug.Log("Now mode: " + _nowMode.Mode);
            yield return new WaitForSeconds(Mathf.Clamp(_nowMode._nowDuration - _startAlertDelay, 0, _nowMode._nowDuration));

            if (_alertable != null) {
                StartCoroutine(_alertable.Alert(_startAlertDelay));
            }
            yield return new WaitForSeconds(_startAlertDelay);

            _nowMode = _nextMode;
            
            _nextMode = GetRandomMode();
        }
    }

    public AIMode GetRandomMode() {
        if (_modes.Count == 0) {
            return null;
        }
         
        var nowModeStack = _modes.Select(item => (AIMode)item.Clone()).ToList();

        if (!_isModeCanRepeatLast && nowModeStack.Count > nowModeStack.Count(x => x.Mode == _lastRandomingMode.Mode)) {
            nowModeStack.RemoveAll(x => x.Mode == _lastRandomingMode.Mode);
        }

        var sumProbability = nowModeStack.Sum(x => x.Probability);

        var randValue = Random.Range(0f, sumProbability);

        float nowProbability = 0f;

        var selectedMode = nowModeStack[Random.Range(0, nowModeStack.Count)];

        foreach (var mode in nowModeStack) {
            nowProbability += mode.Probability;

            if (nowProbability > randValue) {
                selectedMode = mode;
                break;
            } else if (nowProbability < randValue) {
                continue;
            } else {
                selectedMode = mode;
                break;
            }
        }

        selectedMode.RandomizeNowDuration();
        _lastRandomingMode = selectedMode;

        return selectedMode;
    }
}
