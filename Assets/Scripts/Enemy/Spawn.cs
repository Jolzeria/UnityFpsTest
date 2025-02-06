using System;
using UnityEngine;

public enum MoveType
{
    MoveX,
    MoveZ
}

public class Spawn : MonoBehaviour
{
    private void Start()
    {
        CreateEnemy(MoveType.MoveZ);
    }

    private void CreateEnemy(MoveType moveType)
    {
        var prefab = Resources.Load<GameObject>("Enemy");
        var obj = GameObject.Instantiate(prefab);
        obj.SetActive(true);

        obj.transform.position = new Vector3(transform.position.x - transform.localScale.x / 2, 0.12f,
            transform.position.z + 0.5f);
        obj.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);

        // 添加移动脚本
        switch (moveType)
        {
            case MoveType.MoveX:
                obj.AddComponent<MoveX>();
                break;
            case MoveType.MoveZ:
                obj.AddComponent<MoveZ>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }
    }
}