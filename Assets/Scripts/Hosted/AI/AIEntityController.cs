using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIEntityController : MonoBehaviour, WeaponTriggerable
{
    [SerializeField] private WeaponDetectorController _detector;
    [SerializeField] private List<string> _targetsTags;
    [SerializeField] private float _dodgeThreshold = 1f;
    [SerializeField] private float _dodgeDelay;
    [SerializeField] private float _flyBetweenThreshold = 1f;
    private Vector3 _size;
    private bool _isDodjing = false;
    private AIController _bossObject;
    private Rigidbody2D _rb;
    private float _checkTimeForDodgeDelay = 0;
    private ShootController _shootController;

    void Awake() {
        if (!_detector) {
            Debug.LogWarning($"AI Entity Controller: {gameObject.name} Weapon Detector Controller not found!");
        }

        _rb = GetComponent<Rigidbody2D>();

        if (!_rb) {
            Debug.LogError($"AI Entity Controller: {gameObject.name}: component Rigidbody2D not found!");
        }

        var renderer = GetComponent<SpriteRenderer>();

        if (renderer == null) {
            Debug.LogWarning($"AI Entity Controller: {gameObject.name}: component Sprite Renderer not found!");
        } else {
            _size = renderer.bounds.size;
        }

        _shootController = GetComponent<ShootController>();

        if (_shootController == null) {
            Debug.LogWarning($"AI Entity Controller: {gameObject.name}: component Shoot Controller not found!");
        }
    }

    void Start() {
        StartCoroutine(PerformMode());
    }

    public void Connect(AIController boss) {
        _bossObject = boss;
    }

    public void OnWeaponTrigger(GameObject trigger, string targetTag) {
        if (_bossObject != null && _bossObject.GetNowMode() != null && _bossObject.GetNowMode().IsEntityCanDodge && Time.time - _checkTimeForDodgeDelay > _dodgeDelay) {
            StartCoroutine(PerformDodge(trigger, targetTag));
        }
    }

    private IEnumerator PerformMode() {
        while(!_bossObject) {
            yield return null;
        }

        _shootController?.StartRepeatUsing();

        while(true && Utils.GetRandomObjectWithTag("Player")) {
            if (_shootController) {
                _shootController.SetDelay(_bossObject.GetNowMode().EntityShootDelay);
                _shootController.SetSpeed(_bossObject.GetNowMode().EntityShootSpeed);
                _shootController.SetProjectile(_bossObject.GetNowMode().EntityAttackPrefab);
            }

            switch(_bossObject.GetNowMode().Mode) {
                case AIMode.Modes.PURSUIT: {
                    if (_targetsTags.Count > 0) {
                        var target = Utils.GetRandomObjectWithTag(_targetsTags[Random.Range(0, _targetsTags.Count)]);

                        if (target) {
                            while(target != null && _bossObject.GetNowMode().Mode == AIMode.Modes.PURSUIT) {
                                if (!_isDodjing) {
                                    PursuitMode(target);
                                }
                                yield return new WaitForFixedUpdate();
                            }
                        }
                    }
                    
                    break;
                }

                case AIMode.Modes.FREE: {
                    float startDirection = Mathf.Sign(Random.Range(0, 2) * 2 - 1);

                    while(_bossObject.GetNowMode().Mode == AIMode.Modes.FREE && Utils.GetRandomObjectWithTag("Player")) {
                        if (!_isDodjing) {
                            FlyBetweenBorders(ref startDirection);
                        }
                        yield return new WaitForFixedUpdate();
                    }

                    break;
                }

                case AIMode.Modes.MEGAFIRE: {
                    float startDirection = Mathf.Sign(Random.Range(0, 2) * 2 - 1);

                    while(_bossObject.GetNowMode().Mode == AIMode.Modes.MEGAFIRE && Utils.GetRandomObjectWithTag("Player")) {
                        if (!_isDodjing) {
                            FlyBetweenBorders(ref startDirection);
                        }
                        yield return new WaitForFixedUpdate();
                    }

                    break;
                }
            }
        }

        _shootController?.StopRepeatUsing();
            
        _rb.velocity = Vector2.zero;
    }

    private IEnumerator PerformDodge(GameObject trigger, string targetTag) {
        _checkTimeForDodgeDelay = Time.time;
        _isDodjing = true;

        Vector3 targetPosition = transform.position;
        bool isDodgeFromOne = false;
        float targetSizeX = trigger.GetComponent<SpriteRenderer>()?.bounds.size.x ?? 0f;

        if (trigger && trigger.tag != "Untagged") {
            List<GameObject> allSuchTriggers = new List<GameObject>(GameObject.FindGameObjectsWithTag(trigger.tag));

            allSuchTriggers.RemoveAll(x => x.GetComponent<WeaponController>()?.GetTargetsTags().Contains(targetTag) != true);

            if (allSuchTriggers.Count > 1) {
                allSuchTriggers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

                float minDistanceToInterval = float.MaxValue;
                float requiredInterval = _size.x + 2 * _dodgeThreshold + targetSizeX;
                bool intervalFound = false;

                // Iterate through adjacent pairs of bullets to find nearest suitable interval
                for (int i = 0; i < allSuchTriggers.Count - 1; i++) {
                    float distanceBetween = Mathf.Abs(allSuchTriggers[i + 1].transform.position.x - allSuchTriggers[i].transform.position.x);

                    // If interval is wide enough for a dodge
                    if (distanceBetween >= requiredInterval) {
                        intervalFound = true;

                        // Calculate midpoint of the interval
                        float intervalCenterX = (allSuchTriggers[i].transform.position.x + allSuchTriggers[i + 1].transform.position.x) / 2;
                        float distanceToInterval = Mathf.Abs(transform.position.x - intervalCenterX);

                        // Choose the nearest interval
                        if (distanceToInterval < minDistanceToInterval) {
                            minDistanceToInterval = distanceToInterval;
                            targetPosition.x = intervalCenterX;
                        }
                    }
                }

                if (!intervalFound) {
                    // No interval found, move to the closest edge of the formation
                    float leftEdge = allSuchTriggers.Min(x => x.transform.position.x) - (_size.x / 2f + _dodgeThreshold + targetSizeX / 2f);
                    float rightEdge = allSuchTriggers.Max(x => x.transform.position.x) + (_size.x / 2f + _dodgeThreshold + targetSizeX / 2f);

                    if (Mathf.Abs(transform.position.x - leftEdge) < Mathf.Abs(transform.position.x - rightEdge)) {
                        targetPosition.x = leftEdge; 
                    } else if (Mathf.Abs(transform.position.x - leftEdge) > Mathf.Abs(transform.position.x - rightEdge)) {
                        targetPosition.x = rightEdge;
                    } else {
                        float randValue = Mathf.Sign(Random.Range(0, 2) * 2 -1);

                        if (randValue > 0) {
                            targetPosition.x = leftEdge; 
                        } else {
                            targetPosition.x = rightEdge;
                        }
                    }
                }
            } else {
                isDodgeFromOne = true;
            }
        } else {
            isDodgeFromOne = true;
        }

        if (isDodgeFromOne) {
            float distance = transform.position.x - trigger.transform.position.x;

            targetPosition = transform.position + new Vector3(
                Mathf.Sign(distance) * (_size.x / 2f + targetSizeX / 2f - Mathf.Abs(distance) + _dodgeThreshold),
                0,
                0
            );
        }

        var targetVelocity = trigger.GetComponent<Rigidbody2D>();

        if (targetVelocity) {
            _rb.velocity = new Vector2(targetVelocity.velocity.x, 0);
        } else {
            _rb.velocity = Vector2.zero;
        }

        StartCoroutine(Utils.SmoothlyMove(transform, targetPosition, 0.1f));

        yield return new WaitUntil(() => trigger.transform.position.y < transform.position.y - _size.y / 2f);

        _isDodjing = false;
    }

    void PursuitMode(GameObject target) {
        float direction = target.transform.position.x - transform.position.x;
        
        if (Mathf.Abs(direction) > 0.1f) {
            float targetSpeed = direction * _bossObject.GetNowMode().EntitySpeed * _bossObject.GetNowMode().IncreaseFactorForSpeed * Time.deltaTime;
            _rb.velocity = new Vector2(Mathf.MoveTowards(_rb.velocity.x, targetSpeed, 0.1f), 0);
        } else {
            _rb.velocity = Vector2.zero;
        }
    }

    void FlyBetweenBorders(ref float direction) {
        if (IsReachedBound(direction)) {
            direction = -direction;
        }

        _rb.velocity = new Vector2(direction * _bossObject.GetNowMode().EntitySpeed * _bossObject.GetNowMode().IncreaseFactorForSpeed * Time.deltaTime, 0);
    }

    private bool IsReachedBound(float direction) {
        float leftBound = Utils.GetLeftBoundPlayerShipPosX();
        float rightBound = Utils.GetRightBoundPlayerShipPosX();

        if (leftBound == Mathf.Infinity || rightBound == -Mathf.Infinity) {
            return false;
        }

        if (direction > 0) {
            return transform.position.x >= rightBound + _flyBetweenThreshold;
        } else {
            return transform.position.x <= leftBound - _flyBetweenThreshold;
        }
    }
}
