using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic minion controller handling behavior, status, and initialization.
/// </summary>
public abstract class MinionController : MonoBehaviour, IInteractive<Grabber>, IBehaves
{
    /// <summary>
    /// The maximum amount of grabbers that can grab this minion at once.
    /// </summary>
    [SerializeField]
    protected int mMaxHands;
    
    /// <summary>
    /// How many grabbers have grabbed this minion.
    /// </summary>
    protected int mHands;
    
    /// <summary>
    /// Whether this minion has been activated.
    /// </summary>
    protected bool mActivated;

    /// <summary>
    /// Whether this minion is currently grabbed.
    /// </summary>
    protected bool mGrabbed;
    
    /// <summary>
    /// All of the grabber components that have grabbed this minion.
    /// </summary>
    protected HashSet<Grabber> mGrabbedBy;

    /// <summary>
    /// All of the grabber components that have currently selected this minion.
    /// </summary>
    protected HashSet<Grabber> mSelectedBy;

    /// <summary>
    /// Color of minion while unselected.
    /// </summary>
    [SerializeField]
    protected Color mUnselectedColor;

    /// <summary>
    /// Color of minion while selected.
    /// </summary>
    [SerializeField]
    protected Color mSelectedColor;

    /// <summary>
    /// Color of minion while grabbed.
    /// </summary>
    [SerializeField]
    protected Color mGrabbedColor;

    /// <summary>
    /// Behavior tree governing minion behavior.
    /// </summary>
    protected BehaviorTree mBehavior;

    /// <summary>
    /// How fast the minion should move.
    /// </summary>
    [SerializeField]
    protected float mMovementSpeed;

    /// <summary>
    /// Detects other minions to attack.
    /// </summary>
    protected TriggerDetector mEnemeyDetector;

    /// <summary>
    /// Enemies detected within range of minion.
    /// </summary>
    protected HashSet<MinionController> mEnemies;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        mEnemies = new HashSet<MinionController>();
        mGrabbedBy = new HashSet<Grabber>();
        mSelectedBy = new HashSet<Grabber>();
        mHands = 0;
        mActivated = false;
        mGrabbed = false;
        SetColor();
        SetEnemeyDetector();
    }

    /// <summary>
    /// Gets the closest enemy to this minion.
    /// </summary>
    /// <returns></returns>
    protected MinionController GetClosestEnemey()
    {
        return FindClosestOtherMinion(mEnemies);
    }

    /// <summary>
    /// Decides whether this minion has any enemies in range.
    /// </summary>
    /// <returns></returns>
    protected bool HasEnemey()
    {
        return mEnemies.Count > 0;
    }

    /// <summary>
    /// Called when another collider enters the enemy detector.
    /// </summary>
    /// <param name="collider">Collider that's entered the enemey detector.</param>
    private void EnemeyDetectorEnter(Collider collider)
    {
        MinionController minion = collider.GetComponent<MinionController>();
        if (minion != null)
        {
            mEnemies.Add(minion);
        }
    }

    /// <summary>
    /// Called when another collider exits the enemy detector.
    /// </summary>
    /// <param name="collider">Collider that's exited the enemey detector.</param>
    private void EnemeyDetectorExit(Collider collider)
    {
        MinionController minion = collider.GetComponent<MinionController>();
        if (minion != null)
        {
            mEnemies.Remove(minion);
        }
    }

    /// <summary>
    /// Finds the minion's enemey detector component, stores a reference to it and wires
    /// in events related to enemy detection.
    /// </summary>
    private void SetEnemeyDetector()
    {
        TriggerDetector[] children = this.GetComponentsInChildren<TriggerDetector>();
        
        foreach (TriggerDetector child in children)
        {
            if (child.name.Equals("EnemeyDetector"))
            {
                mEnemeyDetector = child;
                break;
            }
        }

        mEnemeyDetector.mTriggerEnterEvent += EnemeyDetectorEnter;
        mEnemeyDetector.mTriggerExitEvent += EnemeyDetectorExit;
    }

    /// <summary>
    /// Runs the behavior contained in attached behavior tree.
    /// </summary>
    public void RunBehavior()
    {
        mBehavior.RunTree();
    }

    /// <summary>
    /// Sets the color of this minion according to grabbed and selected status.
    /// </summary>
    public void SetColor()
    {
        Color color;
        
        if(IsGrabbed())
            color = mGrabbedColor;
        else if(IsSelected())
            color = mSelectedColor;
        else
            color = mUnselectedColor;

        GetComponent<MeshRenderer>().material.color = color;
    }

    public virtual void Grab(Grabber grabbing)
    {
        mGrabbedBy.Add(grabbing);
        mGrabbed = true;
        mHands++;
    }

    public virtual void Release(Grabber releasing)
    {
        mGrabbedBy.Remove(releasing);
        mGrabbed = false;
        mHands--;
    }
    public abstract void Activate(Grabber activating);
    public abstract void Deactivate(Grabber deactivating);
    public abstract void Select(Grabber selecting);
    public abstract void Deselect(Grabber deselecting);

    /// <summary>
    /// Decides whether this minion is currently grabbed by a user.
    /// </summary>
    /// <returns>If this object is currently grabbed.</returns>
    public bool IsGrabbed()
    {
        return mGrabbed;
    }

    /// <summary>
    /// Returns user grabber currently grabbing this object, if there is one.
    /// </summary>
    /// <returns>User grabber grabbing this object.</returns>
    public HashSet<Grabber> GrabbedBy()
    {
        return mGrabbedBy;
    }

    /// <summary>
    /// Decides if this minion is currently selected by the user.
    /// </summary>
    /// <returns>Whether this minion is currently selected.</returns>
    public bool IsSelected()
    {
        return mSelectedBy.Count > 0;
    }

    /// <summary>
    /// Returns game object instance representing this minion.
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    /// <summary>
    /// Finds the closest minion to this minion in given collection.
    /// </summary>
    /// <param name="otherMinions">Collection of other minions.</param>
    /// <returns></returns>
    public MinionController FindClosestOtherMinion(HashSet<MinionController> otherMinions)
    {
        MinionController closest = null;

        if(otherMinions.Count > 0)
        {
            float closestDist = float.MaxValue;

            foreach(MinionController minion in otherMinions)
            {
                float dist = Vector3.Distance(minion.transform.position, this.transform.position);
                if(dist < closestDist)
                {
                    closest = minion;
                    closestDist = dist;
                }
            }
        }

        return closest;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    private void OnDestroy()
    {
        mEnemeyDetector.mTriggerEnterEvent += EnemeyDetectorEnter;
        mEnemeyDetector.mTriggerExitEvent += EnemeyDetectorExit;
    }
}
