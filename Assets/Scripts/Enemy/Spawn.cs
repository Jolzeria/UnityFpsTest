using System;
using UnityEngine;

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
        CreateEnemy(MoveType.MoveXHalfRound);
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

        obj.transform.position = new Vector3(transform.position.x - transform.localScale.x / 2, 0.12f,
            transform.position.z + 0.5f);
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
}