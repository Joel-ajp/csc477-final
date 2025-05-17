using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    private Image _fadeImage;
    private Color _fadeColor;
    private float _fadeDuration;
    private GameObject _canvasObj;
    private GameObject _fadeImageObj;


    public static void StartFade(string sceneName) // static to be globally accessable
    {
        // Create fade object
        GameObject fadeObj = new GameObject("FadeTransition");
        FadeTransition fadeTransition = fadeObj.AddComponent<FadeTransition>();
        fadeTransition._fadeColor = Color.black;
        fadeTransition._fadeDuration = 1f;

        DontDestroyOnLoad(fadeObj);

        fadeTransition.InitializeCanvas();
        fadeTransition.StartCoroutine(fadeTransition.FadeAndLoadScene(sceneName));
    }

    private void InitializeCanvas()
    {
        // Create Canvas
        _canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = _canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        _canvasObj.AddComponent<CanvasScaler>();
        _canvasObj.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(_canvasObj);

        // Create Fade Image
        _fadeImageObj = new GameObject("FadeScreen");
        _fadeImageObj.transform.SetParent(_canvasObj.transform, false);

        RectTransform rectTransform = _fadeImageObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        _fadeImage = _fadeImageObj.AddComponent<Image>();
        _fadeImage.color = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, 0);

        DontDestroyOnLoad(_fadeImageObj);
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade out
        yield return Fade(0f, 1f);

        // Load scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade in
        yield return Fade(1f, 0f);

        // Clean up fade objects
        Destroy(_fadeImageObj);
        Destroy(_canvasObj);
        Destroy(gameObject);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = _fadeImage.color;

        while (timer <= _fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            _fadeImage.color = color;
            yield return null;
        }
    }
}
