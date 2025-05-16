using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    private Image _fadeImage;
    private Color _fadeColor;
    private float _fadeDuration;

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
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // make sure its on top
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(canvasObj);

        // Create Fade Image
        GameObject fadeImageObj = new GameObject("FadeScreen");
        fadeImageObj.transform.SetParent(canvasObj.transform, false);

        RectTransform rectTransform = fadeImageObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        _fadeImage = fadeImageObj.AddComponent<Image>();
        _fadeImage.color = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, 0);

        DontDestroyOnLoad(fadeImageObj);
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
