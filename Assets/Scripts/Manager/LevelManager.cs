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
    private Transform scoreTranform; // Score文字

    // 显示全局信息相关
    private Transform topShow;
    private TMP_Text topShowText;
    private bool topShowStatus;
    private float topShowLastTime;

    // 游戏结束显示得分相关
    private Transform gameoverInfo;
    private TMP_Text gameoverInfoText;

    // 游戏总时长
    private float gameRunTimer;
    
    // 显示游戏剩余时间
    private Transform remainTime;
    
    // 所有的关卡信息
    private Transform levelListTrans;
    private Transform level1Trans;
    private List<SpawnData> level1Datas;
    private Transform level2Trans;
    private List<SpawnData> level2Datas;
    private Transform level3Trans;
    private List<SpawnData> level3Datas;

    public override void Init()
    {
        base.Init();

        level = 1;
        gameStatus = 0;
        gameRunTimer = 0f;
        scoreTranform = InstanceManager.Instance.Get(InstanceType.Score);

        topShow = InstanceManager.Instance.Get(InstanceType.TopShowText);
        topShowText = topShow.GetComponent<TMP_Text>();
        topShowStatus = false;
        topShowLastTime = 0f;

        gameoverInfo = InstanceManager.Instance.Get(InstanceType.GameoverInfo);
        gameoverInfoText = gameoverInfo.GetComponent<TMP_Text>();
        
        remainTime = InstanceManager.Instance.Get(InstanceType.TwoDCanvas).Find("RemainTime");
        
        levelListTrans = InstanceManager.Instance.Get(InstanceType.LevelList);
        level1Trans = levelListTrans.Find("Level1");
        ResetLevelDatas();
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public void Update()
    {
        // 控制全局信息显示
        if (topShowStatus)
        {
            if (Time.unscaledTime - topShowLastTime > 3f)
            {
                topShowStatus = false;
                topShow.gameObject.SetActive(false);
            }
        }

        // 判断游戏结束的逻辑以及生成靶子
        if (gameStatus == 1)
        {
            gameRunTimer += Time.deltaTime;
            CreateTarget();
            var countDown = Mathf.FloorToInt(31f - gameRunTimer);
            remainTime.GetComponent<TMP_Text>().text = countDown.ToString();

            if (gameRunTimer >= 30f)
            {
                GameOver();
            }
        }
    }

    private void CreateTarget()
    {
        for (int i = level1Datas.Count - 1; i >= 0; i--)
        {
            if (level1Datas[i].spawnTime <= gameRunTimer)
            {
                var data = level1Datas[i];
                var position = data.spawnPosition;
                var moveType = data.moveType;
                var moveDirection = data.moveDirection;
                var speedLevel = data.speedLevel;
                
                // 计算分数
                var score = 0;
                if (moveType == MoveType.MoveStraight)
                    score += 1;
                if (moveType == MoveType.MoveXHalfRound)
                    score += 2;
                if (moveDirection == MoveDirection.forward || moveDirection == MoveDirection.back)
                    score += 1;
                if (moveDirection == MoveDirection.right || moveDirection == MoveDirection.left)
                    score += 2;
                if (speedLevel == SpeedLevel.Level1)
                    score += 1;
                if (speedLevel == SpeedLevel.Level2)
                    score += 2;
                if (speedLevel == SpeedLevel.Level3)
                    score += 3;

                TargetSpawnManager.Instance.Add(position, moveType, moveDirection, speedLevel, score);
                level1Datas.Remove(level1Datas[i]);
            }
        }
    }

    private void ResetLevelDatas()
    {
        level1Datas = level1Trans.GetComponent<LevelEditor>().GetSpawnDatas();
    }

    public void StartGame()
    {
        if (gameStatus == 1) return;

        gameStatus = 1;

        ShowCountdownText($"游戏开始");
        ShowScoreText();
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
        ShowInitText();
        
        gameRunTimer = 0f;
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

    private void ShowInitText()
    {
        scoreTranform.gameObject.SetActive(false);
        remainTime.gameObject.SetActive(false);
        gameoverInfo.gameObject.SetActive(false);
    }

    private void ShowScoreText()
    {
        scoreTranform.gameObject.SetActive(true);
        remainTime.gameObject.SetActive(true);
        gameoverInfo.gameObject.SetActive(false);
    }

    private void ShowGameoverText()
    {
        scoreTranform.gameObject.SetActive(false);
        remainTime.gameObject.SetActive(false);
        gameoverInfo.gameObject.SetActive(true);
    }

    // 显示有时间限制的提示信息
    private void ShowCountdownText(string text)
    {
        topShowLastTime = Time.unscaledTime;
        topShow.gameObject.SetActive(true);
        topShowStatus = true;
        topShowText.text = text;
    }
}