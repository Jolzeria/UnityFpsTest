using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MoveType
{
    MoveX,
    MoveZ,
    MoveXHalfRound
}

public class Spawn : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            
            CreateEnemy(MoveType.MoveXHalfRound);
        }
    }

    /// <summary>
    /// 创建一个靶子
    /// </summary>
    /// <param name="moveType">移动路线类型</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void CreateEnemy(MoveType moveType)
    {
        var prefab = Resources.Load<GameObject>("Target");
        var obj = GameObject.Instantiate(prefab);
        obj.SetActive(true);

        var randomPoint = GetRandomPoint();
        obj.transform.position = randomPoint;
        obj.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        // 添加移动脚本
        switch (moveType)
        {
            case MoveType.MoveX:
                obj.AddComponent<MoveX>();
                break;
            case MoveType.MoveZ:
                obj.AddComponent<MoveZ>();
                break;
            case MoveType.MoveXHalfRound:
                obj.AddComponent<MoveXHalfRound>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }
    }

    /// <summary>
    /// 返回立方体顶部面上随机一点的坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPoint()
    {
        var cubePosition = transform.position;
        var cubeScale = transform.localScale;
        
        // 计算顶部面中心
        var topY = cubePosition.y + (cubeScale.y / 2f); // 顶部Y坐标
        var randomX = Random.Range(-cubeScale.x / 2f, cubeScale.x / 2f);
        var randomZ = Random.Range(-cubeScale.z / 2f, cubeScale.z / 2f);
        
        // 转换为世界坐标
        var randomPoint = cubePosition + new Vector3(randomX, cubeScale.y / 2f, randomZ);
        return randomPoint;
    }
}