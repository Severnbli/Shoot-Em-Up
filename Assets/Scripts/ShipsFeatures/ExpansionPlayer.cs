using UnityEngine;

public class ExpansionPlayer : MonoBehaviour {
    // private Transform _tr;
    // private Vector3 _movement;

    // [SerializeField] private bool _isBoatHigherThanCenter = true;
    // [SerializeField] private float _maximumDevation = 2f;
    // [SerializeField] private float _devationSpeed = 0.1f;
    // private float _nowDevation = 0f;
    
    // void Awake() {
    //     _tr = gameObject.GetComponent<Transform>();

    //     _movement = new Vector3(0, _devationSpeed, 0);
    // }

    // void Update() {
    //     Vector2 scrollDelta = Input.mouseScrollDelta;

    //     if (scrollDelta.y > 0) {
    //         if (_nowDevation < _maximumDevation) {
    //             _nowDevation += _devationSpeed;

    //             if (_isBoatHigherThanCenter) {
    //                 _tr.position += _movement;
    //             } else {
    //                 _tr.position -= _movement;
    //             }
    //         }
    //     } else if (scrollDelta.y < 0) {
    //         if (_nowDevation > 0) {
    //             _nowDevation -= _devationSpeed;

    //             if (_isBoatHigherThanCenter) {
    //                 _tr.position -= _movement;
    //             } else {
    //                 _tr.position += _movement;
    //             }
    //         }
    //     }
    // }
}
