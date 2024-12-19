using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageManager
{
    private static Queue<DamageInfo> damageInfos;

    public static void Init()
    {
        damageInfos = new Queue<DamageInfo>();
    }

    public static void UnInit()
    {
        damageInfos.Clear();
        damageInfos = null;
    }

    public static void Update()
    {
        while (damageInfos.Count > 0)
        {
            var damageInfo = damageInfos.Dequeue();
            EventHandler.ExecuteEvent(damageInfo.receiver, GameEventEnum.DamageProcess, damageInfo);
        }
    }

    public static void Add(DamageInfo damageInfo)
    {
        if (damageInfo == null)
            return;
        damageInfos.Enqueue(damageInfo);
    }

    public static void Remove()
    {

    }
}
