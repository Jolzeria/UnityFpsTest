using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : Singleton<DamageTextManager>
{
    private Queue<DamageTextData> damageTextInfos;
    private Transform m_CanvasTransform;
    private List<DamageTextData> livedDamageTexts;
    
    public struct DamageTextData
    {
        public Vector3 position;
        public int damage;
    }
    
    public override void Init()
    {
        base.Init();
        
        damageTextInfos = new Queue<DamageTextData>();
        livedDamageTexts = new List<DamageTextData>();
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
            
            // 增加随机值
            var offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            damageTextInfo.position += offset;
            
            // 判断是否重叠，如果重叠就向上位移
            bool isOverlay = true;
            while (isOverlay)
            {
                if (livedDamageTexts.Count == 0)
                    break;
                
                for (var i = 0; i < livedDamageTexts.Count; i++)
                {
                    var distanceSqr = (damageTextInfo.position - livedDamageTexts[i].position).sqrMagnitude;
                    var radiusSqr = 0.6;
                    if (distanceSqr < radiusSqr)
                    {
                        isOverlay = true;
                        damageTextInfo.position += new Vector3(0, 0.1f, 0);
                        break;
                    }

                    if (i == livedDamageTexts.Count - 1)
                        isOverlay = false;
                }
            }
            
            script.SetData(damageTextInfo, m_CanvasTransform);
            
            obj.transform.SetParent(m_CanvasTransform);
            AddLivedText(damageTextInfo);
        }
    }

    public void Add(DamageTextData damageTextData)
    {
        if (damageTextInfos == null)
            return;
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

    private void AddLivedText(DamageTextData data)
    {
        if (livedDamageTexts == null)
            return;
        livedDamageTexts.Add(data);
    }

    public void RemoveLivedText(DamageTextData data)
    {
        if (livedDamageTexts == null)
            return;
        livedDamageTexts.Remove(data);
    }
}