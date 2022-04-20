using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentNode : BehaviorNode
{
    protected List<BehaviorNode> mChildren;

    public ParentNode()
    {
        mChildren = new List<BehaviorNode>();
    }


}
