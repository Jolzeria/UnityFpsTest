using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParabolaCurveCreateData
{
    public int uid;
    public Vector3 startPoint;
    public Quaternion startRotation;
    public float speed;
    public Vector3 direction;
    public bool followRotate;
    public float gravity;
    public float duration;
}

public struct ParabolaCurveUpdateData
{
    public int uid;
    public Vector3 point;
    public Vector3 velocity;
    public bool followRotate;
    public float gravity;
    public float timer;
}
