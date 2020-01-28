using UnityEngine;
using TMPro;

/// <summary>
/// Add this script to a gameobject to fade it's recttransform using leantween
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LeanFadeText : CustomLeanTween
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
    #endregion
    #region Private
    /// <summary>
    /// Image attached to this gameobject
    /// </summary>
    private TextMeshProUGUI text;
    /// <summary>
    /// The initial color of the image object
    /// </summary>
    private Color initialColor;
    #endregion
    #endregion

    #region Methods
    #region Unity
    /// <summary>
    /// get components and values
    /// </summary>
    private new void Awake()
    {
        base.Awake();
        text = GetComponent<TextMeshProUGUI>();
        initialColor = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.color = initialColor;
    }

    /// <summary>
    /// Reset the color to initialcolor
    /// </summary>
    private new void OnEnable()
    {
        text.color = initialColor;
        base.OnEnable();
    }

    /// <summary>
    /// Stop all animations on disable to prevent weird animations on next enable
    /// </summary>
    private new void OnDisable()
    {
        base.OnDisable();
        if (repeatOnDisable)
            text.color = initialColor;
    }
    #endregion

    #region Public
    /// <summary>
    /// Start fading the gameobject's text
    /// </summary>
    public override void Animate()
    {
        switch (fadeStyle)
        {
            case FadeStyle.FadeIn:
                if (loop)
                    LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) => { text.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                else
                    LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) => { text.alpha = val; }).setEase(easingStyle);
                break;
            case FadeStyle.FadeOut:
                if (loop)
                    LeanTween.value(gameObject, 1, 0, duration).setOnUpdate((float val) => { text.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                else
                {
                    LeanTween.value(gameObject, 1, 0, duration).setOnUpdate((float val) => { text.alpha = val; }).setEase(easingStyle).setOnComplete(() => gameObject.SetActive(!inactiveOnTransparent));
                }
                break;
            default:
                break;
        }
    }
    #endregion
    #endregion
}