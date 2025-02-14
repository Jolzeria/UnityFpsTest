using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeUnit : MonoBehaviour
{
    protected BaseAttribute attribute;
    public BeUnit OriginalCreator { get; set; }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        //UnInit();
    }

    protected virtual void Init()
    {
        RegisterEvent();
    }

    protected virtual void UnInit()
    {
        UnRegisterEvent();
    }

    protected virtual void RegisterEvent()
    {
    }

    protected virtual void UnRegisterEvent()
    {
    }

    public void AddAttrValue(AttributeType attr, float value)
    {
        attribute.AddAttrValue(attr, value);
    }

    public float GetAttrValue(AttributeType attr)
    {
        return attribute.GetAttrValue(attr);
    }

    public void SetAttrValue(AttributeType attr, float value)
    {
        attribute.SetAttrValue(attr, value);
    }
}