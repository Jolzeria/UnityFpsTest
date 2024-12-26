using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DamageManager : Singleton<DamageManager>
{
    private Queue<DamageInfo> damageInfos;

    public override void Init()
    {
        damageInfos = new Queue<DamageInfo>();
    }

    public override void UnInit()
    {
        damageInfos.Clear();
        damageInfos = null;
    }

    public void Update()
    {
        while (damageInfos.Count > 0)
        {
            var damageInfo = damageInfos.Dequeue();
            EventHandler.ExecuteEvent(damageInfo.receiver, GameEventEnum.DamageProcess, damageInfo);
        }
    }

    public void Add(DamageInfo damageInfo)
    {
        if (damageInfo == null)
            return;
        damageInfos.Enqueue(damageInfo);
    }

    public void Remove()
    {
    }
}