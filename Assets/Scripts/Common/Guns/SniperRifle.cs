using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SniperRifle : BaseGun
{
    public float speed = 100f;
    public float gravity = 0f;
    public bool followRotate = false;
    public float shootInterval = 0.5f;
    public float duration = 5f;

    protected override void Init()
    {
        base.Init();

        ATK = 20;
        CurAmmo = 7;
        MagazineSize = 7;
        TotalAmmo = 42;
        MaxAmmo = 42;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        // 发射子弹
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootTimer <= 0)
            {
                shootTimer += shootInterval;
                Shoot();
            }
            shootTimer -= Time.deltaTime;
        }
        else
        {
            shootTimer = 0;
        }
    }

    private void Shoot()
    {
        //var rb = bullet.transform.SafeAddComponent<Rigidbody>();
        //rb.useGravity = false;
        //rb.angularDrag = 0;

        //// 子弹回收后再使用，力无限叠加
        //float forwardSpeed = Vector3.Dot(rb.velocity, bullet.transform.forward);
        //if (forwardSpeed == 0)
        //    rb.AddForce(bullet.transform.forward * 2000f);
        
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

        var mainCameraTrans = Camera.main.transform;
        float realDuration;
        Vector3 direction;
        if (Physics.Raycast(mainCameraTrans.position, mainCameraTrans.forward, out var hitInfo, speed * duration, LayerManager.Environment | LayerManager.Enemy))
        {
            direction = hitInfo.point - muzzle.transform.position;
            realDuration = duration;
        }
        else
        {
            var endPoint = mainCameraTrans.position + mainCameraTrans.forward * speed * duration;
            direction = endPoint - muzzle.transform.position;
            realDuration = duration * (direction.magnitude / (speed * duration));
        }
        
        // 子弹射击方向增加偏差值
        var radius = 0.15f;
        var offset = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
        direction += offset;

        var bullet = CreateBullet();
        var bulletCreateData = new ParabolaCurveCreateData
        {
            uid = bullet.GetInstanceID(),
            startPoint = muzzle.transform.position,
            startRotation = muzzle.transform.rotation,
            speed = speed,
            direction = direction.normalized,
            followRotate = followRotate,
            gravity = gravity,
            duration = realDuration
        };

        ParabolaCurveManager.Instance.Add(bulletCreateData, bullet);
    }

    private GameObject CreateBullet()
    {
        var bullet = BulletPool.Instance.Get();
        var bulletEntity = bullet.AddComponent<EntityUnit>();
        var originalCreator = transform.GetComponentInParent<BeUnit>();
        bulletEntity.OriginalCreator = originalCreator;

        bullet.transform.localScale = Vector3.one;

        return bullet;
    }
}
