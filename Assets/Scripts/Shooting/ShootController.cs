using System.Collections;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private GameObject _bullet; // Префаб пули

    [SerializeField] private float _speed; // Скорость пули

    [SerializeField] private Transform _bulletStartPosition; // Начальное положение пули

    [SerializeField] private float _fireDelay; // Скорость ведения огня

    private bool _isRepeatFire = false; // Безостановочный огонь

    private bool _isFireAllowed = true;

    public IEnumerator OneShot() {
        if (_isFireAllowed) {
            _isFireAllowed = false;

            GameObject bullet = Instantiate(_bullet, _bulletStartPosition.position, Quaternion.identity);

            if (bullet != null) {
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                if (rb != null) {
                    rb.velocity = _speed * _bulletStartPosition.TransformDirection(Vector3.down);
                } else {
                    Debug.LogWarning("No Rigidbody 2D component on bullet!");
                }
            } else {
                Debug.LogWarning("No bullet prefab!");
            }

            yield return new WaitForSeconds(_fireDelay);
            _isFireAllowed = true;
        }
    }

    public void StartRepeatFire() {
        if (!_isRepeatFire) {
            _isRepeatFire = true;

            StartCoroutine(RepeatFire());
        }
    }

    void OnDestroy() {
        StopRepeatFire();
    }

    public void StopRepeatFire() {
        _isRepeatFire = false;
    }

    IEnumerator RepeatFire() {
        while (_isRepeatFire) {
            yield return OneShot();
        }
    }
}
