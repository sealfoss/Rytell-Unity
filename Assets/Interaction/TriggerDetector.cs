using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public delegate void TriggerEnterDelegate(Collider other);
    public TriggerEnterDelegate mTriggerEnterEvent;

    public delegate void TriggerExitDelegate(Collider other);
    public TriggerEnterDelegate mTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        mTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        mTriggerExitEvent?.Invoke(other);
    }
}
