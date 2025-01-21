using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BulletMarksPool : Singleton<BulletMarksPool>
{
    private Queue<GameObject> m_BulletMarksPool;
    private Transform m_PoolTransform;
    
    public override void Init()
    {
        base.Init();
        
        m_BulletMarksPool = new Queue<GameObject>();
    }

    public override void UnInit()
    {
        base.UnInit();
        
        m_BulletMarksPool?.Clear();
        m_BulletMarksPool = null;
    }
    
    public void SetParent(Transform parent)
    {
        m_PoolTransform = parent;
    }

    public GameObject Get()
    {
        if (m_BulletMarksPool == null) return null;

        if (m_BulletMarksPool.Count > 0)
        {
            var bullet = m_BulletMarksPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        return CreateBulletMarks();
    }

    public void Release(GameObject bullet)
    {
        if (bullet == null) return;
        if (m_BulletMarksPool == null) return;

        bullet.SetActive(false);
        m_BulletMarksPool.Enqueue(bullet);

        if (m_PoolTransform != null)
        {
            bullet.transform.SetParent(m_PoolTransform);
        }
    }

    private GameObject CreateBulletMarks()
    {
        var prefab = Resources.Load<GameObject>("BulletMarks");
        var obj = GameObject.Instantiate(prefab);
        return obj;
    }
}