using System;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum MyLayer
{
    Default = 1 << 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water = 1 << 4,
    UI = 1 << 5,
    Environment = 1 << 6,
    Enemy = 1 << 7,
    Player = 1 << 8,
    Weapon = 1 << 9,
    Boundary = 1 << 10
}

public class LayerManager
{
    public const int Default = (int)MyLayer.Default;
    public const int TransparentFX = (int)MyLayer.TransparentFX;
    public const int IgnoreRaycast = (int)MyLayer.IgnoreRaycast;
    public const int Water = (int)MyLayer.Water;
    public const int UI = (int)MyLayer.UI;
    public const int Environment = (int)MyLayer.Environment;
    public const int Enemy = (int)MyLayer.Enemy;
    public const int Player = (int)MyLayer.Player;
    public const int Weapon = (int)MyLayer.Weapon;
    public const int Boundary = (int)MyLayer.Boundary;
}

public class Layer
{
    public const int Default = 0;
    public const int TransparentFX = 1;
    public const int IgnoreRaycast = 2;
    public const int Water = 4;
    public const int UI = 5;
    public const int Environment = 6;
    public const int Enemy = 7;
    public const int Player = 8;
    public const int Weapon = 9;
    public const int Boundary = 10;
}

public static class Utility
{
    public static Vector3 WithY(this Vector3 vector, float value)
    {
        return new Vector3(vector.x, value, vector.z);
    }

    public static T SafeAddComponent<T>(this Transform trans) where T : Component
    {
        var com = trans.GetComponent<T>();

        return com != null ? com : trans.AddComponent<T>();
    }
}
