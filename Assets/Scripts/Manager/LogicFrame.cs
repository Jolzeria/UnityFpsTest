using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{
    public static bool isInit = false;

    private void Awake()
    {
        EventHandler.Init();
        
        BulletPool.Instance.SetParent(transform.Find("BulletPool"));
        RoundBulletPool.Instance.SetParent(transform.Find("RoundBulletPool"));
        DamageTextPool.Instance.SetParent(transform.Find("DamageTextPool"));
        BulletMarksPool.Instance.SetParent(transform.Find("BulletMarksPool"));
        GunPool.Instance.SetParent(transform.Find("GunPool"));
        DamageTextManager.Instance.SetCanvas(transform.Find("DamageCanvas"));
        CharacterManager.Instance.Init();
        TargetSpawnManager.Instance.SetParent(GameObject.Find("TrainCenter/EnemySpawn").transform);
        
        FindInstance();

        isInit = true;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        ParabolaCurveManager.Instance.UnInit();
        BulletPool.Instance.UnInit();
        RoundBulletPool.Instance.UnInit();
        DamageTextPool.Instance.UnInit();
        BulletMarksPool.Instance.UnInit();
        DamageManager.Instance.UnInit();
        DamageTextManager.Instance.UnInit();
        CharacterManager.Instance.UnInit();
        GunPool.Instance.UnInit();
        ScoreManager.Instance.UnInit();
        TargetSpawnManager.Instance.UnInit();
        InstanceManager.Instance.UnInit();
        
        EventHandler.UnInit();
    }

    private void Update()
    {
        ParabolaCurveManager.Instance.Update();
        DamageManager.Instance.Update();
        DamageTextManager.Instance.Update();
        LevelManager.Instance.Update();
    }

    private void FixedUpdate()
    {
        ParabolaCurveManager.Instance.FixedUpdate();
    }

    private void FindInstance()
    {
        InstanceManager.Instance.Add(InstanceType.TwoDCanvas, transform.Find("2DCanvas"));
        InstanceManager.Instance.Add(InstanceType.Sight, transform.Find("2DCanvas/sight"));
        InstanceManager.Instance.Add(InstanceType.Scope, transform.Find("2DCanvas/Scope"));
        InstanceManager.Instance.Add(InstanceType.Score, transform.Find("2DCanvas/Score"));
        InstanceManager.Instance.Add(InstanceType.LevelInfo, transform.Find("2DCanvas/LevelInfo"));
        InstanceManager.Instance.Add(InstanceType.GameoverInfo, transform.Find("2DCanvas/GameoverInfo"));
        InstanceManager.Instance.Add(InstanceType.LevelList, GameObject.Find("LevelList").transform);
    }
}
