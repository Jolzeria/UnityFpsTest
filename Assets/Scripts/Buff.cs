using System;

public static class Buff
{
    [Flags]
    public enum BuffFlag
    {
        None = 0,
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2,
        D = 1 << 3,
        E = 1 << 4
    }

    public static void AddFlag(this BuffFlag fg, BuffFlag flag)
    {
        fg |= flag;
    }

    public static bool HasFlag(this BuffFlag fg, BuffFlag flag)
    {
        return (fg & flag) != 0;
    }

    public static void RemoveFlag(this BuffFlag fg, BuffFlag flag)
    {
        fg &= ~flag;
    }
}
