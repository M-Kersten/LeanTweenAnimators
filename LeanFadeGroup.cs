using UnityEngine;

namespace nl.DTT.OSRVR.UI
{
    /// <summary>
    /// Add this script to a gameobject to fade it's recttransform using leantween
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class LeanFadeGroup : CustomLeanTween
    {
        #region Variables        
        #region Editor
        /// <summary>
        /// Sets the min and max value the alpha is set to
        /// </summary>
        [SerializeField]
        private Vector2 minMaxAlpha = new Vector2(0, 1);
        /// <summary>
        /// Kind of fade to perform on this object
        /// </summary>
        [Tooltip("Kind of fade to perform on this object")]
        [SerializeField]
        public FadeStyle fadeStyle;
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
        private CanvasGroup group;
        /// <summary>
        /// The initial color of the image object
        /// </summary>
        private float initialAlpha;
        #endregion
        #endregion

        #region Methods
        #region Unity
        private new void Awake()
        {
            base.Awake();
            group = GetComponent<CanvasGroup>();
            initialAlpha = group.alpha;
        }
        #endregion

        #region Public
        public override void Animate()
        {
            switch (fadeStyle)
            {
                case FadeStyle.FadeIn:
                    if (loop)
                        LeanTween.value(gameObject, minMaxAlpha.x, minMaxAlpha.y, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                    else
                        LeanTween.value(gameObject, minMaxAlpha.x, minMaxAlpha.y, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setDelay(delay);
                    break;
                case FadeStyle.FadeOut:
                    if (loop)
                        LeanTween.value(gameObject, minMaxAlpha.y, minMaxAlpha.x, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                    else
                    {
                        if (gameObject.activeInHierarchy)
                            LeanTween.value(gameObject, minMaxAlpha.y, minMaxAlpha.x, duration).setDelay(delay).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setOnComplete(() => { if (inactiveOnTransparent) { gameObject.SetActive(false); } });
                    }
                    break;
            }
        }

        /// <summary>
        /// Start fading the gameobject's recttransform
        /// </summary>
        public void FadeGroup(bool fadeIn)
        {
            FadeStyle fade = fadeIn ? FadeStyle.FadeIn : FadeStyle.FadeOut;
            switch (fade)
            {
                case FadeStyle.FadeIn:
                    if (loop)
                        LeanTween.value(gameObject, minMaxAlpha.x, minMaxAlpha.y, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                    else
                        LeanTween.value(gameObject, minMaxAlpha.x, minMaxAlpha.y, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle);
                    break;
                case FadeStyle.FadeOut:
                    if (loop)
                        LeanTween.value(gameObject, minMaxAlpha.y, minMaxAlpha.x, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setLoopPingPong();
                    else
                    {
                        if (gameObject.activeInHierarchy)
                            LeanTween.value(gameObject, minMaxAlpha.y, minMaxAlpha.x, duration).setOnUpdate((float val) => { group.alpha = val; }).setEase(easingStyle).setOnComplete(() => { if (inactiveOnTransparent) { gameObject.SetActive(false); } });
                    }
                    break;
            }
        }

        new void OnEnable()
        {
            if (fadeStyle == FadeStyle.FadeIn)
            {
                group.alpha = minMaxAlpha.x;
            }
            if (fadeStyle == FadeStyle.FadeOut)
            {
                group.alpha = minMaxAlpha.y;
            }
            base.OnEnable();
        }

        new void OnDisable()
        {
            base.OnDisable();
            if (repeatOnDisable)
                group.alpha = initialAlpha;
        }
        #endregion
        #endregion
    }
}