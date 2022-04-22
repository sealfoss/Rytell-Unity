using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior tree tailored to desired behavior of basic minion.
/// </summary>
public class BasicMinionBehavior : BehaviorTree
{

    /* Minion Controller Members */

    /// <summary>
    /// Minion controller owning this behavior.
    /// </summary>
    private BasicMinion mMinion;

    /// <summary>
    /// Accessors for minion controller.
    /// </summary>
    public BasicMinion Minion { get => mMinion; set => mMinion = value; }


    /* Movement Members */ 

    /// <summary>
    /// Where the minion is headed.
    /// </summary>
    private Vector3 mMoveTarget;

    /// <summary>
    /// Blackboard key for move target.
    /// </summary>
    [SerializeField]
    private string mMoveTargetKey;

    /// <summary>
    /// How fast the minion can move towards a target.
    /// </summary>
    private float mMoveSpeed;

    /// <summary>
    /// Key for movment speed value.
    /// </summary>
    [SerializeField]
    private string mMoveSpeedKey;

    /// <summary>
    /// How close to a target the minion needs to be before it considers itself as arrived.
    /// </summary>
    private float mMoveThreshold;

    /// <summary>
    /// Balackboard key for movement threshold.
    /// </summary>
    [SerializeField]
    private string mMoveThresholdKey;

    /// <summary>
    /// Behavior node governing minion movment.
    /// </summary>
    private MoveTo mMoveNode;

    /// <summary>
    /// Accessors for move target.
    /// </summary>
    public Vector3 MoveTarget 
    { 
        get => mMoveTarget;
        set
        {
            mMoveTarget = value;
            mMoveNode.Target = mMoveTarget;
        }
    }

    /// <summary>
    /// Accessors for moment speed member.
    /// </summary>
    public float MoveSpeed 
    { 
        get => mMoveSpeed;
        set
        {
            mMoveSpeed = value;
            mMoveNode.Speed = mMoveSpeed;
        }
    }

    /// <summary>
    /// Accessors for movement threshold.
    /// </summary>
    public float MoveThreshold 
    { 
        get => mMoveThreshold;
        set
        {
            mMoveThreshold = value;
            mMoveNode.Threshold = mMoveThreshold;
        }
    }


    /* Random Position Members */

    /// <summary>
    /// Bounds of movement for minion on X, Y, Z.
    /// </summary>
    private float[] mMinMaxRange;

    /// <summary>
    /// Blackboard key for min/max range.
    /// </summary>
    [SerializeField]
    private string mMinMaxKey;

    /// <summary>
    /// Behavior node responsible for generating random move-to positions.
    /// </summary>
    private GetRandomPointNode mRandomNode;

    /// <summary>
    /// Accessors for movement bounds.
    /// </summary>
    public float[] MinMaxRange 
    { 
        get => mMinMaxRange;
        set
        {
            mMinMaxRange = value;
            mRandomNode.SetMinMaxVals(mMinMaxRange);
        }
    }


    /* Wait Time Members */

    /// <summary>
    /// How long the minion should wait after arriving at its target before moving on to something else.
    /// </summary>
    private float mWaitTime;

    /// <summary>
    /// Blackboard key for wait time.
    /// </summary>
    [SerializeField]
    private string mWaitKey;

    /// <summary>
    /// Behavior node controlling wait time.
    /// </summary>
    private WaitNode mWaitNode;

    /// <summary>
    /// Accessors for wait time.
    /// </summary>
    public float WaitTime
    {
        get => mWaitTime;
        set => mWaitTime = value;
    }


    /* Attack Logic Members */

    /// <summary>
    /// Bool indicating whether there is a valid attack target.
    /// Needed because you can't set Vector3's to null.
    /// </summary>
    private bool mAttacking;

    /// <summary>
    /// Blackboard key for attacking bool.
    /// </summary>
    [SerializeField]
    private string mAttackingKey;

    /// <summary>
    /// Bool check node for attack.
    /// </summary>
    private CheckBoolNode mAttackingBoolNode;

    /// <summary>
    /// Accessors for attacking bool.
    /// </summary>
    public bool Attacking
    {
        get => mAttacking;
        set
        {
            mAttacking = value;
            mAttackingBoolNode.Val = mAttacking;
        }
    }

    /// <summary>
    /// Attack node key.
    /// </summary>
    [SerializeField]
    private string mAttackKey;

    /// <summary>
    /// Behavior node governing attack.
    /// </summary>
    private BasicMinionAttackNode mAttackNode;

    /// <summary>
    /// Position of other minion to attack.
    /// </summary>
    private Vector3 mAttackTarget;

    /// <summary>
    /// Blackboard key for attack target.
    /// </summary>
    [SerializeField]
    private string mAttackTargetKey;

    /// <summary>
    /// How fast the minion can move towards an attack target.
    /// </summary>
    private float mAttackSpeed;

    /// <summary>
    /// Key for movment speed value.
    /// </summary>
    [SerializeField]
    private string mAttackSpeedKey;

    /// <summary>
    /// How close to an attack target the minion needs to be before it attacks.
    /// </summary>
    private float mAttackDistance;

    /// <summary>
    /// Balackboard key for movement threshold.
    /// </summary>
    [SerializeField]
    private string mAttackDistanceKey;

    /// <summary>
    /// Behavior node governing minion movment towards attack target.
    /// </summary>
    private MoveTo mAttackMoveNode;

    /// <summary>
    /// Accessors for attack target.
    /// </summary>
    public Vector3 AttackTarget
    {
        get => mAttackTarget;
        set
        {
            mAttackTarget = value;
            mAttackMoveNode.Target = mAttackTarget;
        }
    }

    /// <summary>
    /// Accessors for attack distance.
    /// </summary>
    public float AttackDistance
    {
        get => mAttackDistance;
        set
        {
            mAttackDistance = value;
            mAttackMoveNode.Threshold = mAttackDistance;
        }
    }

    /// <summary>
    /// Accessors for attack speed.
    /// </summary>
    public float AttackSpeed
    {
        get => mAttackSpeed;
        set
        {
            mAttackSpeed = value;
            mAttackMoveNode.Speed = mAttackSpeed;
        }
    }

    /// <summary>
    /// Sets up a behavior tree according to desired behavior of Basic Minions.
    /// </summary>
    protected override void BuildTree()
    {
        float[] minMax = { -8, 8, 1, 1, -8, 8 };
        Vector3 rand = new Vector3(
            Random.Range(minMax[0], minMax[1]),
            Random.Range(minMax[2], minMax[3]),
            Random.Range(minMax[4], minMax[5]));
        float speed = Random.Range(1, 20);

        //mRoot = this.gameObject.AddComponent<SelectorNode>();
        mRoot = this.gameObject.AddComponent<SequenceNode>();
        mRoot.Tree = this;

        mMoveNode = this.gameObject.AddComponent<MoveToSimple>();
        mMoveNode.Tree = this;
        mMoveNode.SetKeys(mMoveTargetKey, mMoveSpeedKey, mMoveThresholdKey);
        mMoveNode.SetValues(rand, speed, 0.1f);

        mRandomNode = this.gameObject.AddComponent<GetRandomPointNode>();
        mRandomNode.Tree = this;
        mRandomNode.SetKeys(mMinMaxKey, mMoveTargetKey);
        mRandomNode.SetMinMaxVals(minMax);

        mWaitNode = this.gameObject.AddComponent<WaitNode>();
        mWaitNode.Tree = this;
        mWaitNode.WaitTimeKey = mWaitKey;
        mWaitNode.WaitTime = 2.0f;

        mAttackingBoolNode = this.gameObject.AddComponent<CheckBoolNode>();
        mAttackingBoolNode.Tree = this;
        mAttackingBoolNode.Key = mAttackingKey;
        mAttackingBoolNode.Val = false;

        mAttackMoveNode = this.gameObject.AddComponent<MoveToSimple>();
        mAttackMoveNode.Tree = this;
        mAttackMoveNode.SetKeys(mAttackTargetKey, mAttackSpeedKey, mAttackDistanceKey);

        mAttackNode = this.gameObject.AddComponent<BasicMinionAttackNode>();
        mAttackNode.Tree = this;
        
        SequenceNode attackSequence = this.gameObject.AddComponent<SequenceNode>();
        attackSequence.Tree = this;
        attackSequence.AddChild(mAttackingBoolNode);
        attackSequence.AddChild(mMoveNode);
        attackSequence.AddChild(null);
        
        SequenceNode wanderSequence = this.gameObject.AddComponent<SequenceNode>();
        wanderSequence.Tree = this;
        wanderSequence.AddChild(mMoveNode);
        wanderSequence.AddChild(mWaitNode);
        wanderSequence.AddChild(mRandomNode);

        //mRoot.AddChild(attackSequence);
        //mRoot.AddChild(wanderSequence);

        
        mRoot.AddChild(mMoveNode);
        mRoot.AddChild(mWaitNode);
        mRoot.AddChild(mRandomNode);
        
    }

    /// <summary>
    /// Custom behavior node for Basic Minions attack.
    /// </summary>
    public class BasicMinionAttackNode : BehaviorNode
    {
        public override bool Run()
        {
            // To be continued...
            return false;
        }
    }
}
