
public class CharacterAttribute : BaseAttribute
{
    protected override float GetBaseValue(AttributeType attr)
    {
        switch (attr)
        {
            case AttributeType.CurHp:
                return 100;
            case AttributeType.MaxHp:
                return 100;
            case AttributeType.CurAmmo:
                return 1;
            case AttributeType.MaxAmmo:
                return 1;
            case AttributeType.MagazineAmmo:
                return 1;
            case AttributeType.MoveSpeedRate:
                return 1;
            case AttributeType.ATK:
                return 0;
            case AttributeType.DEF:
                return 0;
            default:
                return 0;
        }
    }
}
