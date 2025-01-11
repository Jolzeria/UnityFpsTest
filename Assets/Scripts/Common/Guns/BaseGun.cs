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