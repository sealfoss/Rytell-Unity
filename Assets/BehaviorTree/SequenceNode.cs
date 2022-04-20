using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : BehaviorNode
{
    public override bool Run()
    {
        bool success = true;

        foreach (BehaviorNode child in base.mChildren)
        {
            if (!child.Run())
            {
                success = false;
                break;
            }
        }

        return success;
    }
}
