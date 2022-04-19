using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeityController : MonoBehaviour
{
    protected Grabber[] mGrabbers;
    [SerializeField]
    protected int mHandCount;

    protected virtual void Awake()
    {
        mGrabbers = new Grabber[mHandCount];
    }
}
