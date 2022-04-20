using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : BehaviorNode
{
    public override bool Run()
    {
        bool success = false;

        foreach(BehaviorNode child in base.mChildren)
        {
            if (child.Run())
            {
                success = true;
                break;
            }
        }

        return success;
    }
}
