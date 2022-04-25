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
    /// Color of minion's weapon.
    /// </summary>
    [SerializeField]
    private Color mSpearColor;

    /// <summary>
    /// Color of minion's eye when angry.
    /// </summary>
    [SerializeField]
    private Color mMadColor;

    /// <summary>
    /// Color of minion's eye when not angry.
    /// </summary>
    [SerializeField]
    private Color mChillColor;

    /// <summary>
    /// Minion's suit color.
    /// </summary>
    [SerializeField]
    private Color mSuitColor;

    /// <summary>
    /// Minion's eye.
    /// </summary>
    [SerializeField]
    private GameObject mEye;

    /// <summary>
    /// Minion's weapon bearing arm.
    /// </summary>
    [SerializeField]
    private GameObject mArm;

    /// <summary>
    /// Accessor for mArm child component.
    /// </summary>
    public Transform ArmTransform { get => mArm.transform; }

    /// <summary>
    /// Minion's spear.
    /// </summary>
    [SerializeField]
    private GameObject mSpear;

   

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
        mUnselectedColor = mSuitColor;
        SetColor();
        mHitter.mTriggerEnterEvent += OnHitterEnter;
        mHitter.mTriggerExitEvent += OnHitterExit;
        mDeathEffect.transform.parent = null;
        SetEyeColor(mChillColor);
        mSpear.GetComponent<MeshRenderer>().material.color = mSpearColor;
    }

    /// <summary>
    /// Update is called every frame.
    /// </summary>
    private void Update()
    {
        DetectEnemies();
        RunBehavior();
        CheckDeathDestroy();
    }

    /// <summary>
    /// Checks whether enough time has passed since death to 
    /// </summary>
    private void CheckDeathDestroy()
    {
        if(mDead)
        {
            mTimeSinceDeath += Time.deltaTime;

            if(mTimeSinceDeath > mDeathDuration)
            {
                this.transform.position = new Vector3(0, -100, 0);
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Tells this minion to go die, cleans up gameobject, etc.
    /// </summary>
    public override void Die()
    {
        this.mDead = true;
        mDeathEffect.Play();
        mTimeSinceDeath = 0;
        mHitter.mTriggerEnterEvent -= OnHitterEnter;
        mHitter.mTriggerExitEvent -= OnHitterExit;
        Rigidbody r = this.GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.None;
        r.isKinematic = false;
        r.useGravity = true;
        this.mBehavior.Deactivate();
        mAvailable = false;
    }

    /// <summary>
    /// Hits another minion with this minion's spear.
    /// </summary>
    private void HitVictim()
    {
        mVictim.Die();
        Vector3 dir = (mVictim.transform.position - this.transform.position).normalized;
        mVictim.GetComponent<Rigidbody>().velocity = dir;
    }

    /// <summary>
    /// Called when another object overlapps the minion's spear tip.
    /// </summary>
    /// <param name="collider">Collider of object overlapping spear tip.</param>
    private void OnHitterEnter(Collider collider)
    {
        Transform rootTrans = collider.transform.root;
        GameObject rootObj = rootTrans.gameObject;
        BasicMinion minion = rootObj.GetComponent<BasicMinion>();
        if (minion != null && minion != this && minion == mToAttack && minion != mVictim && minion)
        {
            mVictim = minion;
            HitVictim();
        }
    }

    /// <summary>
    /// Called when another object exits the minion's spear tip.
    /// </summary>
    /// <param name="collider">Collider of object exiting spear tip.</param>
    private void OnHitterExit(Collider collider)
    {
        Transform rootTrans = collider.transform.root;
        GameObject rootObj = rootTrans.gameObject;
        BasicMinion minion = rootObj.GetComponent<BasicMinion>();
        if (minion != null && minion != this && mVictim != null && minion == mVictim)
        {
            mVictim = null;
        }
    }

    /// <summary>
    /// Sets the color of the minion's "eye".
    /// </summary>
    /// <param name="eyeColor">New color for minion's eye.</param>
    private void SetEyeColor(Color eyeColor)
    {
        this.mEye.GetComponent<MeshRenderer>().material.color = eyeColor;
    }

    /// <summary>
    /// If there are enemies within range, the closest is found,
    /// designated as an attack target, and attacked.
    /// </summary>
    private void DetectEnemies()
    {
        if (Dead)
            return;
        mToAttack = GetClosestEnemey();
        if(mToAttack != null)
        {
            mMinionBehavior.AttackTarget = mToAttack.transform.position;
            mMinionBehavior.MoveTarget = mToAttack.transform.position;
            mMinionBehavior.Attacking = true;
            Vector3 dir = (mToAttack.transform.position - this.transform.position);
            dir.y = 0;
            dir.Normalize();
            Quaternion rot = Quaternion.LookRotation(dir);
            mArm.transform.rotation = rot;
            SetEyeColor(mMadColor);
        }
        else
        {
            SetEyeColor(mChillColor);
            mMinionBehavior.Attacking = false;
            Vector3 dir = Vector3.up;
            Quaternion rot = Quaternion.LookRotation(dir);
            mArm.transform.rotation = Quaternion.Lerp(mArm.transform.rotation, rot, Time.deltaTime * mMinionBehavior.ArmRotationSpeed);
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
