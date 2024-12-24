using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BulletPool : Singleton<BulletPool>
{
    // 私有化构造函数，禁止实例化
    private BulletPool()
    {
    }

    private Queue<GameObject> m_BulletPool;
    private Transform m_PoolTransform;

    public void Init(Transform parent)
    {
        m_BulletPool = new Queue<GameObject>();

        m_PoolTransform = parent;

        for (int i = 0; i < 3; i++)
        {
            var bullet = CreateBullet();
            if (bullet == null)
            {
                continue;
            }

            Release(bullet);
        }
    }

    public void UnInit()
    {
        m_BulletPool?.Clear();
        m_BulletPool = null;
    }

    public GameObject Get()
    {
        if (m_BulletPool == null) return null;

        if (m_BulletPool.Count > 0)
        {
            var bullet = m_BulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }

        return CreateBullet();
    }

    public void Release(GameObject bullet)
    {
        if (bullet == null) return;
        if (m_BulletPool == null) return;

        bullet.SetActive(false);
        m_BulletPool.Enqueue(bullet);

        if (m_PoolTransform != null)
        {
            bullet.transform.SetParent(m_PoolTransform);
        }
    }

    private GameObject CreateBullet()
    {
        var bulletPrefab = Resources.Load<GameObject>("Bullet");
        var bullet = GameObject.Instantiate(bulletPrefab);
        return bullet;
    }
}