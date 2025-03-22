using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoSceneSwitcher : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private float _delayStartVideo;
    [SerializeField] private string _nextSceneName;
    private AsyncOperation _asyncLoad;

    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    private IEnumerator LoadNextSceneAsync()
    {
        _asyncLoad = SceneManager.LoadSceneAsync(_nextSceneName);
        _asyncLoad.allowSceneActivation = false;

        while (!_asyncLoad.isDone)
        {
            if (_asyncLoad.progress >= 0.9f)
            {
                yield return new WaitUntil(() => _videoPlayer.isPlaying == false);
                _asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator PlayVideo() {
        yield return new WaitForSeconds(_delayStartVideo);

        _videoPlayer.Play();
        
        StartCoroutine(LoadNextSceneAsync());
    }
}
