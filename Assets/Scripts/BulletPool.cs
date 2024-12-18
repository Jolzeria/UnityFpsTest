using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BulletPool
{
    private static Queue<GameObject> m_BulletPool;
    private static Transform m_PoolTransform;

    public static void Init(Transform parent)
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

    public static void UnInit()
    {
        m_BulletPool?.Clear();
        m_BulletPool = null;
    }

    public static GameObject Get()
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

    public static void Release(GameObject bullet)
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

    private static GameObject CreateBullet()
    {
        var bulletPrefab = Resources.Load<GameObject>("Bullet");
        var bullet = GameObject.Instantiate(bulletPrefab);
        return bullet;
    }
}
