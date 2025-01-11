using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnit : BeUnit
{
    private Transform gunSlot;
    
    protected override void Init()
    {
        base.Init();

        attribute = new CharacterAttribute();
        attribute.Init();

        gunSlot = transform.Find("root/gunSlot");
        // 默认装备
        EquipWeapon(GunType.Pistol);
    }

    protected override void UnInit()
    {
        base.UnInit();

        attribute.UnInit();
    }

    private void Update()
    {
        // 按1装备手枪
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(GunType.Pistol);
        }

        // 按5收回所有枪
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GunPool.Instance.ReleaseUnit(this);
        }
    }

    private void EquipWeapon(GunType gunType)
    {
        var gun = GunPool.Instance.Get(this, gunType);
        
        gun.transform.SetParent(gunSlot);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);

        var script = gun.GetComponent<BaseGun>();
        script.EquipWeapon(this);
    }
}