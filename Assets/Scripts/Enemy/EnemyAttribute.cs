
public class EnemyAttribute : BaseAttribute
{
    protected override float GetBaseValue(AttributeType attr)
    {
        switch (attr)
        {
            case AttributeType.CurHp:
                return 30;
            case AttributeType.MaxHp:
                return 30;
            case AttributeType.ATK:
                return 1;
            case AttributeType.DEF:
                return 0;
            default:
                return 0;
        }
    }
}
