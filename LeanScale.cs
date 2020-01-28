using System;
using UnityEngine;

/// <summary>
/// Add this script to a gameobject to rotate it using leantween
/// </summary>
public class LeanScale : CustomLeanTween
{
    #region Variables
    #region Editor
    /// <summary>
    /// Whether to start fading on start again after disabling
    /// </summary>
    [Tooltip("Whether to start fading on start again after disabling")]
    [SerializeField]
    private bool repeatOnDisable;
    /// <summary>
    /// Which scale the object should animate to
    /// </summary>
    [Tooltip("Scale the object should animate to")]
    [SerializeField]
    private Vector3 endScale;
    #endregion
    #region Private
    /// <summary>
    /// The initial scale of the object before animating
    /// </summary>
    private Vector3 initialScale;
    private bool atEndScale;
    #endregion
    #endregion

    #region Methods
    #region Unity
    new void Awake()
    {
        atEndScale = false;
        initialScale = gameObject.transform.localScale;
        base.Awake();
    }

    new void OnDisable()
    {        
        base.OnDisable();
        if (repeatOnDisable)
        {
            gameObject.transform.localScale = initialScale;
            atEndScale = false;
        }
    }
    #endregion
    #region Public
    /// <summary>
    /// Animate the object
    /// </summary>
    public override void Animate()
    {
        if (loop)
            LeanTween.scale(gameObject, atEndScale ? initialScale : endScale, duration).setEase(easingStyle).setDelay(delay).setOnComplete(() => Animate());
        else
            LeanTween.scale(gameObject, atEndScale ? initialScale : endScale, duration).setEase(easingStyle).setDelay(delay);

        atEndScale = !atEndScale;
    }

    /// <summary>
    /// Custom rotation with more options
    /// </summary>
    public void CustomScaleObject(Vector3 toScale, float customDelay = 0, Action onComplete = null)
    {
        if (loop)
            LeanTween.scale(gameObject, toScale, duration).setEase(easingStyle).setDelay(customDelay > 0 ? customDelay : delay).setLoopPingPong();
        else
            LeanTween.scale(gameObject, toScale, duration).setEase(easingStyle).setDelay(customDelay > 0 ? customDelay : delay).setOnComplete(onComplete);
    }
    #endregion
    #endregion
}