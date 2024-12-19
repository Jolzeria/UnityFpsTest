using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{

    private void Awake()
    {
        EventHandler.Init();
        BulletManager.Init();
        BulletPool.Init(transform.Find("Pool"));
        DamageManager.Init();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        EventHandler.UnInit();
        BulletManager.UnInit();
        BulletPool.UnInit();
        DamageManager.UnInit();
    }

    private void Update()
    {
        BulletManager.Update();
        DamageManager.Update();
    }

    private void FixedUpdate()
    {
        BulletManager.FixedUpdate();
    }
}
