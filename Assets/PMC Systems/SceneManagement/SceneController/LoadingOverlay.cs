using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingOverlay : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    public IEnumerator FadeInBlack()
    {
		Debug.Log("Fading in...");
        yield return FadeTo(0f, 1f, 1f);
    }

    public IEnumerator FadeOutBlack()
    {
		Debug.Log("Fading out...");
        yield return FadeTo(1f, 0f, 1f);
    }

    private IEnumerator FadeTo(float startAlpha, float targetAlpha, float duration)
    {
        VisualElement overlay = document.rootVisualElement.Query("ScreenOverlay");
        if (startAlpha < targetAlpha)
        {
            overlay.style.display = DisplayStyle.Flex;
        }
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            overlay.style.opacity = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
        
        overlay.style.opacity = targetAlpha;
        
        if (startAlpha > targetAlpha)
        {
            overlay.style.display = DisplayStyle.None;
        }
    }
}
