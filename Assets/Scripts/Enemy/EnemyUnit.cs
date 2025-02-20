using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BeUnit
{
    public int score = 0;
    
    protected override void Init()
    {
        base.Init();

        attribute = new EnemyAttribute();
        attribute.Init();
        
        attribute.SetAttrValue(AttributeType.Score, (float)score);
    }

    protected override void UnInit()
    {
        base.UnInit();

        attribute.UnInit();
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        
        EventHandler.RegisterEvent<DamageInfo>(this, GameEventEnum.DamageProcess, DamageProcess);
    }

    protected override void UnRegisterEvent()
    {
        base.UnRegisterEvent();
        
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
            
            // 击杀获得分数
            ScoreManager.Instance.Score3Add((int)damageInfo.receiver.GetAttrValue(AttributeType.Score));
            Debug.Log("击败敌人");
            
            // 自毁
            TargetSpawnManager.Instance.Release(gameObject);
        }

        // 击中获得分数
        ScoreManager.Instance.Score2Add(1);
        
        damageInfo.receiver.AddAttrValue(AttributeType.CurHp, addHp);
        Debug.Log("receiver CurHp:" + damageInfo.receiver.GetAttrValue(AttributeType.CurHp));

        DamageTextManager.Instance.Add(new DamageTextManager.DamageTextData()
            {position = damageInfo.hitPoint, damage = Mathf.RoundToInt(addHp)});
    }
}
