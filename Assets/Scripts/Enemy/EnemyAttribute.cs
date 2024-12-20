
public class EnemyAttribute : BaseAttribute
{
    protected override float GetBaseValue(AttributeType attr)
    {
        switch (attr)
        {
            case AttributeType.CurHp:
                return 100;
            case AttributeType.MaxHp:
                return 100;
            case AttributeType.ATK:
                return 1;
            case AttributeType.DEF:
                return 5;
            default:
                return 0;
        }
    }
}
