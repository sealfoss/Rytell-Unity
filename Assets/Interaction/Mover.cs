using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public enum MovementType
    {
        PHYS,
        LERP,
        PARENT
    }

    public static Mover GetMoverFromType(MovementType type, GameObject gameObject)
    {
        Mover mover = null;

        switch(type)
        {
            case MovementType.PHYS:
                break;
            case MovementType.LERP:
                break;
            case MovementType.PARENT:
                break;
        }

        return mover;
    }

    public abstract void MoveObject(IInteractive<Grabber> toMove, Grabber movedBy, Transform target);
}
