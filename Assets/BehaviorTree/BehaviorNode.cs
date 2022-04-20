using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode : MonoBehaviour
{
    protected BehaviorTree mTree;
    protected List<BehaviorNode> mChildren;

    public BehaviorNode()
    {
        mChildren = new List<BehaviorNode>();
    }

    public void SetTree(BehaviorTree tree)
    {
        mTree = tree;
    }

    public abstract bool Run();

    public void AddChild(BehaviorNode child)
    {
        mChildren.Add(child);
    }

    public List<BehaviorNode> GetChildren()
    {
        return mChildren;
    }
}
