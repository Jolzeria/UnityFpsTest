using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pistol : BaseGun
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

    protected override void ShootBullet()
    {
        base.ShootBullet();

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
