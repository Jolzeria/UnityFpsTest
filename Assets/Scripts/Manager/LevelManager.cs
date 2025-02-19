using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    private int level;
    private int gameStatus; // 游戏状态 0：停止 1：开始
    private float lastCreateTime; // 上次创建靶子的时间
    private Transform scoreTranform; // Score文字
    // 显示难度信息相关
    private Transform levelInfo;
    private TMP_Text levelInfoText;
    private bool levelInfoShowStatus;
    private float levelInfoShowTime;
    // 游戏结束显示得分相关
    private Transform gameoverInfo;
    private TMP_Text gameoverInfoText;
    private bool gameoverInfoShowStatus;
    // 游戏总时长
    private float gameRunTime;

    public override void Init()
    {
        base.Init();

        level = 1;
        gameStatus = 0;
        lastCreateTime = 0f;
        scoreTranform = InstanceManager.Instance.Get(InstanceType.Score);
        
        levelInfo = InstanceManager.Instance.Get(InstanceType.LevelInfo);
        levelInfoText = levelInfo.GetComponent<TMP_Text>();
        levelInfoShowStatus = false;
        levelInfoShowTime = 0f;

        gameoverInfo = InstanceManager.Instance.Get(InstanceType.GameoverInfo);
        gameoverInfoText = gameoverInfo.GetComponent<TMP_Text>();
        gameoverInfoShowStatus = false;
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public void Update()
    {
        // 控制难度信息等显示
        if (levelInfoShowStatus)
        {
            levelInfoShowTime += Time.deltaTime;

            if (levelInfoShowTime > 3f)
            {
                levelInfoShowStatus = false;
                levelInfo.gameObject.SetActive(false);
                levelInfoShowTime = 0f;
            }
        }
        
        // 当游戏状态不是0的时候隐藏游戏结束信息
        if (gameStatus != 0)
        {
            gameoverInfo.gameObject.SetActive(false);
        }

        // 控制生成靶子
        if (gameStatus == 1 && Time.time - lastCreateTime > 1)
        {
            lastCreateTime = Time.time;

            var randomNum = Random.Range(1, 4);
            var moveType = randomNum == 1 ? MoveType.MoveX : randomNum == 2 ? MoveType.MoveZ : MoveType.MoveXHalfRound;
            var speedLevel = level == 1 ? SpeedLevel.Level1 : level == 2 ? SpeedLevel.Level2 : SpeedLevel.Level3;

            TargetSpawnManager.Instance.Add(moveType, speedLevel);
        }
        
        // 计算游戏总运行时间
        if (gameStatus == 1)
        {
            gameRunTime += Time.deltaTime;

            if (gameRunTime >= 30f)
            {
                GameOver();
                ResetGame();
            }
        }
    }

    public void StartGame()
    {
        if (gameStatus == 1) return;

        gameStatus = 1;
        lastCreateTime = 0f;

        levelInfo.gameObject.SetActive(true);
        levelInfoShowStatus = true;
        levelInfoText.text = "游戏开始！";
        
        scoreTranform.gameObject.SetActive(true);
    }

    public void StopGame()
    {
        gameStatus = 2;
        TargetSpawnManager.Instance.Reset();
        
        levelInfo.gameObject.SetActive(true);
        levelInfoShowStatus = true;
        levelInfoText.text = "暂停游戏";
    }

    public void LevelUp()
    {
        if (level >= 3 || gameStatus == 1) return;
        level += 1;

        levelInfo.gameObject.SetActive(true);
        levelInfoShowStatus = true;
        levelInfoText.text = "当前难度：" + level;
    }

    public void LevelDown()
    {
        if (level <= 1 || gameStatus == 1) return;
        level -= 1;

        levelInfo.gameObject.SetActive(true);
        levelInfoShowStatus = true;
        levelInfoText.text = "当前难度：" + level;
    }

    public void ResetGame()
    {
        gameStatus = 0;
        ScoreManager.Instance.ResetScore();
        TargetSpawnManager.Instance.Reset();
    }

    public void GameOver()
    {
        scoreTranform.gameObject.SetActive(false);
        gameoverInfo.gameObject.SetActive(true);
        gameoverInfoText.text = $"游戏结束！\n得分：{ScoreManager.Instance.GetScore()}";
    }
}