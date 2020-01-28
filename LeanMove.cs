using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Add this script to a gameobject to move it around using leantween
/// </summary>
public class LeanMove : MonoBehaviour
{
    #region Variables
    #region Structs
    /// <summary>
    /// A movement of a UI object, can give perform an action on arrival
    /// </summary>
    [Serializable]
    private struct Movement
    {
        public Vector3 endPosition;
        public float duration;
        public float delay;
        public bool actionOnArrival;
        public UnityEvent action;
    }
    #endregion
    #region Editor
    /// <summary>
    /// Kind of ease to apply to the rotate
    /// </summary>
    [Tooltip("Kind of ease to apply to the rotate")]
    [SerializeField]
    private LeanTweenType easingStyle;
    /// <summary>
    /// Whether to start rotating on start
    /// </summary>
    [Tooltip("Whether to start rotating on start")]
    [SerializeField]
    private bool onEnable;
    /// <summary>
    /// Looping move, for endless animation
    /// </summary>
    [Tooltip("loop the animation?")]
    [SerializeField]
    private bool loop;
    /// <summary>
    /// Whether the location should be relative to the starting location
    /// </summary>
    [Tooltip("Is the location relative to the starting postiion?")]
    [SerializeField]
    private bool localRelative;
    /// <summary>
    /// start the movements from this position
    /// </summary>
    [Tooltip("start the movements from this position")]
    [SerializeField]
    private Vector3 startingPosition;
    public UnityEvent actionOnTargetFound;
    /// <summary>
    /// Position to move to
    /// </summary>
    [Tooltip("Position to move to")]
    [SerializeField]
    private Movement[] movements;
    #endregion
    #endregion

    #region Methods
    #region Unity
    private void Awake()
    {
        if (localRelative)
        {
            for (int i = 0; i < movements.Length; i++)
            {
                movements[i].endPosition += startingPosition;
            }
        }
    }
    #endregion
    #region Public
    /// <summary>
    /// Move this gameobject's transform
    /// </summary>
    public void MoveObject(int position = -1, Action action = null)
    {
        StartCoroutine(MoveToPositions(position, action ?? null));
    }

    void OnEnable()
    {
        actionOnTargetFound?.Invoke();
        transform.localPosition = startingPosition;
        if (onEnable)
            MoveObject();
    }

    void OnDisable()
    {
        StopCoroutine(MoveToPositions());
        transform.localPosition = startingPosition;
        LeanTween.cancel(gameObject);
    }
    #endregion

    #region Private
    /// <summary>
    /// Set starting position in editor
    /// </summary>
    [ContextMenu("Set starting position")]
    private void SetStartingPosition()
    {
        startingPosition = transform.localPosition;
    }

    #region Coroutines
    /// <summary>
    /// Move to specified positions one at a time
    /// </summary>
    private IEnumerator MoveToPositions(int positions = -1, Action customAction = null)
    {
        // If movements is empty, early out of coroutine
        if (positions >= movements.Length)
            yield return null;

        // if moving to only one specific destination
        if (positions >= 0)
            StartCoroutine(MoveToPosition(positions));
        else
        {
            // else cycle through all destinations
            for (int i = 0; i < movements.Length; i++)
            {
                StartCoroutine(MoveToPosition(i));
                yield return new WaitForSeconds(movements[i].delay + movements[i].duration);
            }
            if (loop)
            {
                StartCoroutine(MoveToPositions());
            }
        }
    } 

    /// <summary>
    /// Moves to a specific position in the movements index
    /// </summary>
    /// <param name="movementsIndex"></param>
    /// <returns></returns>
    private IEnumerator MoveToPosition(int movementsIndex)
    {
        LeanTween.moveLocal(
            gameObject,
            movements[movementsIndex].endPosition,
            movements[movementsIndex].duration).setEase(easingStyle).setDelay(movements[movementsIndex].delay);

        yield return new WaitForSeconds(movements[movementsIndex].delay);
        if (!movements[movementsIndex].actionOnArrival)
            movements[movementsIndex].action?.Invoke();

        yield return new WaitForSeconds(movements[movementsIndex].duration);
        if (movements[movementsIndex].actionOnArrival)
            movements[movementsIndex].action?.Invoke();
    }
    #endregion
    #endregion
    #endregion
}