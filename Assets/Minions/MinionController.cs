using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionController : MonoBehaviour, IInteractive<Grabber>, IBehaves
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
    protected HashSet<Grabber> mGrabbedBy;

    protected HashSet<Grabber> mSelectedBy;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected Mover.MovementType mMovementType;

    [SerializeField]
    protected Color mUnselectedColor;

    [SerializeField]
    protected Color mSelectedColor;

    [SerializeField]
    protected Color mGrabbedColor;

    protected BehaviorTree mBehavior;

    [SerializeField]
    protected float mMovementSpeed;

    protected virtual void Awake()
    {
        mGrabbedBy = new HashSet<Grabber>();
        mSelectedBy = new HashSet<Grabber>();
        mHands = 0;
        mActivated = false;
        mGrabbed = false;
        SetColor();
    }

    public void RunBehavior()
    {
        mBehavior.RunTree();
    }

    public void SetColor()
    {
        Color color;
        
        if(IsGrabbed())
            color = mGrabbedColor;
        else if(IsSelected())
            color = mSelectedColor;
        else
            color = mUnselectedColor;

        GetComponent<MeshRenderer>().material.color = color;
    }

    public virtual void Grab(Grabber grabbing)
    {
        mGrabbedBy.Add(grabbing);
        mGrabbed = true;
        mHands++;
    }

    public virtual void Release(Grabber releasing)
    {
        mGrabbedBy.Remove(releasing);
        mGrabbed = false;
        mHands--;
    }
    public abstract void Activate(Grabber activating);
    public abstract void Deactivate(Grabber deactivating);
    public abstract void Select(Grabber selecting);
    public abstract void Deselect(Grabber deselecting);
    public bool IsGrabbed()
    {
        return mGrabbed;
    }

    public HashSet<Grabber> GrabbedBy()
    {
        return mGrabbedBy;
    }

    public bool IsSelected()
    {
        return mSelectedBy.Count > 0;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
