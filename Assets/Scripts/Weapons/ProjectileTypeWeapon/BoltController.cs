using UnityEngine;

public class BoltController : ProjectileTypeWeaponController
{
    public override void SetPhysics()
    {
        _rb.velocity = _speed * _startTransform.TransformDirection(Vector3.up);
    }
}
