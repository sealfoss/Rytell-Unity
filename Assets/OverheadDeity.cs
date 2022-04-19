using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadDeity : DeityController
{
    [SerializeField]
    protected GameObject mHandPrefab;
    [SerializeField]
    private float mHandHeight = 1.0f;
    [SerializeField]
    private float mSearchDist = 100.0f;
    [SerializeField]
    private int mGameBoardLayerMask;
    protected override void Awake()
    {
        base.Awake();
        mGrabbers[0] = this.gameObject.AddComponent<ScreenGrabber>();
        mGrabbers[0].Init(Mover.MovementType.PHYS, mHandPrefab);
        mGameBoardLayerMask = 1 << 8;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(
            ray.origin,
            ray.direction,
            out hit,
            mSearchDist,
            mGameBoardLayerMask))
        {
            Vector3 handPos = hit.point + new Vector3(0, mHandHeight, 0);
            mGrabbers[0].MoveHand(handPos, Quaternion.identity);
        }
    }
}
