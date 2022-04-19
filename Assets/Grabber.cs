using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grabber : MonoBehaviour
{
    protected DeityController mDeity;
    protected IInteractive<Grabber> mGrabbed;
    protected Mover mMover;
    protected GameObject mHand;



    public void Init(Mover.MovementType movementType, GameObject handPrefab)
    {
        mHand = Instantiate(handPrefab);
        mHand.SetActive(true);
        mMover = Mover.GetMoverFromType(movementType, this.gameObject);
    }

    public abstract void Grab(IInteractive<Grabber> toGrab);
    public abstract void Release(IInteractive<Grabber> toRelease);
    public abstract void Activate(IInteractive<Grabber> toActivate);
    public abstract void Deactivat(IInteractive<Grabber> toDeactivate);

    public void MoveHand(Vector3 position, Quaternion rotation)
    {
        mHand.transform.position = position;
        mHand.transform.rotation = rotation;
    }

    public void HideHand()
    {
        mHand.SetActive(false);
    }
}
