using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shotgun : BaseGun
{
    public float speed = 10f;
    public float gravity = 0f;
    public bool followRotate = false;
    public float shootInterval = 2f;
    public float duration = 5f;

    protected override void Init()
    {
        base.Init();

        ATK = 10;
        CurAmmo = 2;
        MagazineSize = 2;
        TotalAmmo = 20;
        MaxAmmo = 20;
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

        for (int i = 0; i < 20; i++)
        {
            if (Physics.Raycast(mainCameraTrans.position, mainCameraTrans.forward + GetOffsetCoord(), out var hitInfo,
                speed * duration, LayerManager.Environment | LayerManager.Enemy))
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
    }

    private GameObject CreateBullet()
    {
        var bullet = RoundBulletPool.Instance.Get();
        var bulletEntity = bullet.AddComponent<EntityUnit>();
        var originalCreator = transform.GetComponentInParent<BeUnit>();
        bulletEntity.OriginalCreator = originalCreator;

        bullet.transform.localScale = Vector3.one;

        return bullet;
    }

    /// <summary>
    /// </summary>
    /// <returns>获取偏移坐标</returns>
    private Vector3 GetOffsetCoord()
    {
        var offset = new Vector3(NextGaussian(0, 0.2f, -0.05f, 0.05f), NextGaussian(0, 0.2f, -0.05f, 0.05f), 0);

        return offset;
    }

    /// <summary>
    /// 生成高斯分布数
    /// </summary>
    /// <param name="mean">均值</param>
    /// <param name="variance">方差</param>
    /// <param name="min">不想要的最小值</param>
    /// <param name="max">不想要的最大值</param>
    /// <returns></returns>
    private float NextGaussian(float mean, float variance, float min, float max)
    {
        float x;
        do
        {
            x = NextGaussian(mean, variance);
        } while (x < min || x > max);

        return x;
    }

    private float NextGaussian(float mean, float standardDeviation)
    {
        return mean + NextGaussian() * standardDeviation;
    }

    private float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
        return v1 * s;
    }
}