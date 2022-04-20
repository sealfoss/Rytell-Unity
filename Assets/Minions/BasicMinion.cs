using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMinion : MinionController
{


    protected override void Awake()
    {
        base.Awake();
        mBehavior = this.gameObject.AddComponent<BasicMinionBehavior>();
        mBehavior.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        RunBehavior();
    }

    public override void Grab(Grabber grabbing)
    {
        base.Grab(grabbing);
        mBehavior.Deactivate();
        SetColor();
    }

    public override void Release(Grabber releasing)
    {
        base.Release(releasing);
        mBehavior.Activate();
        SetColor();
    }

    public override void Activate(Grabber activating)
    {

    }

    public override void Deactivate(Grabber deactivating)
    { 

    }

    public override void Select(Grabber selecting)
    {
        mSelectedBy.Add(selecting);
        SetColor();
    }

    public override void Deselect(Grabber deselecting)
    {
        mSelectedBy.Remove(deselecting);
        SetColor();
    }
}
