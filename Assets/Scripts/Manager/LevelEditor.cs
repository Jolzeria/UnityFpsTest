using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    [Tooltip("游戏开始后多少秒生成该靶子")]
    public float spawnTime;
    [Tooltip("靶子存活的时间")]
    public float lifeTime = 5f;
    [Tooltip("靶子生成的位置")]
    public Vector3 spawnPosition;
    [Tooltip("靶子初始移动方向")]
    public MoveDirection moveDirection;
    [Tooltip("靶子移动方式")]
    public MoveType moveType;
    [Tooltip("移动速度")]
    public SpeedLevel speedLevel;
    [Tooltip("拖入 GameObject 自动记录位置")]
    public Transform target;
}

public class LevelEditor : MonoBehaviour
{
    // [Header("靶子生成列表")]
    public List<SpawnData> spawnDatas;
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
    }

    public List<SpawnData> GetSpawnDatas()
    {
        return spawnDatas.ToList();
    }
}
