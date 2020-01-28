using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this script to a gameobject to fill the image using leantween
/// </summary>
[RequireComponent(typeof(Image))]
public class LeanFill : CustomLeanTween
{
    #region Variables
    #region Editor
    /// <summary>
    /// Fillstyle on first frame
    /// </summary>
    [Tooltip("fillstyle on first frame")]
    [SerializeField]
    FadeStyle initialFillStyle;
    #endregion
    #region Private
    /// <summary>
    /// Image component on this scripts gameobject
    /// </summary>
    private Image img;
    /// <summary>
    /// fillamount value on awake
    /// </summary>
    private float startFill;
    #endregion
    #endregion

    #region Methods
    #region Unity
    private new void Awake()
    {
        base.Awake();
        img = GetComponent<Image>();
        startFill = img.fillAmount;
    }

    /// <summary>
    /// Starts the gameobject in initial state and runs fill if onenable is true
    /// </summary>
    private new void OnEnable()
    {
        img.fillAmount = startFill;
        base.OnEnable();
    }

    /// <summary>
    /// Stop all animations on disable to prevent weird animations on next enable
    /// </summary>
    private new void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
    #endregion
    #region Public
    /// <summary>
    /// Animate the gameobject
    /// </summary>
    public override void Animate()
    {
        switch (initialFillStyle)
        {
            case FadeStyle.FadeIn:
                if (loop)
                    LeanTween.value(gameObject, img.fillAmount, 1, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay).setLoopPingPong();
                else
                    LeanTween.value(gameObject, img.fillAmount, 1, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay);
                initialFillStyle = FadeStyle.FadeOut;
                break;
            case FadeStyle.FadeOut:
                if (loop)
                    LeanTween.value(gameObject, img.fillAmount, 0, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay).setLoopPingPong();
                else
                    LeanTween.value(gameObject, img.fillAmount, 0, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay);
                initialFillStyle = FadeStyle.FadeIn;
                break;
        }
    }

    /// <summary>
    /// Fill the image to 1
    /// </summary>
    public void FillIn()
    {
        LeanTween.value(gameObject, img.fillAmount, 1, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay);
    }

    /// <summary>
    /// Fill the image to 0
    /// </summary>
    public void FillOut()
    {
        LeanTween.value(gameObject, img.fillAmount, 0, duration).setOnUpdate((float val) => { img.fillAmount = val; }).setEase(easingStyle).setDelay(delay);
    }
    #endregion
    #endregion
}