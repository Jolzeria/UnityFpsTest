using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    protected Transform muzzle;
    protected float shootTimer;
    protected BeUnit equippedUnit;
    protected bool isLoading = false;

    protected float ATK;
    protected float CurAmmo;
    protected float MagazineSize;
    protected float TotalAmmo;
    protected float MaxAmmo;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void Init()
    {
        muzzle = transform.Find("root/muzzle");
        shootTimer = 0f;
    }

    protected virtual void OnUpdate()
    {
    }

    protected void Shoot()
    {
        // 换弹匣时不能射击
        if (isLoading)
        {
            Debug.Log("正在换弹");
            return;
        }

        // 没子弹了
        if (CurAmmo == 0 && TotalAmmo == 0)
        {
            Debug.Log("没子弹了");
            return;
        }

        // 换弹匣
        if (CurAmmo == 0 && TotalAmmo > 0)
        {
            isLoading = true;

            var reloadAmmoNum = MagazineSize;
            if (TotalAmmo < MagazineSize)
                reloadAmmoNum = TotalAmmo;
            CurAmmo = reloadAmmoNum;
            equippedUnit.AddAttrValue(AttributeType.CurAmmo, reloadAmmoNum);
            TotalAmmo -= reloadAmmoNum;
            equippedUnit.AddAttrValue(AttributeType.TotalAmmo, -reloadAmmoNum);
            Debug.Log("换子弹");

            isLoading = false;
            return;
        }

        CurAmmo -= 1f;
        equippedUnit.AddAttrValue(AttributeType.CurAmmo, -1);
        
        // 修改分数
        ScoreManager.Instance.Score1Add(1);

        ShootBullet();
    }

    protected virtual void ShootBullet()
    {
    }

    public void EquipWeapon(BeUnit beUnit)
    {
        equippedUnit = beUnit;
        SetAttributes();
    }

    protected void SetAttributes()
    {
        equippedUnit.SetAttrValue(AttributeType.ATK, ATK);
        equippedUnit.SetAttrValue(AttributeType.CurAmmo, CurAmmo);
        equippedUnit.SetAttrValue(AttributeType.MagazineSize, MagazineSize);
        equippedUnit.SetAttrValue(AttributeType.TotalAmmo, TotalAmmo);
        equippedUnit.SetAttrValue(AttributeType.MaxAmmo, MaxAmmo);
    }
}