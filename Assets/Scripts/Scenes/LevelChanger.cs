using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private bool _isThisLoadingScreen = false;
    [SerializeField] private float _loadingTime = 5f;
    [SerializeField] private bool _isFadeOnTheStartOk = true;
    private AsyncOperation _asyncLoad;
    private Animator _animator;

    void Start() {
        _animator = GetComponent<Animator>();

        if (!_isFadeOnTheStartOk) {
            _animator.SetBool("noStartFade", true);
        }

        if (_isThisLoadingScreen) {
            StartCoroutine(LoadLoadingScreen());
        }
    }

    public void ChangeLevel() {
        _animator.SetBool("isFade", true);
    }

    public void LoadNextLevel() {
        if (_isThisLoadingScreen) {
            _asyncLoad.allowSceneActivation = true;
        } else {
            SceneManager.LoadScene("LoadScreen");
        }
    }

    private IEnumerator LoadLoadingScreen() {
        float startTime = Time.time;

        _asyncLoad = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("nextScene"));

        _asyncLoad.allowSceneActivation = false;

        while (!_asyncLoad.isDone)
        {
            if (_asyncLoad.progress >= 0.9f)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < _loadingTime) {
                    yield return new WaitForSeconds(_loadingTime - elapsedTime);
                }
                
                _animator.SetBool("isFade", true);
            }
            yield return null;
        }
    }
}
