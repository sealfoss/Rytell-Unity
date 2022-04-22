using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes child behaviors one at a time, stops at the first to succeed.
/// Returns true should any executed child behaviors returns true.
/// </summary>
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
