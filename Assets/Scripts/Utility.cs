using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

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
