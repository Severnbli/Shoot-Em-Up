using System.Collections;
using UnityEngine;

public class AlertBarController : MonoBehaviour, Alertable
{
    private Animator _animator;

    void Awake() {
        _animator = GetComponent<Animator>();

        if (!_animator) {
            Debug.LogError($"Alert Bar Controller: {gameObject.name} has no component Animator!");
        }
    }

    public IEnumerator Alert(float duration) {
        _animator.SetBool("isAlert", true);
        
        yield return new WaitForSeconds(duration);

        _animator.SetBool("isAlert", false);
    }
}
