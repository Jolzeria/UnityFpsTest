using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{

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
        
        FindInstance();
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
        InstanceManager.Instance.UnInit();
        
        EventHandler.UnInit();
    }

    private void Update()
    {
        ParabolaCurveManager.Instance.Update();
        DamageManager.Instance.Update();
        DamageTextManager.Instance.Update();
    }

    private void FixedUpdate()
    {
        ParabolaCurveManager.Instance.FixedUpdate();
    }

    private void FindInstance()
    {
        InstanceManager.Instance.Add(InstanceType.Sight, transform.Find("2DCanvas/sight"));
        InstanceManager.Instance.Add(InstanceType.Scope, transform.Find("2DCanvas/Scope"));
    }
}
