using UnityEngine;
using System.Collections;

public class LevelStartSlowMotion : MonoBehaviour
{
    [SerializeField] private float initialTimeScale = 0.2f; // Начальный низкий TimeScale
    [SerializeField] private float normalTimeScale = 1f;    // Нормальный TimeScale
    [SerializeField] private float transitionDuration = 3f; // Длительность перехода в секундах

    void Start()
    {
        Time.timeScale = initialTimeScale;
        StartCoroutine(SmoothIncreaseTimeScale());
    }

    private IEnumerator SmoothIncreaseTimeScale()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, normalTimeScale, elapsedTime / transitionDuration);
            yield return null;
        }

        Time.timeScale = normalTimeScale; // Обязательно устанавливаем окончательное значение
    }
}
