﻿using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MoveType
{
    MoveX,
    MoveZ,
    MoveXHalfRound
}

public enum SpeedLevel
{
    Level1,
    Level2,
    Level3
}

public class TargetSpawnManager : Singleton<TargetSpawnManager>
{
    private Transform spawnRangeTrans;
    private List<GameObject> targetList;

    public override void Init()
    {
        base.Init();

        targetList = new List<GameObject>();
    }

    public override void UnInit()
    {
        base.UnInit();
        
        targetList.Clear();
        targetList = null;
    }

    public void SetParent(Transform parent)
    {
        spawnRangeTrans = parent;
    }

    public void Add(MoveType moveType, SpeedLevel speedLevel, int score)
    {
        if (targetList == null || targetList.Count >= 20)
            return;
        
        var obj = CreateEnemy(moveType, speedLevel, score);
        targetList?.Add(obj);
    }
    
    public void Release(GameObject obj)
    {
        if (targetList == null)
            return;
        
        GameObject.Destroy(obj);
        targetList?.Remove(obj);
    }

    public void Reset()
    {
        if (targetList == null)
            return;

        foreach (var obj in targetList)
        {
            GameObject.Destroy(obj);
        }
        targetList.Clear();
    }

    /// <summary>
    /// 创建一个靶子
    /// </summary>
    /// <param name="moveType">移动路线类型</param>
    /// <param name="speedLevel">移动速度</param>
    /// <param name="score">击杀获得分数</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public GameObject CreateEnemy(MoveType moveType, SpeedLevel speedLevel, int score)
    {
        var prefab = Resources.Load<GameObject>("Target");
        var obj = GameObject.Instantiate(prefab);
        obj.SetActive(true);
        
        // 设置分数
        obj.GetComponent<BeUnit>().SetAttrValue(AttributeType.Score, (float)score);

        var randomPoint = GetRandomPoint();
        obj.transform.position = randomPoint;
        obj.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        // 添加移动脚本
        BaseMove script;
        switch (moveType)
        {
            case MoveType.MoveX:
                script = obj.AddComponent<MoveX>();
                break;
            case MoveType.MoveZ:
                script = obj.AddComponent<MoveZ>();
                break;
            case MoveType.MoveXHalfRound:
                script = obj.AddComponent<MoveXHalfRound>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }
        
        // 设置移动速度
        switch (speedLevel)
        {
            case SpeedLevel.Level1:
                script.speed = 5;
                break;
            case SpeedLevel.Level2:
                script.speed = 12;
                break;
            case SpeedLevel.Level3:
                script.speed = 20;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(speedLevel), speedLevel, null);
        }

        return obj;
    }

    /// <summary>
    /// 返回立方体顶部面上随机一点的坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPoint()
    {
        var cubePosition = spawnRangeTrans.position;
        var cubeScale = spawnRangeTrans.localScale;

        // 计算顶部面中心
        var topY = cubePosition.y + (cubeScale.y / 2f); // 顶部Y坐标
        var randomX = Random.Range(-cubeScale.x / 2f, cubeScale.x / 2f);
        var randomZ = Random.Range(-cubeScale.z / 2f, cubeScale.z / 2f);

        // 转换为世界坐标
        var randomPoint = cubePosition + new Vector3(randomX, cubeScale.y / 2f, randomZ);
        return randomPoint;
    }

    public int GetTargetCount()
    {
        return targetList.Count;
    }
}