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
    [SerializeField]
    private Vector3 mMoveTarget;

    /// <summary>
    /// Blackboard key for move target.
    /// </summary>
    private string mMoveTargetKey = "MoveTargetKey";

    /// <summary>
    /// How fast the minion can move towards a target.
    /// </summary>
    [SerializeField]
    private float mMoveSpeed;

    /// <summary>
    /// Key for movment speed value.
    /// </summary>
    private string mMoveSpeedKey = "MoveSpeedKey";

    /// <summary>
    /// How close to a target the minion needs to be before it considers itself as arrived.
    /// </summary>
    [SerializeField]
    private float mMoveThreshold;

    /// <summary>
    /// Balackboard key for movement threshold.
    /// </summary>
    private string mMoveThresholdKey = "MoveThresholdKey";

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

    /// <summary>
    /// Behavior node controlling where the minion is facing while wandering.
    /// </summary>
    private FaceTargetNode mFaceNode;

    /// <summary>
    /// How fast to rotate towards target while moving around.
    /// </summary>
    [SerializeField]
    private float mRotationSpeed;

    /// <summary>
    /// Accessors for rotation speed.
    /// </summary>
    public float RotationSpeed
    {
        get => mRotationSpeed;
        set
        {
            mRotationSpeed = value;
            mFaceNode.RotateSpeed = mRotationSpeed;
        }
    }

    /// <summary>
    /// Blackboard key for rotation speed.
    /// </summary>
    private string mRotationSpeedKey = "MoveRotationSpeedKey";

    /// <summary>
    /// Tolerance between current rotation and target rotation, in degrees.
    /// </summary>
    [SerializeField]
    private float mRotationThreshold;

    /// <summary>
    /// Key for accessing rotation threshold in behavior tree blackboard.
    /// </summary>
    private string mRotationThresholdKey = "RotationThresholdKey";

    public float RotationThreshold
    {
        get => mRotationThreshold;
        set
        {
            mRotationThreshold = value;
            mFaceNode.RotationThreshold = mRotationThreshold;
        }
    }


    /* Random Position Members */

    /// <summary>
    /// Bounds of movement for minion on X, Y, Z.
    /// </summary>
    [SerializeField]
    private float[] mMinMaxRange;

    /// <summary>
    /// Blackboard key for min/max range.
    /// </summary>
    private string mMinMaxKey = "MinMaxKey";

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
    [SerializeField]
    private float mWaitTime;

    /// <summary>
    /// Blackboard key for wait time.
    /// </summary>
    private string mWaitKey = "WaitKey";

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
    private string mAttackingKey = "AttackingKey";

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
    /// Behavior node governing attack.
    /// </summary>
    private BasicMinionAttackNode mAttackNode;

    /// <summary>
    /// Position of other minion to attack.
    /// </summary>
    [SerializeField]
    private Vector3 mAttackTarget;

    /// <summary>
    /// Blackboard key for attack target.
    /// </summary>
    private string mAttackTargetKey = "AttackTargetKey";

    /// <summary>
    /// How fast the minion can move towards an attack target.
    /// </summary>
    [SerializeField]
    private float mAttackSpeed;

    /// <summary>
    /// Key for movment speed value.
    /// </summary>
    private string mAttackSpeedKey = "AttackSpeedKey";

    /// <summary>
    /// How close to an attack target the minion needs to be before it attacks.
    /// </summary>
    private float mAttackDistance;

    /// <summary>
    /// Balackboard key for movement threshold.
    /// </summary>
    private string mAttackDistanceKey = "AttackDistanceKey";

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

    private FaceTargetNode mArmRotator;

    /// <summary>
    /// Key for arm rotation target.
    /// </summary>
    private string mArmRotationTargetKey = "RotateArmTargetKey";

    /// <summary>
    /// Direction to rotate the arm towards.
    /// </summary>
    private Vector3 mArmRotationTarget;

    public Vector3 ArmRotationTarget
    {
        get => mArmRotationTarget;
        set
        {
            mArmRotationTarget = value;
            mArmRotator.Target = mArmRotationTarget;
        }
    }

    /// <summary>
    /// Key for arm rotation speed.
    /// </summary>
    private string mArmRotationSpeedKey = "RotateArmSpeedKey";

    /// <summary>
    /// How fast the arm should rotate into place.
    /// </summary>
    [SerializeField]
    private float mArmRotationSpeed;

    /// <summary>
    /// Accessor for arm rotation speed.
    /// </summary>
    public float ArmRotationSpeed
    {
        get => mArmRotationSpeed;
        set 
        {
            mArmRotationSpeed = value;
            mArmRotator.RotateSpeed = mArmRotationSpeed;
        }
    }

    /// <summary>
    /// Blackboard key for arm rotation threshold.
    /// </summary>
    private string mArmRotationThresholdKey = "ArmRotationThresholdKey";

    /// <summary>
    /// Arm rotation threshold.
    /// </summary>
    [SerializeField]
    private float mArmRotationThreshold;

    /// <summary>
    /// Accessor for arm rotation threshold.
    /// </summary>
    public float ArmRotationThreshold
    {
        get => mArmRotationThreshold;
        set 
        { 
            mArmRotationThreshold = value;
            mArmRotator.RotationThreshold = mArmRotationThreshold;
        }
    }
    

    /// <summary>
    /// Sets up a behavior tree according to desired behavior of Basic Minions.
    /// </summary>
    protected override void BuildTree()
    {
        mMinion = this.GetComponent<BasicMinion>();
        Vector3 rand = new Vector3(
            Random.Range(mMinMaxRange[0], mMinMaxRange[1]),
            Random.Range(mMinMaxRange[2], mMinMaxRange[3]),
            Random.Range(mMinMaxRange[4], mMinMaxRange[5]));

        //mRoot = this.gameObject.AddComponent<SelectorNode>();
        mRoot = this.gameObject.AddComponent<SequenceNode>();
        mRoot.Tree = this;

        mMoveNode = this.gameObject.AddComponent<MoveToSimple>();
        mMoveNode.Tree = this;
        mMoveNode.SetKeys(mMoveTargetKey, mMoveSpeedKey, mMoveThresholdKey);
        mMoveNode.SetValues(rand, mMoveSpeed, mMoveThreshold);

        mFaceNode = this.gameObject.AddComponent<FaceTargetNode>();
        mFaceNode.Tree = this;
        mFaceNode.ToRotate = this.gameObject.transform;
        mFaceNode.TargetKey = mMoveTargetKey;
        mFaceNode.FaceImmediately = false;
        mFaceNode.RotateSpeedKey = mRotationSpeedKey;
        mFaceNode.RotateSpeed = mRotationSpeed;
        mFaceNode.RotationThresholdKey = mRotationThresholdKey;
        mFaceNode.RotationThreshold = mRotationThreshold;
        mFaceNode.SucceedWhileRotating = true;

        mRandomNode = this.gameObject.AddComponent<GetRandomPointNode>();
        mRandomNode.Tree = this;
        mRandomNode.SetKeys(mMinMaxKey, mMoveTargetKey);
        mRandomNode.SetMinMaxVals(mMinMaxRange);

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

        mArmRotator = this.gameObject.AddComponent<FaceTargetNode>();
        mArmRotator.Tree = this;
        mArmRotator.ToRotate = Minion.ArmTransform;
        mArmRotator.TargetKey = mArmRotationTargetKey;
        mArmRotator.Target = Vector3.up;
        mArmRotator.RotateSpeedKey = mArmRotationSpeedKey;
        mArmRotator.RotateSpeed = mArmRotationSpeed;
        mArmRotator.RotationThresholdKey = mArmRotationThresholdKey;
        mArmRotator.RotationThreshold = mArmRotationThreshold;
        mArmRotator.SucceedWhileRotating = true;
        mArmRotator.FaceImmediately = false;
       


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

        //mRoot.AddChild(mArmRotator);
        mRoot.AddChild(mFaceNode);
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
