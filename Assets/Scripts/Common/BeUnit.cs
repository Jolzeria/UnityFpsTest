using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeUnit : MonoBehaviour
{
    protected BaseAttribute attribute;
    public BeUnit OriginalCreator { get; set; }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        //UnInit();
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
        Debug.Log("player ATK:" + damageInfo.snapShot.GetAttrValue(AttributeType.ATK));
        Debug.Log("receiver DEF:" + damageInfo.receiver.GetAttrValue(AttributeType.DEF));
        float addHp = damageInfo.receiver.GetAttrValue(AttributeType.DEF) -
                      damageInfo.snapShot.GetAttrValue(AttributeType.ATK);
        if (addHp > 0)
            addHp = 0f;

        float curHp = damageInfo.receiver.GetAttrValue(AttributeType.CurHp);
        if (curHp <= 0)
        {
            Debug.Log("敌人已死亡");
            return;
        }

        if (Mathf.Abs(addHp) >= curHp)
        {
            addHp = -curHp;
            Debug.Log("击败敌人");
        }

        damageInfo.receiver.AddAttrValue(AttributeType.CurHp, addHp);
        Debug.Log("receiver CurHp:" + damageInfo.receiver.GetAttrValue(AttributeType.CurHp));

        DamageTextManager.Instance.Add(new DamageTextManager.DamageTextData()
            {position = damageInfo.hitPoint, damage = Mathf.RoundToInt(addHp)});
    }

    public void AddAttrValue(AttributeType attr, float value)
    {
        attribute.AddAttrValue(attr, value);
    }

    public float GetAttrValue(AttributeType attr)
    {
        return attribute.GetAttrValue(attr);
    }
}