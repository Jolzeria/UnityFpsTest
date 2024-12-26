using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrame : MonoBehaviour
{

    private void Awake()
    {
        EventHandler.Init();
        BulletPool.Instance.SetParent(transform.Find("Pool"));
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        ParabolaCurveManager.Instance.UnInit();
        BulletPool.Instance.UnInit();
        DamageManager.Instance.UnInit();
        EventHandler.UnInit();
    }

    private void Update()
    {
        ParabolaCurveManager.Instance.Update();
        DamageManager.Instance.Update();
    }

    private void FixedUpdate()
    {
        ParabolaCurveManager.Instance.FixedUpdate();
    }
}
