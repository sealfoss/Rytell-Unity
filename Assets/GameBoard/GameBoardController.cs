using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    [SerializeField]
    private GameObject mTilePrefab;

    [SerializeField]
    private int mTilesWide;

    [SerializeField]
    private int mTilesDeep;

    [SerializeField]
    private float mTileSize;

    [SerializeField]
    private float mColliderWIdth;

    [SerializeField]
    private float mColliderDepth;

    [SerializeField]
    private int mCollisionLayer;

    float mBoardWidth;

    float mBoardDepth;

    BoxCollider mCollider;

    private void Awake()
    {
        mBoardWidth = mTileSize * mTilesWide;
        mBoardDepth = mTileSize * mTilesDeep;
        InitTiles();
        InitBoardCollider();
    }

    private void InitBoardCollider()
    {
        mCollider = this.gameObject.AddComponent<BoxCollider>();
        mCollider.size = new Vector3(mColliderWIdth, mTileSize, mColliderDepth);
        mCollider.isTrigger = false;
        this.gameObject.layer = mCollisionLayer;
    }

    private void InitTiles()
    {
        float y = this.transform.position.y;
        float startX = -(mBoardWidth / 2.0f);
        float startZ = -(mBoardDepth / 2.0f);
        float tileSizeHalf = mTileSize / 2.0f;
        int tileCount = 0;

        // Front to back.
        for(int i = 0; i < mTilesDeep; i++)
        {
            float z = startZ + (mTileSize * i) + tileSizeHalf;

            // Left to right.
            for(int j = 0; j < mTilesWide; j++)
            {
                float x = startX + (mTileSize * j) + tileSizeHalf;
                Vector3 localPosition = new Vector3(x, y, z);
                GameObject tile = Instantiate(mTilePrefab);
                tile.transform.SetParent(this.transform);
                tile.transform.localPosition = localPosition;
                string tileName = $"Tile{tileCount: 0000}";
                tile.name = tileName;
                tileCount++;
                tile.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
