using UnityEngine;

public class BackgroundEntityController : MonoBehaviour
{
    [SerializeField] private float _scaleVariation;
    [SerializeField] private float _speed;

    Rigidbody2D _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        if (!_rb) {
            Debug.LogError($"{gameObject.name} BackgroundEntityController: has no component Rigidbody2D!");
        }
    }
    
    public void Setup() {
        var randParameter = Random.Range(1 - _scaleVariation, 1 + _scaleVariation);

        gameObject.transform.localScale *= randParameter;
        
        transform.rotation = Utils.GetRandomZRotation();
        
        _rb.velocity = Vector2.down * _speed;
    }

    public void SetHorizontalMovement(float movement) {
        _rb.velocity = new Vector2(movement, _rb.velocity.y);
    }

    public void SetSpeed(float speed) {
        _speed = speed;
    }
    
    public float GetSpeed() => _speed;

    public void SetScaleVariation(float variation) {
        _scaleVariation = variation;
    }
}
