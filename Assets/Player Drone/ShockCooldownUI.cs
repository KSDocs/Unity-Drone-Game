using UnityEngine;
using UnityEngine.UI;
public class ShockCooldownUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillBar;
    public Image bgBar;
    public CanvasGroup canvasGroup; // for fade
    public float fadeSpeed = 5f;    // higher = faster fade
    public float hideDelay = 0.5f;  // seconds to wait after full before fading

    private float lastUseTime = -10f;
    private float currentProgress = 1f;

    void Update()
    {
        // Lerp alpha for smooth fade
        if (canvasGroup != null)
        {
            float targetAlpha = 1f;

            // fade out only when cooldown full and delay passed
            if (currentProgress >= 1f && Time.time - lastUseTime > hideDelay)
                targetAlpha = 0f;

            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        }
    }

    /// <summary>
    /// 0 = empty, 1 = full
    /// </summary>
    public void SetCooldownProgress(float progress)
    {
        currentProgress = Mathf.Clamp01(progress);

        if (fillBar != null)
            fillBar.fillAmount = currentProgress;

        if (bgBar != null)
            bgBar.enabled = true;

        lastUseTime = Time.time;

        // Ensure CanvasGroup is visible immediately
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }
}