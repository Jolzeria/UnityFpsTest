using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shotgun : BaseGun
{
    public float speed = 100f;
    public float gravity = 0f;
    public bool followRotate = false;
    public float shootInterval = 2f;
    public float duration = 5f;

    public int bulletCount = 20;
    public int sectorCount = 4;
    public float rotationConstraint = 1f;
    public float normalDistributionFactor = 1f;
    public float radius = 50f;

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

        var randomEndPos = GetEndPoint(bulletCount, sectorCount, rotationConstraint, normalDistributionFactor, radius);

        foreach (var endPos in randomEndPos)
        {
            if (Physics.Raycast(mainCameraTrans.position, endPos - mainCameraTrans.position, out var hitInfo,
                speed * duration, LayerManager.Environment | LayerManager.Enemy))
            {
                direction = hitInfo.point - muzzle.transform.position;
                realDuration = duration;
            }
            else
            {
                direction = endPos - muzzle.transform.position;
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

        // for (int i = 0; i < 20; i++)
        // {
        //     if (Physics.Raycast(mainCameraTrans.position, mainCameraTrans.forward + GetOffsetCoord(), out var hitInfo,
        //         speed * duration, LayerManager.Environment | LayerManager.Enemy))
        //     {
        //         direction = hitInfo.point - muzzle.transform.position;
        //         realDuration = duration;
        //     }
        //     else
        //     {
        //         var endPoint = mainCameraTrans.position + mainCameraTrans.forward * speed * duration;
        //         direction = endPoint - muzzle.transform.position;
        //         realDuration = duration * (direction.magnitude / (speed * duration));
        //     }
        //
        //     var bullet = CreateBullet();
        //     var bulletCreateData = new ParabolaCurveCreateData
        //     {
        //         uid = bullet.GetInstanceID(),
        //         startPoint = muzzle.transform.position,
        //         startRotation = muzzle.transform.rotation,
        //         speed = speed,
        //         direction = direction.normalized,
        //         followRotate = followRotate,
        //         gravity = gravity,
        //         duration = realDuration
        //     };
        //
        //     ParabolaCurveManager.Instance.Add(bulletCreateData, bullet);
        // }
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

    /*/// <summary>
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
    }*/

    /// <summary>
    /// 返回随机位置后的终点坐标List
    /// </summary>
    /// <param name="vBulletCount">需要随机位置的子弹总数</param>
    /// <param name="vSectorCount">需要随机的区块数</param>
    /// <param name="vRotationConstraint">旋转约定系数(随机落点的扇形大小)</param>
    /// <param name="vNormalDistributionFactor">正态分布因子</param>
    /// <param name="vRadius">准心半径</param>
    private List<Vector3> GetEndPoint(int vBulletCount, int vSectorCount, float vRotationConstraint,
        float vNormalDistributionFactor, float vRadius)
    {
        var result = new List<Vector3>();

        // 获取摄像机FOV（垂直角度），计算出屏幕到摄像机的真实距离
        var m_Camera = Camera.main;
        var cameraPos = m_Camera.transform.position;
        var verticalDegree = m_Camera.fieldOfView;
        var screenDepth = (float) Screen.height / 2f / Mathf.Tan(verticalDegree / 2f * Mathf.Deg2Rad);

        // 基础数据
        var bulletMaxDistance = speed * duration;
        var endPos = m_Camera.transform.position + m_Camera.transform.forward * bulletMaxDistance;

        // 分几组循环
        var loopGroupSize = vBulletCount / vSectorCount + 1;

        for (int j = 0; j < loopGroupSize; j++)
        {
            // 圆内第一个真随机点的角度
            var angleFirst = Random.Range(0.0f, 360.0f);

            // 当前组的循环次数
            var curLoopSize = j == loopGroupSize - 1 ? vBulletCount % vSectorCount : vSectorCount;

            // 根据第一个点的角度进行随机
            for (int i = 0; i < curLoopSize; i++)
            {
                var angleMin = angleFirst + 360.0f / vSectorCount * i - 180.0f * (1 - vRotationConstraint);
                var angleMax = angleFirst + 360.0f / vSectorCount * i + 180.0f * (1 - vRotationConstraint);
                var angleRandom = Random.Range(angleMin, angleMax);

                var randomResult = Random.Range(0.0f, 1.0f);
                // 准心随机半径-像素值
                var radiusRandom = vRadius * Mathf.Pow(randomResult, vNormalDistributionFactor);
                // 从镜头半径放大到终点半径
                var distance = Vector3.Distance(endPos, cameraPos);
                radiusRandom = radiusRandom / screenDepth * distance;

                // 最终的命中点偏移坐标
                var offset = new Vector3(Mathf.Cos(angleRandom * Mathf.Deg2Rad) * radiusRandom,
                    Mathf.Sin(angleRandom * Mathf.Deg2Rad) * radiusRandom, 0);
                // 将偏移坐标转向摄像机正面方向
                offset = Quaternion.LookRotation(m_Camera.transform.forward) * offset;
                // 计算最后子弹终点的坐标
                var finalPosition = m_Camera.transform.position + m_Camera.transform.forward * bulletMaxDistance + offset;

                result.Add(finalPosition);
            }
        }

        return result;
    }
}