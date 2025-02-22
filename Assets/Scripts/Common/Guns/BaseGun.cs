using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    protected Transform muzzle;
    protected float shootTimer;
    public float shootInterval;
    // 装备的对象
    protected BeUnit equippedUnit;
    // 换弹相关
    protected bool isReLoading;
    protected float reloadTextShowTime;
    protected Transform reloadBulletTextTrans;
    protected TMP_Text reloadBulletText;

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

        isReLoading = false;
        reloadTextShowTime = 1f;
        reloadBulletTextTrans = InstanceManager.Instance.Get(InstanceType.ReloadBulletText);
        reloadBulletText = reloadBulletTextTrans.GetComponent<TMP_Text>();
    }

    protected virtual void OnUpdate()
    {
        if (reloadTextShowTime >= 0)
            reloadTextShowTime -= Time.deltaTime;
        if (reloadTextShowTime <= 0)
            reloadBulletTextTrans.gameObject.SetActive(false);
        
        // 射击间隔时间内不能再次射击
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    protected void Shoot()
    {
        // 换弹匣时不能射击
        if (isReLoading)
        {
            reloadTextShowTime = 1f;
            reloadBulletText.text = "正在换弹…";
            reloadBulletTextTrans.gameObject.SetActive(true);
            return;
        }

        // 没子弹了
        if (CurAmmo == 0 && TotalAmmo == 0)
        {
            reloadTextShowTime = 1f;
            reloadBulletText.text = "没子弹了";
            reloadBulletTextTrans.gameObject.SetActive(true);
            return;
        }
        
        // 射击间隔时间内不能再次射击
        if (shootTimer > 0)
        {
            return;
        }
        shootTimer = shootInterval;

        // 换弹匣
        if (isReLoading == false && CurAmmo == 0 && TotalAmmo > 0)
        {
            isReLoading = true;
            StartCoroutine(WaitForReload());

            return;
        }

        CurAmmo -= 1f;
        equippedUnit.AddAttrValue(AttributeType.CurAmmo, -1);
        
        // 修改分数
        ScoreManager.Instance.Score1Add(1);

        ShootBullet();
    }

    IEnumerator WaitForReload()
    {
        yield return new WaitForSeconds(2f);
        
        var reloadAmmoNum = MagazineSize;
        if (TotalAmmo < MagazineSize)
            reloadAmmoNum = TotalAmmo;
        CurAmmo = reloadAmmoNum;
        equippedUnit.AddAttrValue(AttributeType.CurAmmo, reloadAmmoNum);
        TotalAmmo -= reloadAmmoNum;
        equippedUnit.AddAttrValue(AttributeType.TotalAmmo, -reloadAmmoNum);

        isReLoading = false;
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