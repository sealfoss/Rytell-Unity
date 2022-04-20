using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : BehaviorNode
{
    private float mElapsed;
    private string mWaitTimeKey;

    public void SetKey(string waitTimeKey)
    {
        mWaitTimeKey = waitTimeKey;
    }

    public void SetWaitTime(float waitTime)
    {
        float[] value = { waitTime };
        mTree.SetBlackboardValue<float[]>(mWaitTimeKey, value);
    }

    public float GetWaitTime()
    {
        float[] waitTime = mTree.GetBlackboardValue<float[]>(mWaitTimeKey);
        return waitTime[0];
    }

    public override bool Run()
    {
        bool success = false;
        mElapsed += Time.deltaTime;

        if(mElapsed >= GetWaitTime())
        {
            mElapsed = 0;
            success = true;
        }

        return success;
    }
}
