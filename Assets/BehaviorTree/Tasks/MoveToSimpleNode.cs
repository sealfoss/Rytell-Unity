using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveToSimple : MoveTo
{
    public override bool Run()
    {
        Vector3 target = GetTarget();
        GameObject obj = mTree.gameObject;
        Vector3 current = obj.transform.position;
        float speed = GetSpeed() * Time.deltaTime;
        Vector3 newPosition = Vector3.Lerp(current, target, speed);
        obj.transform.position = newPosition;
        return !CheckIfWithinThreshold();
    }
}
