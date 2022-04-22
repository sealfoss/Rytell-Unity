using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grabs game objects that have attached components implementing IInteractive interface.
/// </summary>
public abstract class Grabber : MonoBehaviour
{
    /// <summary>
    /// Player class.
    /// </summary>
    protected DeityController mDeity;

    /// <summary>
    /// Object that has been grabbed, if any.
    /// </summary>
    protected IInteractive<Grabber> mGrabbed;

    /// <summary>
    /// Object selected to be grabbed, if any.
    /// </summary>
    protected IInteractive<Grabber> mSelected;

    /// <summary>
    /// Object activated by this grabber, if any.
    /// </summary>
    protected IInteractive<Grabber> mActivated;

    /// <summary>
    /// The type of movement supported by this grabber.
    /// </summary>
    protected Mover mMover;

    /// <summary>
    /// Gameobject representing the grabbing "hand" in the game world.
    /// </summary>
    protected GameObject mHand;

    /// <summary>
    /// Objects currently available to be grabbed.
    /// </summary>
    protected List<IInteractive<Grabber>> mAvailable;

    /// <summary>
    /// Detects objects overlapping hand.
    /// </summary>
    protected TriggerDetector mDetector;

    private void Update()
    {
        PickSelected();
    }

    private void TriggerEnter(Collider collision)
    {
        IInteractive<Grabber> interacted = 
            collision.gameObject.GetComponent<IInteractive<Grabber>>();
        if (interacted != null)
            mAvailable.Add(interacted);
    }

    private void TriggerExit(Collider collision)
    {
        IInteractive<Grabber> interacted =
            collision.gameObject.GetComponent<IInteractive<Grabber>>();
        if (interacted != null)
            mAvailable.Remove(interacted);
    }

    private void PickSelected()
    {
        if(mAvailable.Count > 0)
        {
            float closestDist = float.MaxValue;
            IInteractive<Grabber> closest = null;

            foreach (IInteractive<Grabber> i in mAvailable)
            {
                GameObject obj = i.GetGameObject();
                Vector3 pos = obj.transform.position;
                float dist = Vector3.Magnitude(this.transform.position - pos);
                if(closestDist > dist)
                {
                    closestDist = dist;
                    closest = i;
                }
            }

            if(mSelected != null && mSelected != closest)
                mSelected.Deselect(this);
            
            mSelected = closest;
            mSelected.Select(this);
        }
        else if(mSelected != null)
        {
            mSelected.Deselect(this);
            mSelected = null;
        }
    }

    public void Init(Mover.MovementType movementType, GameObject handPrefab)
    {
        mHand = Instantiate(handPrefab);
        mHand.SetActive(true);
        mMover = Mover.GetMoverFromType(movementType, this.gameObject);
        mAvailable = new List<IInteractive<Grabber>>();
        mDetector = mHand.gameObject.AddComponent<TriggerDetector>();
        mDetector.mTriggerEnterEvent += TriggerEnter;
        mDetector.mTriggerExitEvent += TriggerExit;
    }

    public abstract void Grab();
    public abstract void Release();
    public abstract void Activate();
    public abstract void Deactivate();

    public void MoveHand(Vector3 position, Quaternion rotation)
    {
        mHand.transform.position = position;
        mHand.transform.rotation = rotation;
    }

    public void HideHand()
    {
        mHand.SetActive(false);
    }

    public IInteractive<Grabber> GetSelected()
    {
        return mSelected;
    }
    public IInteractive<Grabber> GetGrabbed()
    {
        return mGrabbed;
    }
}
