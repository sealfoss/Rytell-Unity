using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomPointNode : BehaviorNode
{

    private string mMinMaxKey;
    private string mPointKey;

    public void SetKeys(string minMaxKey, string pointKey)
    {
        mMinMaxKey = minMaxKey;
        mPointKey = pointKey;
    }

    public void SetMinMaxVals(float[] minMax)
    {
        mTree.SetBlackboardValue<float[]>(mMinMaxKey, minMax);
    }

    private float[] GetMinMaxValues()
    {
        float[] minMax = mTree.GetBlackboardValue<float[]>(mMinMaxKey);
        return minMax;
    }

    private void SetPointValue(Vector3 point)
    {
        Vector3[] value = { point };
        mTree.SetBlackboardValue(mPointKey, value);
    }

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
