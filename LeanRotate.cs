using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Add this script to a gameobject to rotate it using leantween
/// </summary>
public class LeanRotate : CustomLeanTween
{
    #region Variables
    #region Editor        
    /// <summary>
    /// Rotation gameobject's transform gets on first frame
    /// </summary>
    [Tooltip("Sets gameobjects rotation on first frame")]
    [SerializeField]
    private float startRotation;
    /// <summary>
    /// Rotation gameobject's transform gets on first frame
    /// </summary>
    [Tooltip("Sets gameobjects rotation on first frame")]
    [SerializeField]
    private float endRotation;
    /// <summary>
    /// Action to perform when rotation is finished
    /// </summary>
    [SerializeField]
    private UnityEvent OnFinished;
    #endregion
    #region Public
    /// <summary>
    /// Rotation to set on enable
    /// </summary>
    public float StartRotation { get { return startRotation; } }
    #endregion
    #region Private
    private bool atStartRotation;
    #endregion
    #endregion

    #region Methods
    #region Unity
    new void OnEnable()
    {
        atStartRotation = true;
        transform.localEulerAngles = (Vector3.forward * startRotation);
        base.OnEnable();        
    }

    #endregion
    #region Public
    public override void Animate()
    {       
        if (loop)
            LeanTween.rotateLocal(gameObject, new Vector3(0, 0, endRotation), duration).setEase(easingStyle).setDelay(delay).setLoopPingPong();
        else
            LeanTween.rotateLocal(gameObject, new Vector3(0, 0, atStartRotation ? endRotation : startRotation), duration).setEase(easingStyle).setDelay(delay).setOnComplete(() => OnFinished?.Invoke());
        atStartRotation = !atStartRotation;
    }

    /// <summary>
    /// Rotate this gameobject's transform
    /// </summary>
    public void RotateObject(float from, float to, float seconds = 0)
    {
        if (loop)
            LeanTween.rotateLocal(gameObject, new Vector3(0, 0, endRotation), seconds == 0 ? duration : seconds).setEase(easingStyle).setDelay(delay).setLoopPingPong();
        else
            LeanTween.rotateLocal(gameObject, new Vector3(0, 0, endRotation), seconds == 0 ? duration : seconds).setEase(easingStyle).setDelay(delay).setOnComplete(() => OnFinished?.Invoke());
    }

    public void ResetRotate()
    {
        LeanTween.cancel(gameObject);
    }

    public void Rotate(bool rotateIn)
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0, 0, rotateIn ? endRotation : startRotation), duration).setEase(easingStyle).setDelay(delay).setOnComplete(() => OnFinished?.Invoke());
    }
    #endregion
    #endregion
}