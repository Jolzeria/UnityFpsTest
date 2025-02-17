using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    private int level;
    private Transform gameInfo = null;
    private bool gameInfoShowStatus;
    private float gameInfoShowTime;
    private int gameStatus; // 游戏状态 0：停止 1：开始
    private float lastAddTime; // 上次创建靶子的时间

    public override void Init()
    {
        base.Init();

        level = 1;
        gameInfoShowStatus = false;
        gameInfoShowTime = 0f;
        gameStatus = 0;
        lastAddTime = 0f;
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public void Update()
    {
        // 控制信息显示时间
        if (gameInfoShowStatus)
        {
            gameInfoShowTime += Time.deltaTime;

            if (gameInfoShowTime > 3f)
            {
                gameInfoShowStatus = false;
                gameInfo.gameObject.SetActive(false);
                gameInfoShowTime = 0f;
            }
        }

        if (gameStatus == 1 && Time.time - lastAddTime > 2)
        {
            lastAddTime = Time.time;

            var randomNum = Random.Range(1, 4);
            var moveType = randomNum == 1 ? MoveType.MoveX : randomNum == 2 ? MoveType.MoveZ : MoveType.MoveXHalfRound;
            var speedLevel = level == 1 ? SpeedLevel.Level1 : level == 2 ? SpeedLevel.Level2 : SpeedLevel.Level3;

            TargetSpawnManager.Instance.Add(moveType, speedLevel);
        }
    }

    public void StartGame()
    {
        if (gameStatus == 1) return;

        gameStatus = 1;
        lastAddTime = 0f;

        gameInfo = InstanceManager.Instance.Get(InstanceType.GameInfo);
        gameInfo.gameObject.SetActive(true);
        gameInfoShowStatus = true;
        var text = gameInfo.GetComponent<TMP_Text>();
        text.text = "游戏开始！";
    }

    public void StopGame()
    {
        gameStatus = 0;
        TargetSpawnManager.Instance.Reset();
    }

    public void LevelUp()
    {
        if (level >= 3) return;
        level += 1;

        gameInfo = InstanceManager.Instance.Get(InstanceType.GameInfo);
        gameInfo.gameObject.SetActive(true);
        gameInfoShowStatus = true;
        var text = gameInfo.GetComponent<TMP_Text>();
        text.text = "当前难度：" + level;
    }

    public void LevelDown()
    {
        if (level <= 1) return;
        level -= 1;

        gameInfo = InstanceManager.Instance.Get(InstanceType.GameInfo);
        gameInfo.gameObject.SetActive(true);
        gameInfoShowStatus = true;
        var text = gameInfo.GetComponent<TMP_Text>();
        text.text = "当前难度：" + level;
    }

    public void ResetGame()
    {
        gameStatus = 0;
        ScoreManager.Instance.ResetScore();
        TargetSpawnManager.Instance.Reset();
    }
}