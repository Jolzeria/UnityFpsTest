using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{

    private void Awake()
    {
        EventHandler.Init();
        ParabolaCurveManager.Init();
        BulletPool.Init(transform.Find("Pool"));
        DamageManager.Init();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        ParabolaCurveManager.UnInit();
        BulletPool.UnInit();
        DamageManager.UnInit();
        EventHandler.UnInit();
    }

    private void Update()
    {
        ParabolaCurveManager.Update();
        DamageManager.Update();
    }

    private void FixedUpdate()
    {
        ParabolaCurveManager.FixedUpdate();
    }
}
