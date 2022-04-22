using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets the vector of a random position within some given range.
/// </summary>
public class GetRandomPointNode : BehaviorNode
{
    /// <summary>
    /// Blackboard key for float array representing min and max values on x, y, z.
    /// </summary>
    private string mMinMaxKey;

    /// <summary>
    /// Blackboard key used to store generated random position.
    /// </summary>
    private string mPointKey;

    public void SetKeys(string minMaxKey, string pointKey)
    {
        mMinMaxKey = minMaxKey;
        mPointKey = pointKey;
    }

    /// <summary>
    /// Sets min and max value float array.
    /// </summary>
    /// <param name="minMax"></param>
    public void SetMinMaxVals(float[] minMax)
    {
        mTree.SetBlackboardValue<float[]>(mMinMaxKey, minMax);
    }

    /// <summary>
    /// Gets the float array of min/max values.
    /// </summary>
    /// <returns>Float array containing min/max values.</returns>
    private float[] GetMinMaxValues()
    {
        float[] minMax = mTree.GetBlackboardValue<float[]>(mMinMaxKey);
        return minMax;
    }

    /// <summary>
    /// Sets the value for the vector stored in the blackboard at given key.
    /// </summary>
    /// <param name="point"></param>
    private void SetPointValue(Vector3 point)
    {
        Vector3[] value = { point };
        mTree.SetBlackboardValue(mPointKey, value);
    }

    /// <summary>
    /// Gets the value stored in the blackboard at teh given key.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPoint()
    {
        Vector3[] value = mTree.GetBlackboardValue<Vector3[]>(mPointKey);
        return value[0];
    }

    public override bool Run()
    {
        float[] minMax = GetMinMaxValues();
        Vector3 point = new Vector3(
            Random.Range(minMax[0], minMax[1]),
            Random.Range(minMax[2], minMax[3]),
            Random.Range(minMax[4], minMax[5]));
        SetPointValue(point);
        return true;
    }
}
