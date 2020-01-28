using UnityEngine;
using System;

/// <summary>
/// Base class for custom lean scripts
/// </summary>
abstract public class CustomLeanTween : MonoBehaviour
{
    #region Variables
    #region Enums
    /// <summary>
    /// Kind of fade to perform
    /// </summary>
    [Serializable]
    public enum FadeStyle { FadeIn, FadeOut }
    #endregion
    #region Editor
    /// <summary>
    /// Kind of ease to apply to the animation
    /// </summary>
    [SerializeField]
    public LeanTweenType easingStyle;
    /// <summary>
    /// length of animation in seconds
    /// </summary>
    [Tooltip("length of animation in seconds")]
    [SerializeField]
    public float duration;
    /// <summary>
    /// Delaying of start of animation in seconds
    /// </summary>
    [Tooltip("Delaying of start of animation in seconds")]
    public float delay;
    /// <summary>
    /// Whether to start animation on start
    /// </summary>
    [Tooltip("Whether to start animation on start")]
    public bool startOnEnable;
    /// <summary>
    /// Whether to start animation only on first start
    /// </summary>
    [Tooltip("Whether to start animating only on first start")]
    public bool firstTimeOnly;
    /// <summary>
    /// Looping move, for endless animation
    /// </summary>
    [Tooltip("loop the animation?")]
    public bool loop;
    #endregion
    #region Private
    /// <summary>
    /// Kind of fade to perform on this object
    /// </summary>
    private bool first;
    #endregion
    #endregion

    #region Methods
    #region Unity
    /// <summary>
    /// Set first to be true on awake
    /// </summary>
    internal void Awake()
    {
        first = true;
    }

    /// <summary>
    /// start animating if onEnable is true
    /// </summary>
    internal void OnEnable()
    {
        // check if it's the first start, if so check if first time only is true
        if (startOnEnable)
        {
            if ((firstTimeOnly && first) || !firstTimeOnly)
            {
                first = false;
                Animate();
            }
        }
    }

    /// <summary>
    /// Stop all animations on disable to prevent weird animations on next enable
    /// </summary>
    internal void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }

    /// <summary>
    /// Animates the object. 
    /// Put custom functionality in derived class here
    /// </summary>
    public abstract void Animate();
}
#endregion
#endregion