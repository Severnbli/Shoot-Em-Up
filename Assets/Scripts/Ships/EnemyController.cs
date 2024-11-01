using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _targetChangeoverTime;
    [SerializeField] private float _speed;

    private GameObject _target;

    void Start()
    {
        StartCoroutine(PlayerHunting());
    }

    void FixedUpdate() {
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

    void Hunt() {
        if (_target != null) {
            Utils.horizontalSmoothlyMove(_speed, _target.transform.position.x, transform);
        }
    }
}
