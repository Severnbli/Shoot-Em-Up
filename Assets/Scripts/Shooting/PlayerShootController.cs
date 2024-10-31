using UnityEngine;

public class PlayerShootController : ShootController
{
    [SerializeField] private KeyCode _key;

    void FixedUpdate()
    {
        if (Input.GetKey(_key)) {
            StartCoroutine(OneShot());
        }
    }
}
