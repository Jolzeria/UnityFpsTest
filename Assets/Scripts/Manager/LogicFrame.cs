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
        DamageTextPool.Instance.SetParent(transform.Find("DamageTextPool"));
        GunPool.Instance.SetParent(transform.Find("GunPool"));
        DamageTextManager.Instance.SetCanvas(transform.Find("DamageCanvas"));
        CharacterManager.Instance.Init();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        ParabolaCurveManager.Instance.UnInit();
        BulletPool.Instance.UnInit();
        DamageTextPool.Instance.UnInit();
        DamageManager.Instance.UnInit();
        DamageTextManager.Instance.UnInit();
        CharacterManager.Instance.UnInit();
        GunPool.Instance.UnInit();
        
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
}
