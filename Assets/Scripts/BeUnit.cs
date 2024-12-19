using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeUnit : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        UnInit();
    }

    protected virtual void Init()
    {
        RegisterEvent();
    }

    protected virtual void UnInit()
    {
        UnRegisterEvent();
    }

    protected virtual void RegisterEvent()
    {
        EventHandler.RegisterEvent<DamageInfo>(this, GameEventEnum.DamageProcess, DamageProcess);
    }

    protected virtual void UnRegisterEvent()
    {
        EventHandler.UnRegisterEvent<DamageInfo>(this, GameEventEnum.DamageProcess, DamageProcess);
    }

    private void DamageProcess(DamageInfo damageInfo)
    {
        Debug.Log("damage:" + damageInfo.damage);
    }
}
