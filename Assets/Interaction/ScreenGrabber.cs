using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGrabber : Grabber
{
    public override void Grab()
    {
        if (mSelected != null)
        {
            mSelected.GetGameObject().transform.SetParent(mHand.transform);
            mGrabbed = mSelected;
            mGrabbed.Grab(this);
        }
    }

    public override void Release()
    {
        if (mGrabbed != null)
        {
            mGrabbed.GetGameObject().transform.SetParent(null);
            mGrabbed.Release(this);
            mGrabbed = null;
        }
    }

    public override void Activate()
    {
        if(mGrabbed != null)
        {
            mActivated = mGrabbed;
            mActivated.Activate(this);
        }
    }

    public override void Deactivate()
    {
        if (mActivated != null)
        {
            mActivated.Deactivate(this);
            mActivated = null;
        }
    }
}
