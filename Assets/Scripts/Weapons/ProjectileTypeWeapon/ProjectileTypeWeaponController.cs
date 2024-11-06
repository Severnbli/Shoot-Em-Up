using UnityEngine;

public class ProjectileTypeWeaponController : WeaponController
{
    protected float _speed;

    protected override void FixedUpdate() {
        base.FixedUpdate();

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < 0 || viewportPosition.y > 1) {
            ObjectPooling.PushObject(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (_isColliderChecked) {
            _isColliderChecked = false;
            ObjectPooling.PushObject(gameObject);
        }
    }

    public void SetSpeed(float speed) {
        _speed = speed;
    }

    public override void SetActualScale()
    {
        if (Mathf.Sign(transform.localScale.y) != Mathf.Sign(_startTransform.localScale.y)) {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }
}
