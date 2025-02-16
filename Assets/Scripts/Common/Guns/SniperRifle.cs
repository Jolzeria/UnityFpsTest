using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SniperRifle : BaseGun
{
    public float speed = 300f;
    public float gravity = 0f;
    public bool followRotate = false;
    public float duration = 15f;

    protected override void Init()
    {
        base.Init();

        ATK = 30;
        CurAmmo = 7;
        MagazineSize = 7;
        TotalAmmo = 42;
        MaxAmmo = 42;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        // 发射子弹
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        // 开镜
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ThirdPerson.OpenScope();
        }

        if (Input.GetKeyUp((KeyCode.Mouse1)))
        {
            ThirdPerson.CloseScope();
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
