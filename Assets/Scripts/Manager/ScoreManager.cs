using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private Transform m_ScoreTransform;
    private TMP_Text m_Score1Text;
    private int score1Count;
    private TMP_Text m_Score2Text;
    private int score2Count;
    private TMP_Text m_Score3Text;
    private int score3Count;

    public override void Init()
    {
        base.Init();

        m_ScoreTransform = InstanceManager.Instance.Get(InstanceType.Score);
        m_Score1Text = m_ScoreTransform.Find("score1").GetComponent<TMP_Text>();
        m_Score2Text = m_ScoreTransform.Find("score2").GetComponent<TMP_Text>();
        m_Score3Text = m_ScoreTransform.Find("score3").GetComponent<TMP_Text>();
        
        score1Count = 0;
        score2Count = 0;
        score3Count = 0;
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public void ResetScore()
    {
        score1Count = 0;
        m_Score1Text.text = score1Count.ToString();
        score2Count = 0;
        m_Score2Text.text = score2Count.ToString();
        score3Count = 0;
        m_Score3Text.text = score3Count.ToString();
    }

    public void Score1Add(int count)
    {
        if (score1Count + count < 0)
        {
            return;
        }

        score1Count += count;
        m_Score1Text.text = score1Count.ToString();
    }
    
    public void Score2Add(int count)
    {
        if (score2Count + count < 0)
        {
            return;
        }

        score2Count += count;
        m_Score2Text.text = score2Count.ToString();
    }
    
    public void Score3Add(int count)
    {
        if (score3Count + count < 0)
        {
            return;
        }

        score3Count += count;
        m_Score3Text.text = score3Count.ToString();
    }

    public int GetScore()
    {
        return score3Count;
    }
}