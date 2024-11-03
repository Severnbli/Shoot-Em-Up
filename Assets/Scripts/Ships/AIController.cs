using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _increaseFactorForSpeed = 10f;
    [SerializeField] private float _modeWholeLineIncreaseFactorForSpeed = 2f;
    [SerializeField] private float _modePursuitLiveTime = 5f;
    [SerializeField] private float _modeDoublePenetrationLiveTime = 2f;
    [SerializeField] private float _modeWholeLineDeviation = 1f;

    [SerializeField] private OperatingModes _operationMode = OperatingModes.RANDOM;

    private Rigidbody2D _rb;
    private bool _isRandomMode = false;
    private OperatingModes _previousMode = OperatingModes.RANDOM;

    private bool _isModeActive = false;

    public enum OperatingModes {
        PURSUIT,
        WHOLE_LINE,
        DOUBLE_PENETRATION,
        RANDOM
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_rb == null) {
            Debug.LogWarning($"{gameObject.name} has no Rigidbody2D component!");
        }
    }

    void Start() {
        StartCoroutine(StartWorking());
    }

    IEnumerator StartWorking() {
        while (true) {
            switch (_operationMode) {
                case OperatingModes.PURSUIT:
                    yield return StartCoroutine(Pursuit());
                    break;

                case OperatingModes.WHOLE_LINE:
                    yield return StartCoroutine(Whole_Line());
                    break;

                case OperatingModes.DOUBLE_PENETRATION:
                    yield return StartCoroutine(Double_Penetration());
                    break;

                case OperatingModes.RANDOM:
                    _isRandomMode = true;
                    break;
            }

            if (_isRandomMode) {
                _previousMode = _operationMode;
                _operationMode = GetRandomOperatingModeButNotPrevious();
            }
            Debug.Log("Current Mode: " + _operationMode);
        }
    }

    public OperatingModes GetRandomOperatingModeButNotPrevious() {
        var modes = Enum.GetValues(typeof(OperatingModes))
                        .Cast<OperatingModes>()
                        .Where(mode => mode != OperatingModes.RANDOM && mode != _previousMode)
                        .ToArray();

        if (modes.Length == 0) return _previousMode;

        return modes[UnityEngine.Random.Range(0, modes.Length)];
    }

    IEnumerator Pursuit() {
        _isModeActive = true;

        GameObject closestPlayer = GetClosestPlayer();
        if (closestPlayer == null) yield break;

        float timer = 0f;
        while (timer < _modePursuitLiveTime && closestPlayer != null) {
            float direction = Mathf.Sign(closestPlayer.transform.position.x - transform.position.x);
            _rb.velocity = new Vector2(direction * _speed * _increaseFactorForSpeed * Time.deltaTime, 0);

            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();

            closestPlayer = GetClosestPlayer();  // Обновляем цель
        }

        _isModeActive = false;
    }

    IEnumerator Whole_Line() {
        _isModeActive = true;

        _rb.velocity = Vector2.zero;
        float direction = UnityEngine.Random.Range(0, 2) * 2 - 1;

        if (direction > 0) {
            float leftBound = GetLeftBoundPosX();
            if (leftBound == Mathf.Infinity) yield break;
            transform.position = new Vector3(leftBound - _modeWholeLineDeviation, transform.position.y);
        } else {
            float rightBound = GetRightBoundPosX();
            if (rightBound == -Mathf.Infinity) yield break;
            transform.position = new Vector3(rightBound + _modeWholeLineDeviation, transform.position.y);
        }

        _rb.velocity = new Vector2(direction * _speed * _increaseFactorForSpeed * _modeWholeLineIncreaseFactorForSpeed * Time.deltaTime, 0);

        while (!isReachedBound(direction)) {
            yield return new WaitForFixedUpdate();
        }

        _isModeActive = false;
    }

    IEnumerator Double_Penetration() {
        _isModeActive = true;

        _rb.velocity = Vector2.zero;

        var players = GameObject.FindGameObjectsWithTag("Player").OrderBy(p => p.transform.position.x).ToArray();

        if (players.Length >= 2) {
            GameObject target1 = players[UnityEngine.Random.Range(0, players.Length)];
            GameObject target2;

            do {
                target2 = players[UnityEngine.Random.Range(0, players.Length)];
            } while (target2 == target1);

            yield return StartCoroutine(MoveTowardsTargets(new List<GameObject> { target1, target2 }));
        }

        _isModeActive = false;
    }

    private GameObject GetClosestPlayer() {
        var players = GameObject.FindGameObjectsWithTag("Player");

        GameObject closestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (var player in players) {
            float distance = Mathf.Abs(transform.position.x - player.transform.position.x);
            if (distance < minDistance) {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private IEnumerator MoveTowardsTargets(List<GameObject> targets) {
        foreach (var target in targets) {
            float timer = 0f;
            while ((timer <= _modeDoublePenetrationLiveTime / 2f) && target != null) {
                transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
                Debug.Log("timer pen: " + timer + ", timer <= _modeDoublePen: " + (timer <= _modeDoublePenetrationLiveTime) + ", Live time: " + _modeDoublePenetrationLiveTime);
            }
        }
    }

    private bool isReachedBound(float direction) {
        float leftBound = GetLeftBoundPosX();
        float rightBound = GetRightBoundPosX();

        if (direction > 0) {
            return transform.position.x >= rightBound;
        } else {
            return transform.position.x <= leftBound;
        }
    }

    private float GetRightBoundPosX() {
        var playerShips = GameObject.FindGameObjectsWithTag("Player");
        if (playerShips.Length == 0) return -Mathf.Infinity;
        
        return playerShips.Max(ship => ship.transform.position.x);
    }

    private float GetLeftBoundPosX() {
        var playerShips = GameObject.FindGameObjectsWithTag("Player");
        if (playerShips.Length == 0) return Mathf.Infinity;

        return playerShips.Min(ship => ship.transform.position.x);
    }
}
