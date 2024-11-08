using UnityEngine;

public class AIEntityController : MonoBehaviour
{
    [SerializeField] private WeaponDetectorController _detector;
    private AIController _bossObject;

    protected void Awake() {
        if (!_detector) {
            Debug.LogWarning($"AI Entity Controller: {gameObject.name}. No Weapon Detector Controller!");
        }
    } 

    public void Connect(AIController boss) {
        _bossObject = boss;
    }

    // [SerializeField] private float _speed;
    // [SerializeField] private float _increaseFactorForSpeed = 10f;
    // [SerializeField] private float _modeWholeLineIncreaseFactorForSpeed = 2f;
    // [SerializeField] private float _modePursuitLiveTime = 5f;
    // [SerializeField] private float _modeDoublePenetrationLiveTime = 2f;
    // [SerializeField] private float _modeWholeLineDeviation = 1f;

    // [SerializeField] private float _pursuitProbability = 0.5f;
    // [SerializeField] private float _wholeLineProbability = 0.3f;
    // [SerializeField] private float _doublePenetrationProbability = 0.2f;

    // private Rigidbody2D _rb;
    // private bool _isRandomMode = false;
    

    // private float _checkTime;
    // private bool _isModeActive;
    // private float _wholeLineDirection;
    // private List<GameObject> _doublePenetrationTargets = new List<GameObject>();

    // private void Awake()
    // {
    //     _rb = GetComponent<Rigidbody2D>();

    //     if (_rb == null) {
    //         Debug.LogWarning($"{gameObject.name} has no Rigidbody2D component!");
    //     }
    // }

    // private void FixedUpdate() {
    //     if (_isRandomMode && !_isModeActive) {
    //         _operationMode = GetRandomOperatingMode();
    //     }

    //     HandleOperatingMode();
    // }

    // void HandleOperatingMode() {
    //     switch (_operationMode) {
    //         case OperatingModes.PURSUIT: {
    //             Pursuit();
    //             break;
    //         }
    //         case OperatingModes.WHOLE_LINE: {
    //             Whole_Line();
    //             break;
    //         }
    //         case OperatingModes.DOUBLE_PENETRATION: {
    //             Double_Penetration();
    //             break;
    //         }
    //         case OperatingModes.RANDOM:
    //             _isRandomMode = true;
    //             break;
    //     }
    // }

    // void Pursuit() {
    //     if (!_isModeActive) {
    //         _isModeActive = true;
    //         _checkTime = Time.time;
    //     }

    //     GameObject closestPlayer = GetClosestPlayer();

    //     if (closestPlayer != null) {
    //         float direction = closestPlayer.transform.position.x - transform.position.x;

    //         if (direction != 0) {
    //             direction = Mathf.Sign(direction);
    //             _rb.velocity = new Vector2(direction * _speed * _increaseFactorForSpeed * Time.deltaTime, 0);
    //         } else {
    //             _rb.velocity = Vector2.zero;
    //         }
    //     }

    //     if (Time.time - _checkTime > _modePursuitLiveTime) {
    //         _isModeActive = false;
    //     }
    // }

    // void Whole_Line() {
    //     if (!_isModeActive) {
    //         _isModeActive = true;

    //         _wholeLineDirection = UnityEngine.Random.Range(0, 2) * 2 - 1;

    //         if (_wholeLineDirection > 0) {
    //             float leftBound = Utils.GetLeftBoundPlayerShipPosX();
    //             if (leftBound == Mathf.Infinity) {
    //                 _isModeActive = false;
    //                 return;
    //             }

    //             transform.position = new Vector3(leftBound - _modeWholeLineDeviation, transform.position.y);
    //         } else {
    //             float rightBound = Utils.GetRightBoundPlayerShipPosX();
    //             if (rightBound == -Mathf.Infinity) {
    //                 _isModeActive = false;
    //                 return;
    //             }

    //             transform.position = new Vector3(rightBound + _modeWholeLineDeviation, transform.position.y);
    //         }

    //         _rb.velocity = new Vector2(_wholeLineDirection * _speed * _increaseFactorForSpeed * _modeWholeLineIncreaseFactorForSpeed * Time.deltaTime, 0);
    //     }

    //     if (IsReachedBound(_wholeLineDirection)) {
    //         _isModeActive = false;
    //     }
    // }

    // void Double_Penetration() {
    //     if (!_isModeActive) {
    //         _doublePenetrationTargets.Clear();
    //         _checkTime = Time.time;
    //         _isModeActive = true;
    //         _rb.velocity = Vector2.zero;

    //         var players = GameObject.FindGameObjectsWithTag("Player").OrderBy(p => p.transform.position.x).ToArray();

    //         if (players.Length < 1) {
    //             _isModeActive = false;
    //             return;
    //         } else {
    //             GameObject target1 = players[UnityEngine.Random.Range(0, players.Length)];
                
    //             _doublePenetrationTargets.Add(target1);

    //             if (players.Length >= 2) {
    //                 GameObject target2;

    //                 do {
    //                     target2 = players[UnityEngine.Random.Range(0, players.Length)];
    //                 } while (target2 == target1);
                    
    //                 _doublePenetrationTargets.Add(target2);
    //             }
    //         }
    //     }
        
    //     if (Time.time - _checkTime < _modeDoublePenetrationLiveTime) {
    //         MoveTowardsTargets(_doublePenetrationTargets);
    //     } else {
    //         _isModeActive = false;
    //     }
    // }

    // private GameObject GetClosestPlayer() {
    //     var players = GameObject.FindGameObjectsWithTag("Player");

    //     GameObject closestPlayer = null;
    //     float minDistance = Mathf.Infinity;

    //     foreach (var player in players) {
    //         float distance = Mathf.Abs(transform.position.x - player.transform.position.x);
    //         if (distance < minDistance) {
    //             minDistance = distance;
    //             closestPlayer = player;
    //         }
    //     }

    //     return closestPlayer;
    // }

    // void MoveTowardsTargets(List<GameObject> targets) {
    //     if (Time.time - _checkTime < _modeDoublePenetrationLiveTime / 2f) {
    //         if (targets[0]) {
    //             MoveToTarget(targets[0]);
    //         }
    //     } else {
    //         if (targets.Count < 2) {
    //             _isModeActive = false;
    //             return;
    //         }

    //         if (targets[1]) {
    //             MoveToTarget(targets[1]);
    //         } 
    //     }
    // }

    // void MoveToTarget(GameObject target) {
    //     Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
    //     transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    // }

    // private bool IsReachedBound(float direction) {
    //     float leftBound = Utils.GetLeftBoundPlayerShipPosX();
    //     float rightBound = Utils.GetRightBoundPlayerShipPosX();

    //     if (direction > 0) {
    //         return transform.position.x >= rightBound;
    //     } else {
    //         return transform.position.x <= leftBound;
    //     }
    // }

    // public OperatingModes GetRandomOperatingMode() {
    //     List<OperatingModes> randList = new List<OperatingModes>();

    //     for (int i = 0; i < _pursuitProbability * 100; i++) {
    //         randList.Add(OperatingModes.PURSUIT);
    //     }

    //     for (int i = 0; i < _wholeLineProbability * 100; i++) {
    //         randList.Add(OperatingModes.WHOLE_LINE);
    //     }

    //     for (int i = 0; i < _doublePenetrationProbability * 100; i++) {
    //         randList.Add(OperatingModes.DOUBLE_PENETRATION);
    //     }

    //     return randList[UnityEngine.Random.Range(0, randList.Count)];
    // }
}
