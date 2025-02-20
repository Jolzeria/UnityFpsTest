using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterUnit : BeUnit
{
    private Transform gunSlot;
    
    private Transform gunInfoTrans;
    private TMP_Text ammoCountText;
    private Transform pistolImageTrans;
    private Transform machineImageTrans;
    private Transform shotgunImageTrans;
    private Transform sniperImageTrans;

    // 是否已初始化
    private bool isInit = false;
    
    protected override void Init()
    {
        base.Init();

        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        // 等LogicFrame初始化后再调用单例对象
        yield return new WaitForSeconds(0.1f);
        
        attribute = new CharacterAttribute();
        attribute.Init();

        gunSlot = transform.Find("root/gunSlot");
        
        // 获取右下角子弹数相关UI对象
        gunInfoTrans = InstanceManager.Instance.Get(InstanceType.TwoDCanvas).Find("GunInfo");
        ammoCountText = gunInfoTrans.Find("Ammo").GetComponent<TMP_Text>();
        pistolImageTrans = gunInfoTrans.Find("PistolImage");
        machineImageTrans = gunInfoTrans.Find("MachinegunImage");
        shotgunImageTrans = gunInfoTrans.Find("ShotgunImage");
        sniperImageTrans = gunInfoTrans.Find("SniperImage");
        
        // 默认装备
        EquipWeapon(GunType.Pistol);
        SwitchWeaponImage(GunType.Pistol);
        
        isInit = true;
    }

    protected override void UnInit()
    {
        base.UnInit();

        attribute.UnInit();
    }

    private void Update()
    {
        if (!isInit) return;
        
        // 按1-4换枪
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GunPool.Instance.ReleaseUnit(this);
            EquipWeapon(GunType.Pistol);
            SwitchWeaponImage(GunType.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunPool.Instance.ReleaseUnit(this);
            EquipWeapon(GunType.MachineGun);
            SwitchWeaponImage(GunType.MachineGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunPool.Instance.ReleaseUnit(this);
            EquipWeapon(GunType.Shotgun);
            SwitchWeaponImage(GunType.Shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunPool.Instance.ReleaseUnit(this);
            EquipWeapon(GunType.SniperRifle);
            SwitchWeaponImage(GunType.SniperRifle);
        }

        // // 按5收回所有枪
        // if (Input.GetKeyDown(KeyCode.Alpha5))
        // {
        //     GunPool.Instance.ReleaseUnit(this);
        // }
        
        // 显示右下角子弹数
        ammoCountText.text = attribute.GetAttrValue(AttributeType.CurAmmo).ToString();
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

    private void SwitchWeaponImage(GunType gunType)
    {
        pistolImageTrans.gameObject.SetActive(false);
        machineImageTrans.gameObject.SetActive(false);
        shotgunImageTrans.gameObject.SetActive(false);
        sniperImageTrans.gameObject.SetActive(false);
        
        switch (gunType)
        {
            case GunType.Pistol:
                pistolImageTrans.gameObject.SetActive(true);
                break;
            case GunType.MachineGun:
                machineImageTrans.gameObject.SetActive(true);
                break;
            case GunType.Shotgun:
                shotgunImageTrans.gameObject.SetActive(true);
                break;
            case GunType.SniperRifle:
                sniperImageTrans.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gunType), gunType, null);
        }
    }
}