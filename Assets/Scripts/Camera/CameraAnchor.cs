using System;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    [SerializeField] private float _maxCameraSize = 6f;
    [SerializeField] private float _minCameraSize = 5f;
    [SerializeField] private float _stepToIncrease = 0.1f;
    [SerializeField] private float _paddingToChangeSize = 0.05f;
    [SerializeField] private float _paddinfToChangePivot = 1f;
    private GameObject _enemy;
    private GameObject[] _playerShips;

    void Start() {
        _enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (_enemy == null) {
            Debug.LogError("Camera Anchor: Enemy not found!");
        }

        _playerShips = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate()
    {
        MoveCamera();
        AdjustCameraSize();
    }

    Vector3 CalculateCentroid()
    {
        float sumPositionX = 0f;
        int count = 0;

        if (_enemy != null && _enemy.activeSelf)
        {
            sumPositionX += _enemy.transform.position.x;
            count++;
        }

        float leftBound = Utils.GetLeftBoundPlayerShipPosX();
        float rightBound = Utils.GetRightBoundPlayerShipPosX();

        if (_enemy && _enemy.transform.position.x < leftBound - _paddinfToChangePivot && leftBound != Mathf.Infinity) {
            sumPositionX += rightBound;
            count++;
        } else if (_enemy && _enemy.transform.position.x > rightBound + _paddinfToChangePivot && rightBound != -Mathf.Infinity) {
            sumPositionX += leftBound;
            count++;
        } else {
            if (leftBound != Mathf.Infinity) {
                sumPositionX += leftBound;
                count++;
            }

            if (rightBound != -Mathf.Infinity) {
                sumPositionX += rightBound;
                count++;
            }
        }

        return count > 0 ? new Vector3(sumPositionX / count, transform.position.y, transform.position.z) : transform.position;
    }

    float CalculateRequiredSize()
    {
        float requiredSize = Camera.main.orthographicSize;
        bool isNeedSizeUp = false;

        if (_enemy != null && _enemy.activeSelf)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(_enemy.transform.position);
            
            if (viewportPosition.x < _paddingToChangeSize || viewportPosition.x > 1 - _paddingToChangeSize) {
                isNeedSizeUp = true;
            } 
        }

        foreach (GameObject ship in _playerShips)
        {
            if (ship != null && ship.activeSelf)
            {
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(ship.transform.position);
                
                if (viewportPosition.x < _paddingToChangeSize || viewportPosition.x > 1 - _paddingToChangeSize) {
                    isNeedSizeUp = true;
                }
            }
        }

        if (isNeedSizeUp) {
            requiredSize += _stepToIncrease;
        } else {
            requiredSize -= _stepToIncrease;
        }

        return requiredSize;
    }

    void MoveCamera()
    {
        Vector3 newPosition = CalculateCentroid();

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 2f);
    }

    void AdjustCameraSize()
    {
        float requiredSize = CalculateRequiredSize();
        float targetSize = Mathf.Clamp(requiredSize, _minCameraSize, _maxCameraSize);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime * 2f);
    }
}
