using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionController : MonoBehaviour, IInteractive<Grabber>
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected int mMaxHands;
    
    /// <summary>
    /// 
    /// </summary>
    protected int mHands;
    
    /// <summary>
    /// 
    /// </summary>
    protected bool mActivated;

    /// <summary>
    /// 
    /// </summary>
    protected bool mGrabbed;
    
    /// <summary>
    /// 
    /// </summary>
    protected Grabber[] mGrabbedBy;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Mover.MovementType mMovementType;

    public void Awake()
    {
        mGrabbedBy = new Grabber[mMaxHands];
        mHands = 0;
        mActivated = false;
        mGrabbed = false;
    }

    public virtual void Grab(Grabber grabbing)
    {
        if ((1 + mHands) < mMaxHands)
        {
            mGrabbedBy[mHands] = grabbing;
            mGrabbed = true;
            mHands++;
        }
    }

    public abstract void Release(Grabber releasing);

    public abstract void Activate(Grabber activating);

    public abstract void Deactivate(Grabber deactivating);

}
