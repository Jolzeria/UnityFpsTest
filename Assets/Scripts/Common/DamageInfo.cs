using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public float damage;
    public SnapShot snapShot;
    public BeUnit receiver;
    public Vector3 hitPoint;
}

public class SnapShot
{
    private Dictionary<AttributeType, float> m_attr;
    public BeUnit attacker;
    private AttributeType[] needInit = new AttributeType[] {
        AttributeType.CurHp,
        AttributeType.MaxHp,
        AttributeType.ATK,
        AttributeType.DEF
    };

    public SnapShot(BeUnit beUnit)
    {
        attacker = beUnit;

        m_attr = new Dictionary<AttributeType, float>();

        for (var i = 0; i < needInit.Length; i++)
        {
            var attr = needInit[i];

            m_attr[attr] = beUnit.GetAttrValue(attr);
        }
    }

    public float GetAttrValue(AttributeType attr)
    {
        if (m_attr.TryGetValue(attr, out var result))
            return result;

        Debug.LogError("拿不到值" + attr);
        return 0;
    }
}
