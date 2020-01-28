using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this script to a gameobject to fade it's recttransform using leantween
/// </summary>
[RequireComponent(typeof(Image))]
public class LeanFader : CustomLeanTween
{
    #region Variables
    #region Editor
    /// <summary>
    /// Kind of fade to perform on this object
    /// </summary>
    [Tooltip("Kind of fade to perform on this object")]
    [SerializeField]
    private FadeStyle fadeStyle;
    /// <summary>
    /// Whether to start fading on start again after disabling
    /// </summary>
    [Tooltip("Whether to start fading on start again after disabling")]
    [SerializeField]
    private bool repeatOnDisable;
    /// <summary>
    /// Whether to disable the gameobject on fadeout
    /// </summary>
    [Tooltip("Whether to disable the gameobject on fadeout")]
    [SerializeField]
    private bool inactiveOnTransparent;
    [SerializeField]
    private bool randomizeDelay;
    [SerializeField]
    private Vector2 minMaxRandom;
    #endregion
    #region Private
    /// <summary>
    /// Image attached to this gameobject
    /// </summary>
    private Image img;
    /// <summary>
    /// The initial color of the image object
    /// </summary>
    private Color initialColor;
    private FadeStyle initialFadeStyle;
    #endregion
    #endregion

    #region Methods
    #region Unity
    new void Awake()
    {
        base.Awake();
        initialFadeStyle = fadeStyle;
    }

    private new void OnEnable()
    {
        img = GetComponent<Image>();
        initialColor = new Color(img.color.r, img.color.g, img.color.b, fadeStyle == FadeStyle.FadeIn ? 0 : 1);
        if (startOnEnable)
            img.color = initialColor;   

        base.OnEnable();
    }

    /// <summary>
    /// Stop all animations on disable to prevent weird animations on next enable
    /// </summary>
    private new void OnDisable()
    {
        base.OnDisable();
        if (repeatOnDisable)
        {
            img.color = initialColor;
            fadeStyle = initialFadeStyle;
        }
    }
    #endregion

    #region Public
    /// <summary>
    /// Start fading the gameobject's recttransform
    /// </summary>
    public override void Animate()
    {
        switch (fadeStyle)
        {
            case FadeStyle.FadeIn:
                if (loop)
                {
                    if (!randomizeDelay)
                        LeanTween.alpha((RectTransform)gameObject.transform, 1, duration).setEase(easingStyle).setOnComplete(() => Animate());
                    else
                        LeanTween.alpha((RectTransform)gameObject.transform, 1, duration).setEase(easingStyle).setOnComplete(() => StartCoroutine(AnimateAfterDelay()));
                }
                else
                    LeanTween.alpha((RectTransform)gameObject.transform, 1, duration).setEase(easingStyle).setDelay(delay);
                fadeStyle = FadeStyle.FadeOut;
                break;
            case FadeStyle.FadeOut:
                if (loop)
                {
                    if (!randomizeDelay)
                        LeanTween.alpha((RectTransform)gameObject.transform, 0, duration).setEase(easingStyle).setOnComplete(() => Animate());
                    else
                        LeanTween.alpha((RectTransform)gameObject.transform, 0, duration).setEase(easingStyle).setOnComplete(() => StartCoroutine(AnimateAfterDelay()));
                }                    
                else
                    LeanTween.alpha((RectTransform)gameObject.transform, 0, duration).setEase(easingStyle).setOnComplete(() => gameObject.SetActive(!inactiveOnTransparent)).setDelay(delay);                
                fadeStyle = FadeStyle.FadeIn;
                break;
            default:
                break;
        }
    }

    public void ResetFader()
    {
        LeanTween.cancel(gameObject);
        img.color = initialColor;
        fadeStyle = FadeStyle.FadeOut;
    }

    public void FadeObjectOut()
    {
        LeanTween.alpha((RectTransform)gameObject.transform, 0, duration).setEase(easingStyle).setOnComplete(() => gameObject.SetActive(!inactiveOnTransparent));
    }

    private IEnumerator AnimateAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(minMaxRandom.x, minMaxRandom.y));
        Animate();
    }
    #endregion
    #endregion
}