using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public enum InstanceType
{
    None,
    TwoDCanvas,
    PauseCanvas,
    Sight,
    Scope,
    Score,
    TopShowText,
    GameoverInfo,
    LevelList,
    Max
}

public class InstanceManager : Singleton<InstanceManager>
{
    private Dictionary<InstanceType, Transform> m_transforms;

    public override void Init()
    {
        base.Init();

        m_transforms = new Dictionary<InstanceType, Transform>();
    }

    public override void UnInit()
    {
        base.UnInit();

        m_transforms.Clear();
        m_transforms = null;
    }

    public void Add(InstanceType type, Transform trans)
    {
        if (m_transforms == null)
            return;

        m_transforms.Add(type, trans);
    }

    public Transform Get(InstanceType type)
    {
        if (m_transforms.TryGetValue(type, out var trans))
        {
            return trans;
        }

        return null;
    }
}