using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{

    private void Awake()
    {
    }

    private void Start()
    {
        BulletManager.Init();
        BulletPool.Init(transform.Find("Pool"));
    }
    private void OnDestroy()
    {
        BulletManager.UnInit();
        BulletPool.UnInit();
    }

    private void Update()
    {
        BulletManager.Update();
    }

    private void FixedUpdate()
    {
        BulletManager.FixedUpdate();
    }
}
