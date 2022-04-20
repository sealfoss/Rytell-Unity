using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive<T>
{
    public void Grab(T grabbing);
    public void Release(T releasing);
    public void Activate(T activating);
    public void Deactivate(T deactivating);
    public void Select(T selecting);
    public void Deselect(T deselecting);
    public bool IsGrabbed();
    public HashSet<T> GrabbedBy();
    public GameObject GetGameObject();
}
