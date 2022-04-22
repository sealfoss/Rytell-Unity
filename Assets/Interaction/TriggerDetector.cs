using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects trigger overlap in one object and provides associated events to be fired in others.
/// </summary>
public class TriggerDetector : MonoBehaviour
{
    /// <summary>
    /// Event fired on the occasion this object should begin overlapping another.
    /// </summary>
    /// <param name="other">Collider of other object.</param>
    public delegate void TriggerEnterDelegate(Collider other);
    public TriggerEnterDelegate mTriggerEnterEvent;

    /// <summary>
    /// Event fired on occassion this object should end overlapping another.
    /// </summary>
    /// <param name="other">Collider of other object.</param>
    public delegate void TriggerExitDelegate(Collider other);
    public TriggerEnterDelegate mTriggerExitEvent;

    /// <summary>
    /// Called when this object begins overlapping another.
    /// </summary>
    /// <param name="other">Other object overlapping this one.</param>
    private void OnTriggerEnter(Collider other)
    {
        mTriggerEnterEvent?.Invoke(other);
    }

    /// <summary>
    /// Called when this object ends overlapping another.
    /// </summary>
    /// <param name="other">Other object no longer overlapping this one.</param>
    private void OnTriggerExit(Collider other)
    {
        mTriggerExitEvent?.Invoke(other);
    }
}
