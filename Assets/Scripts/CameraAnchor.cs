using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    private GameObject _enemy;
    private GameObject[] _playerShips;
    public float _padding = 2f; // Дополнительное пространство вокруг объектов

    void Start() {
        _enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (_enemy == null) {
            Debug.LogError("Camera Anchor: Enemy not found!");
        }

        _playerShips = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector3 centroid = CalculateCentroid();
        float requiredSize = CalculateRequiredSize(centroid);

        MoveCamera(centroid);
        // AdjustCameraSize(requiredSize);
    }

    Vector3 CalculateCentroid()
    {
        Vector3 sumPosition = Vector3.zero;
        int count = 0;

        if (_enemy != null && _enemy.activeSelf)
        {
            sumPosition += _enemy.transform.position;
            count++;
        }

        foreach (GameObject ship in _playerShips)
        {
            if (ship != null && ship.activeSelf)
            {
                sumPosition += ship.transform.position;
                count++;
            }
        }

        return count > 0 ? sumPosition / count : transform.position;
    }

    float CalculateRequiredSize(Vector3 centroid)
    {
        float maxDistance = 0f;

        if (_enemy != null && _enemy.activeSelf)
        {
            maxDistance = Vector3.Distance(centroid, _enemy.transform.position);
        }

        foreach (GameObject ship in _playerShips)
        {
            if (ship != null && ship.activeSelf)
            {
                float distance = Vector3.Distance(centroid, ship.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance + _padding;
    }

    void MoveCamera(Vector3 centroid)
    {
        Vector3 newPosition = new Vector3(centroid.x, 0, transform.position.z);
        transform.position = newPosition;
    }

    void AdjustCameraSize(float requiredSize)
    {
        float maxCameraSize = 7f; // Максимально допустимый размер камеры
        Camera.main.orthographicSize = Mathf.Min(Mathf.Max(Camera.main.orthographicSize, requiredSize), maxCameraSize);
    }
}
