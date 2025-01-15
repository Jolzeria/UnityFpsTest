using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class GunPool : Singleton<GunPool>
{
    private Dictionary<BeUnit, Dictionary<GunType, GameObject>> m_GunPool;
    private Transform m_PoolTransform;
    
    public override void Init()
    {
        base.Init();

        m_GunPool = new Dictionary<BeUnit, Dictionary<GunType, GameObject>>();
    }

    public override void UnInit()
    {
        base.UnInit();
        
        if (m_GunPool != null && m_GunPool.Count > 0)
        {
            foreach (var temp in m_GunPool)
            {
                foreach (var gun in temp.Value)
                {
                    Release(gun.Value);
                }
                temp.Value.Clear();
            }

            m_GunPool.Clear();
        }
        m_GunPool = null;
    }
    
    public void SetParent(Transform parent)
    {
        m_PoolTransform = parent;
    }

    public GameObject Get(BeUnit beUnit, GunType gunType)
    {
        if (m_GunPool == null) return null;

        if (!m_GunPool.TryGetValue(beUnit, out var guns))
        {
            guns = new Dictionary<GunType, GameObject>();
            m_GunPool[beUnit] = guns;
        }

        if (!guns.TryGetValue(gunType, out var gun))
        {
            gun = CreateWeapon(gunType);
            guns.Add(gunType, gun);
        }

        gun.SetActive(true);
        return gun;
    }

    public void Release(GameObject gun)
    {
        if (gun == null) return;
        if (m_GunPool == null) return;

        gun.SetActive(false);

        if (m_PoolTransform != null)
        {
            gun.transform.SetParent(m_PoolTransform);
        }
    }

    public void ReleaseUnit(BeUnit beUnit)
    {
        if (m_GunPool.TryGetValue(beUnit, out var guns))
        {
            foreach (var gun in guns)
            {
                Release(gun.Value);
            }
        }
    }

    public void Remove(BeUnit beUnit, GunType gunType)
    {
        if (beUnit == null)
        {
            Debug.LogError("Invalid parameters for Remove.");
            return;
        }

        if (m_GunPool.TryGetValue(beUnit, out var guns) &&
            guns.TryGetValue(gunType, out var gun))
        {
            GameObject.Destroy(gun);
            
            guns.Remove(gunType);
            if (guns.Count == 0)
            {
                m_GunPool.Remove(beUnit);
            }
        }
    }
    
    /// <summary>
    /// 创建枪
    /// </summary>
    private GameObject CreateWeapon(GunType gunType)
    {
        GameObject gun = null;
        switch (gunType)
        {
            case GunType.Pistol:
            {
                var prefab = Resources.Load<GameObject>("Pistol");
                gun = GameObject.Instantiate(prefab);
            }
                break;
            case GunType.MachineGun:
            {
                var prefab = Resources.Load<GameObject>("MachineGun");
                gun = GameObject.Instantiate(prefab);
            }
                break;
            case GunType.Shotgun:
            {
                var prefab = Resources.Load<GameObject>("Shotgun");
                gun = GameObject.Instantiate(prefab);
            }
                break;
            case GunType.SniperRifle:
            {
                var prefab = Resources.Load<GameObject>("SniperRifle");
                gun = GameObject.Instantiate(prefab);
            }
                break;
            default:
                break;
        }

        return gun;
    }
}