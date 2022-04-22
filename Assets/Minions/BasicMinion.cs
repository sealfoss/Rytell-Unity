using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs basic minion behavior, status, and initialization.
/// </summary>
public class BasicMinion : MinionController
{
    /// <summary>
    /// Reference to governing basic minion behavior tree.
    /// </summary>
    private BasicMinionBehavior mMinionBehavior;

    /// <summary>
    /// Minion decided to be worthy, and closest, opponent.
    /// </summary>
    private MinionController mToAttack;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        mMinionBehavior = this.gameObject.GetComponent<BasicMinionBehavior>();
        mMinionBehavior.Minion = this;
        mBehavior = (BehaviorTree)mMinionBehavior;
        mBehavior.Activate();
    }

    /// <summary>
    /// Update is called every frame.
    /// </summary>
    private void Update()
    {
        DetectEnemies();
        RunBehavior();
    }

    /// <summary>
    /// If there are enemies within range, the closest is found,
    /// designated as an attack target, and attacked.
    /// </summary>
    private void DetectEnemies()
    {
        mToAttack = GetClosestEnemey();
        if(mToAttack != null)
        {
            mMinionBehavior.AttackTarget = mToAttack.transform.position;
            mMinionBehavior.Attacking = true;
        }
        else
        {
            mMinionBehavior.Attacking = false;
        }
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
