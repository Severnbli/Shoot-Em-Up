using System.Collections.Generic;
using UnityEngine;

/**
* Для правильно работы необходимо, чтобы на снаряде и на таргете были компоненты Rigidbody2D (режим Kinematic и Collision Detection 
* в режиме Continuos) и коллайдер в режиме Trigger.
*/

public class WeaponController : MonoBehaviour
{
    private float _damageAmount = 0f;
    [SerializeField] private AudioSource _initialSound;
    
    protected Rigidbody2D _rb;

    private List<string> _targetsTags = new List<string>();

    protected bool _isColliderChecked = false;

    protected Transform _startTransform;

    protected virtual void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        if (_rb == null) {
            Debug.LogError($"{gameObject.name} has no component Rigidbody2D!");
        }
    }

    protected virtual void Start() {}

    protected virtual void FixedUpdate() {}

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        if (_targetsTags.Contains(collider.gameObject.tag)) {
            ObjectPooling.PopObject("Explosion", collider.transform.position)?.GetComponent<ExplosionController>()?.PlayMusic();

            HealthController healthController = collider.gameObject.GetComponent<HealthController>();

            if (healthController) {
                healthController.AddDamage(_damageAmount);
            }

            _isColliderChecked = true;
        }
    }

    public virtual void SetPhysics() {}

    public virtual void SetActualScale() {}

    public void AddTargetTag(string tag) {
        _targetsTags.Add(tag);
    }

    public void SetDamageAmount(float damage) {
        _damageAmount = damage;
    }

    public void SetStartTransform(Transform startTransform) {
        _startTransform = startTransform;
    }

    public void SetLocalScaleFactor(float localScaleFactor) {
        transform.localScale = transform.localScale * localScaleFactor;
    }

    public void PlayMusic() {
        if (_initialSound) {
            AudioSource source = GetComponent<AudioSource>();

            if (source == null) {
                gameObject.AddComponent<AudioSource>();

                source = GetComponent<AudioSource>();

                Utils.CopyAudioSourceProperties(_initialSound, source);
            }

            source.Play();
        }
    }

    public List<string> GetTargetsTags() {
        return _targetsTags;
    }
}
