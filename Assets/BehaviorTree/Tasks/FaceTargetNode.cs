using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates agents towards a given target.
/// </summary>
public class FaceTargetNode : BehaviorNode
{
    /// <summary>
    /// Blackboard key of target to rotate towards.
    /// </summary>
    private string mTargetKey;

    /// <summary>
    /// Accessors for target key.
    /// </summary>
    private string TargetKey { get => mTargetKey; set => mTargetKey = value; }

    /// <summary>
    /// Target to rotate towards.
    /// </summary>
    private Vector3 mTarget;

    /// <summary>
    /// Accessors of rotation target.
    /// </summary>
    private Vector3 Target
    {
        get
        {
            Vector3[] val = mTree.GetBlackboardValue<Vector3[]>(mTargetKey);
            mTarget = val[0];
            return mTarget;
        }
        set
        {
            mTarget = value;
            Vector3[] val = { mTarget };
            mTree.SetBlackboardValue(mTargetKey, val);
        }
    }

    /// <summary>
    /// Key for rotation speed value.
    /// </summary>
    private string mRotateSpeedKey;

    /// <summary>
    /// Accessors for rotation speed key.
    /// </summary>
    private string RotateSpeedKey { get => mRotateSpeedKey; set => mRotateSpeedKey = value; }

    /// <summary>
    /// Speed of rotation towards target.
    /// </summary>
    private float mRotateSpeed;

    /// <summary>
    /// Acessors for rotation speed.
    /// </summary>
    private float RotateSpeed
    {
        get
        {
            float[] val = mTree.GetBlackboardValue<float[]>(mRotateSpeedKey);
            mRotateSpeed = val[0];
            return mRotateSpeed;
        }
        set
        {
            mRotateSpeed = value;
            float[] val = { mRotateSpeed };
            mTree.SetBlackboardValue(mRotateSpeedKey, val);
        }
    }

    /// <summary>
    /// Executes the behavior inherent to the owning node.
    /// </summary>
    /// <returns>Returns true if behavior is successfully executed, false if otherwise.</returns>
    public override bool Run()
    {
        Vector3 dir = (Target - this.transform.position).normalized;
        Quaternion look = Quaternion.LookRotation(dir);
        float rotSpeed = RotateSpeed * Time.deltaTime;
        Quaternion rot = Quaternion.Lerp(this.transform.rotation, look, rotSpeed);
        this.transform.rotation = rot;
        return true;
    }
}

