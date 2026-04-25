using UnityEngine;
using UnityEngine.UI;
public class ShockCooldownUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fillBar;
    public Image bgBar;
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    public float fadeSpeed = 5f;
    public float hideDelay = 0.5f;

    [Header("Pop Effect")]
    public RectTransform uiRoot;
    public float popScale = 1.2f;
    public float popSpeed = 8f;

    private float lastUseTime = -10f;
    private float currentProgress = 1f;

    private bool isCoolingDown = false;

    private Vector3 originalScale;

    void Awake()
    {
        if (uiRoot != null)
            originalScale = uiRoot.localScale;
    }

    void Update()
    {
        HandleFade();
        HandlePopEffect();
    }

    void HandleFade()
    {
        if (canvasGroup == null) return;

        float targetAlpha = 1f;

        if (!isCoolingDown && Time.time - lastUseTime > hideDelay)
        {
            targetAlpha = 0f;
        }

        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }

    void HandlePopEffect()
    {
        if (uiRoot == null) return;

        float targetScale = (canvasGroup != null && canvasGroup.alpha > 0.1f)
            ? originalScale.x
            : originalScale.x;

        uiRoot.localScale = Vector3.Lerp(uiRoot.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * popSpeed);
    }

    /// <summary>
    /// Called every frame from shooter
    /// </summary>
    public void SetCooldownProgress(float progress)
    {
        currentProgress = Mathf.Clamp01(progress);

        if (fillBar != null)
            fillBar.fillAmount = currentProgress;

        if (bgBar != null)
            bgBar.enabled = true;

        // Detect cooldown state
        if (currentProgress < 1f)
        {
            isCoolingDown = true;
        }
        else
        {
            // Cooldown just finished
            if (isCoolingDown)
            {
                isCoolingDown = false;
                lastUseTime = Time.time; // start fade delay HERE
            }
        }
    }

    /// <summary>
    /// Called ONLY when firing
    /// </summary>
    public void OnCooldownStarted()
    {
        lastUseTime = Time.time;
        isCoolingDown = true;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        if (uiRoot != null)
            uiRoot.localScale = originalScale * popScale;
    }
}