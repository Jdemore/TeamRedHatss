using System.Collections;
using UnityEngine;

public class LoadingScreenFade : MonoBehaviour
{
    public static LoadingScreenFade Instance { get; private set; }

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    public float FadeDuration => _fadeDuration;

    private void Awake()
    {
        Instance = this;

        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;

        while (t < _fadeDuration)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha =
                Mathf.Lerp(from, to, t / _fadeDuration);

            yield return null;
        }

        _canvasGroup.alpha = to;
    }
}