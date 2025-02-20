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

    // 帮助文字对象
    private Transform helpTrans;

    // 游戏总时长
    private float gameRunTimer;
    
    // 显示游戏剩余时间
    private Transform remainTime;

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

        helpTrans = InstanceManager.Instance.Get(InstanceType.TwoDCanvas).Find("HelpText");
        
        remainTime = InstanceManager.Instance.Get(InstanceType.TwoDCanvas).Find("RemainTime");
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public void Update()
    {
        // 生成靶子
        if (gameStatus == 1 && Time.time - lastCreateTime > 1)
        {
            lastCreateTime = Time.time;

            var randomNum = Random.Range(1, 4);
            var moveType = randomNum == 1 ? MoveType.MoveX : randomNum == 2 ? MoveType.MoveZ : MoveType.MoveXHalfRound;
            var speedLevel = level == 1 ? SpeedLevel.Level1 : level == 2 ? SpeedLevel.Level2 : SpeedLevel.Level3;
            var score = moveType == MoveType.MoveZ ? 1 : moveType == MoveType.MoveX ? 2 : 3;

            TargetSpawnManager.Instance.Add(moveType, speedLevel, score);
        }

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

        // 判断游戏结束的逻辑
        if (gameStatus == 1)
        {
            gameRunTimer += Time.deltaTime;
            var countDown = Mathf.FloorToInt(31f - gameRunTimer);
            remainTime.GetComponent<TMP_Text>().text = countDown.ToString();

            if (gameRunTimer >= 30f)
            {
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        if (gameStatus == 1) return;

        gameStatus = 1;
        lastCreateTime = 0f;

        ShowCountdownText($"游戏开始");
        ShowScoreText();
    }

    public void StopGame()
    {
        if (gameStatus != 1) return;

        gameStatus = 2;
        TargetSpawnManager.Instance.Reset();

        ShowCountdownText($"暂停游戏");
    }

    public void LevelUp()
    {
        if (level >= 3 || gameStatus != 0) return;
        level += 1;

        ShowCountdownText($"当前难度：{level}");
    }

    public void LevelDown()
    {
        if (level <= 1 || gameStatus != 0) return;
        level -= 1;

        ShowCountdownText($"当前难度：{level}");
    }

    public void ResetGame()
    {
        gameStatus = 0;
        ScoreManager.Instance.ResetScore();
        TargetSpawnManager.Instance.Reset();
        ShowCountdownText($"重置游戏");
        ShowHelpText();
    }

    public void GameOver()
    {
        gameoverInfoText.text = $"游戏结束\n得分：{ScoreManager.Instance.GetScore()}";
        ShowGameoverText();

        gameStatus = 0;
        ScoreManager.Instance.ResetScore();
        TargetSpawnManager.Instance.Reset();

        gameRunTimer = 0f;
    }

    private void ShowHelpText()
    {
        helpTrans.gameObject.SetActive(true);
        scoreTranform.gameObject.SetActive(false);
        remainTime.gameObject.SetActive(false);
        gameoverInfo.gameObject.SetActive(false);
    }

    private void ShowScoreText()
    {
        helpTrans.gameObject.SetActive(false);
        scoreTranform.gameObject.SetActive(true);
        remainTime.gameObject.SetActive(true);
        gameoverInfo.gameObject.SetActive(false);
    }

    private void ShowGameoverText()
    {
        helpTrans.gameObject.SetActive(false);
        scoreTranform.gameObject.SetActive(false);
        remainTime.gameObject.SetActive(false);
        gameoverInfo.gameObject.SetActive(true);
    }

    // 显示有时间限制的提示信息
    private void ShowCountdownText(string text)
    {
        levelInfo.gameObject.SetActive(true);
        levelInfoShowStatus = true;
        levelInfoText.text = text;
        levelInfoShowTime = 0f;
    }
}