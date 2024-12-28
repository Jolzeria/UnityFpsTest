using System;
using UnityEngine;

public class Direction : MonoBehaviour
{
    [SerializeReference]
    [SubclassSelector]
    public BaseClass dir;

    [Serializable]
    public abstract class BaseClass
    {
        
    }

    [Serializable]
    public class Type1 : BaseClass
    {
        public Vector3 point;
        public float x = 1f;
        public float y = 1f;
        public float z = 1f;
        
    }

    [Serializable]
    public class Type2 : BaseClass
    {
        public string name;
        public int id;
    }
}