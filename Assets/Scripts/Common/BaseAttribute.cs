using System.Collections.Generic;

public enum AttributeType
{
    None,
    CurHp,
    MaxHp,
    CurMp,
    MaxMp,
    CurAmmo,
    MagazineSize,
    TotalAmmo,
    MaxAmmo,
    MoveSpeedRate,
    ATK,
    DEF,
    Max
}

public abstract class BaseAttribute
{
    private Dictionary<AttributeType, float> attritubes;

    public void Init()
    {
        attritubes = new Dictionary<AttributeType, float>();
        AddToDic();
    }

    public void UnInit()
    {
        attritubes.Clear();
        attritubes = null;
    }

    private void AddToDic()
    {
        for (var i = 1; i < (int)(AttributeType.Max); i++)
        {
            var attr = (AttributeType)i;
            attritubes[attr] = GetBaseValue(attr);
        }
    }

    public void AddAttrValue(AttributeType attr, float value)
    {
        attritubes[attr] += value;
    }

    public float GetAttrValue(AttributeType attr)
    {
        return attritubes[attr];
    }
    
    public void SetAttrValue(AttributeType attr, float value)
    {
        attritubes[attr] = value;
    }

    protected abstract float GetBaseValue(AttributeType attr);
}
