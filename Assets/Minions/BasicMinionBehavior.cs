using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMinionBehavior : BehaviorTree
{
    BasicMinion minion;

    protected override void BuildTree()
    {
        string targetKey = "target";
        string speedKey = "speed";
        string thresholdKey = "threshold";
        string minMaxKey = "minMaxVals";
        string waitKey = "waitTime";
        float[] minMax = { -8, 8, 1, 1, -8, 8 };
        Vector3 rand = new Vector3(
        Random.Range(minMax[0], minMax[1]),
        Random.Range(minMax[2], minMax[3]),
        Random.Range(minMax[4], minMax[5]));
        float speed = Random.Range(1, 20);

        mRoot = this.gameObject.AddComponent<SequenceNode>();
        mRoot.SetTree(this);

        MoveTo move = this.gameObject.AddComponent<MoveToSimple>();
        move.SetTree(this);
        move.SetKeys(targetKey, speedKey, thresholdKey);
        move.SetValues(speed, rand, 0.1f);

        GetRandomPointNode point = this.gameObject.AddComponent<GetRandomPointNode>();
        point.SetTree(this);
        point.SetKeys(minMaxKey, targetKey);
        point.SetMinMaxVals(minMax);

        WaitNode wait = this.gameObject.AddComponent<WaitNode>();
        wait.SetTree(this);
        wait.SetKey(waitKey);
        wait.SetWaitTime(2.0f);

        mRoot.AddChild(move);
        mRoot.AddChild(wait);
        mRoot.AddChild(point);
    }
}
