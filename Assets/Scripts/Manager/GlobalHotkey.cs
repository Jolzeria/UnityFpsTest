using System;
using UnityEngine;

public class GlobalHotkey : MonoBehaviour
{
    private void Update()
    {
        // 开始游戏
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LevelManager.Instance.StartGame();
        }

        // 降低难度
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LevelManager.Instance.LevelDown();
        }

        // 提升难度
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LevelManager.Instance.LevelUp();
        }

        // 重置游戏
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LevelManager.Instance.ResetGame();
        }
    }
}