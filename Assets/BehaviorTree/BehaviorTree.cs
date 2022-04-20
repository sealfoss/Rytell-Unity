using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    protected BehaviorNode mRoot;
    private Dictionary<string, object> mBlackboard;
    private List<BehaviorNode> mNodes;
    private bool activated;

    private void Awake()
    {
        mNodes = new List<BehaviorNode>();
        mBlackboard = new Dictionary<string, object>();
        BuildTree();
        SearchTree(mRoot, mNodes);
    }

    protected abstract void BuildTree();

    private void SearchTree(BehaviorNode node, List<BehaviorNode> list)
    {
        list.Add(node);
        node.SetTree(this);
        foreach(BehaviorNode child in node.GetChildren())
        {
            SearchTree(child, list);
        }
    }

    public void RunTree()
    {
        if (activated)
        {
            mRoot.Run();
        }
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public void SetBlackboardValue<T>(string key, T value) where T : class
    {
        if(mBlackboard.ContainsKey(key))
            mBlackboard.Remove(key);
            
        mBlackboard.Add(key, value);
    }

    public T GetBlackboardValue<T>(string key) where T : class
    {
        return mBlackboard[key] as T;
    }
}
