using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceshipsMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0;
    private Vector3 _movement;

    void Start() {
        _movement = new Vector3(_speed, 0, 0);
    }

    void Update() {
        float mouseXMovement = Input.GetAxis("Mouse X");
        
        if (mouseXMovement > 0) {
            gameObject.transform.position += _movement; 
        } else if (mouseXMovement < 0) {
            gameObject.transform.position -= _movement;
        }
    }
 }
