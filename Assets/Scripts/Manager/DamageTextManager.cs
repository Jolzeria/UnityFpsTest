using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : Singleton<DamageTextManager>
{
    private Queue<DamageTextData> damageTextInfos;
    private Transform m_CanvasTransform;
    
    public struct DamageTextData
    {
        public Vector3 position;
        public int damage;
    }
    
    public override void Init()
    {
        base.Init();
        
        damageTextInfos = new Queue<DamageTextData>();
    }

    public override void UnInit()
    {
        base.UnInit();
        
        damageTextInfos.Clear();
        damageTextInfos = null;
    }

    public void Update()
    {
        while (damageTextInfos.Count > 0)
        {
            var damageTextInfo = damageTextInfos.Dequeue();
            var obj = CreateDamageText();
            var script = obj.GetComponent<DamageText>();
            script.SetData(damageTextInfo, m_CanvasTransform);
            
            obj.transform.SetParent(m_CanvasTransform);
        }
    }

    public void Add(DamageTextData damageTextData)
    {
        damageTextInfos?.Enqueue(damageTextData);
    }
    
    public void SetCanvas(Transform parent)
    {
        m_CanvasTransform = parent;
    }
    
    private GameObject CreateDamageText()
    {
        var damageText = DamageTextPool.Instance.Get();
        return damageText;
    }
}