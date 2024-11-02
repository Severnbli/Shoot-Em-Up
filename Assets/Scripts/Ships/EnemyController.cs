using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _targetChangeoverTime;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isShadow = false;

    private GameObject _enemy;
    private GameObject _target;
    private Rigidbody2D _rb;
    private Animator _anim;

    void Start()
    {
        if (!_isShadow) {
            StartCoroutine(PlayerHunting());
        } else {
            _enemy = GameObject.FindGameObjectWithTag("Enemy");

            if (_enemy == null) {
                Debug.LogError("No object with tag Enemy. Shadow mistake.");
            }

        }

        _rb = GetComponent<Rigidbody2D>();

        if (!_rb) {
            Debug.LogWarning("Enemy Controller error: no Rigidbody2D component.");
        }

        _anim = GetComponent<Animator>();

        if (!_anim) {
            Debug.LogWarning("Enemy Controller error: no Animator component.");
        }
    }

    void FixedUpdate() {
        if (_isShadow) {
            _target = _enemy.GetComponent<EnemyController>()?.GetTarget();
        }

        Hunt();
    }

    private IEnumerator PlayerHunting() {
        while (true) {
            GameObject[] playerShips = GameObject.FindGameObjectsWithTag("Player");

            List<GameObject> activePlayerShips = new List<GameObject>();

            foreach (GameObject ship in playerShips) {
                if (ship.activeSelf) {
                    activePlayerShips.Add(ship);
                } 
            }
            
            if (activePlayerShips.Count > 0) {
                GameObject target;

                do {
                    target = activePlayerShips[UnityEngine.Random.Range(0, activePlayerShips.Count)];
                } while (target == _target && activePlayerShips.Count > 1);

                _target = target;
            } else {
                _target = null;
            }

            yield return new WaitForSeconds(_targetChangeoverTime);
        }
    }

    private void Hunt() {
        float speed = _speed;
        
        if (_target != null) {
            if (Mathf.Abs(transform.position.x - _target.transform.position.x) < .2f) {
                speed = 0;
            } else if (transform.position.x > _target.transform.position.x) {
                speed = -speed;
            } 

            _rb.velocity = new Vector2(speed, 0);

            _anim.SetFloat("velocityX", _rb.velocity.x);
        }
    }

    public GameObject GetTarget() {
        return _target;
    }
}
