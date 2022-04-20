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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            mGrabbers[0].Grab();
        if (Input.GetMouseButtonUp(0))
            mGrabbers[0].Release();
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
            Vector3 p = hit.point;
            Vector3 dir = Camera.main.transform.position - p;
            Vector3 flat = new Vector3(dir.x, 0, dir.z);
            float angle = Vector3.Angle(dir.normalized, flat.normalized) * Mathf.PI / 180.0f;
            float sin = Mathf.Sin(angle);
            float scalar = Mathf.Abs(mHandHeight / sin);
            Vector3 t = (dir.normalized * scalar) + p;
            mGrabbers[0].MoveHand(t, Quaternion.identity);
        }
    }
}
