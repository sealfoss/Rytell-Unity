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

    [SerializeField]
    GameObject mMinionPrefab;

    [SerializeField]
    int mMaxMinionCount;

    HashSet<MinionController> mMinions;

    [SerializeField]
    float mSpawnHeight;

    float mStartX;

    float mStartZ;

    List<MinionController> mRemovalList;

    [SerializeField]
    float mSpawnTimer;

    float mTimeSinceLastSpawn;

    private void Awake()
    {
        mTimeSinceLastSpawn = 0;
        mRemovalList = new List<MinionController>();
        mMinions = new HashSet<MinionController>();
        mBoardWidth = mTileSize * mTilesWide;
        mBoardDepth = mTileSize * mTilesDeep;
        InitTiles();
        InitBoardCollider();
    }

    private void Update()
    {
        MonitorMinions();
    }

    private void MonitorMinions()
    {
        if(mMinions.Count < mMaxMinionCount && mTimeSinceLastSpawn > mSpawnTimer)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(mStartX, mStartX + mBoardWidth),
                mSpawnHeight,
                Random.Range(mStartZ, mStartZ + mBoardWidth));
            GameObject minionObj = Instantiate(mMinionPrefab, spawnPosition, Quaternion.identity);
            MinionController minion = minionObj.GetComponent<MinionController>();
            mMinions.Add(minion);
            mTimeSinceLastSpawn = 0;
        }
        else
        {
            mTimeSinceLastSpawn += Time.deltaTime;
        }

        // Altering collections as you iterate over them is bad.
        foreach (MinionController minion in mMinions)
        {
            if(minion.Dead)
            {
                mRemovalList.Add(minion);
            }
        }
        foreach(MinionController minion in mRemovalList)
        {
            mMinions.Remove(minion);
        }

        mRemovalList.Clear();
    }

    private void InitBoardCollider()
    {
        mCollider = this.gameObject.AddComponent<BoxCollider>();
        mCollider.size = new Vector3(mColliderWIdth, mTileSize, mColliderDepth);
        mCollider.center = new Vector3(0, 0, 0);
        mCollider.isTrigger = false;
        this.gameObject.layer = mCollisionLayer;
    }

    private void InitTiles()
    { 
        mStartX = -(mBoardWidth / 2.0f);
        mStartZ = -(mBoardDepth / 2.0f);
        float tileSizeHalf = mTileSize / 2.0f;
        float y = this.transform.position.y - mTileSize;
        int tileCount = 0;

        // Front to back.
        for(int i = 0; i < mTilesDeep; i++)
        {
            float z = mStartZ + (mTileSize * i) + tileSizeHalf;

            // Left to right.
            for(int j = 0; j < mTilesWide; j++)
            {
                float x = mStartX + (mTileSize * j) + tileSizeHalf;
                Vector3 tilePosition = new Vector3(x, y, z);
                GameObject tile = Instantiate(mTilePrefab);
                tile.transform.position = tilePosition;
                tile.transform.SetParent(this.transform);
                string tileName = $"Tile{tileCount: 0000}";
                tile.name = tileName;
                tileCount++;
                tile.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
