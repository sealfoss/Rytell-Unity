using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveTo : BehaviorNode
{
    protected string mTargetKey;
    protected string mSpeedKey;
    protected string mThresholdKey;

    public void SetKeys(string targetKey, string speedKey, string thresholdKey)
    {
        mTargetKey = targetKey;
        mSpeedKey = speedKey;
        mThresholdKey = thresholdKey;
    }

    public void SetValues(float speed, Vector3 target, float threshold)
    {
        SetSpeed(speed);
        SetTarget(target);
        SetThreshold(threshold);
    }

    public void SetSpeed(float speed)
    {
        float[] value = { speed };
        mTree.SetBlackboardValue(mSpeedKey, value);
    }

    public void SetTarget(Vector3 target)
    {
        Vector3[] value = { target };
        mTree.SetBlackboardValue(mTargetKey, value);
    }

    public void SetThreshold(float threshold)
    {
        float[] value = { threshold };
        mTree.SetBlackboardValue(mThresholdKey, value);
    }

    protected float GetSpeed()
    {
        float[] speed = mTree.GetBlackboardValue<float[]>(mSpeedKey);
        return speed[0];
    }

    public Vector3 GetTarget()
    {
        Vector3 target = Vector3.zero;
        Vector3[] value = mTree.GetBlackboardValue<Vector3[]>(mTargetKey);
        if (value != null)
            target = target = value[0];
        return target;
    }

    protected float GetThreshold()
    {
        float[] threshold = mTree.GetBlackboardValue<float[]>(mThresholdKey);
        return threshold[0];
    }

    protected bool CheckIfWithinThreshold()
    {
        Vector3 current = mTree.gameObject.transform.position;
        Vector3 target = GetTarget();
        float dist = Vector3.Magnitude(current - target);
        float threshold = GetThreshold();
        bool withinDistance = dist <= threshold;
        return withinDistance;
    }
}
