using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private AudioSource _initialSound;

    public void BackToPooling() {
        ObjectPooling.PushObject(gameObject);
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
}
