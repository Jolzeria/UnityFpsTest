using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class BulletManager
{
    private static Dictionary<int, GameObject> bulletObjectDic;
    private static List<ParabolaCurveCreateData> parabolaCurveCreateDatas;
    private static List<ParabolaCurveUpdateData> parabolaCurveUpdateDatas;

    public static void Init()
    {
        bulletObjectDic = new Dictionary<int, GameObject>();
        parabolaCurveCreateDatas = new List<ParabolaCurveCreateData>();
        parabolaCurveUpdateDatas = new List<ParabolaCurveUpdateData>();
    }

    public static void UnInit()
    {
        foreach (var kPair in bulletObjectDic)
        {
            var bullet = kPair.Value;
            BulletPool.Release(bullet);
        }

        bulletObjectDic.Clear();
        bulletObjectDic = null;
        parabolaCurveCreateDatas.Clear();
        parabolaCurveUpdateDatas = null;
        parabolaCurveCreateDatas.Clear();
        parabolaCurveUpdateDatas = null;
    }

    public static void Update()
    {

    }

    public static void FixedUpdate()
    {
        for (int i = 0; i < parabolaCurveUpdateDatas.Count; i++)
        {
            var curData = parabolaCurveUpdateDatas[i];
            var nextData = UpdateData(parabolaCurveUpdateDatas[i], Time.fixedDeltaTime);
            parabolaCurveUpdateDatas[i] = nextData;

            UpdateObject(curData, nextData);
        }
    }

    public static void Add(ParabolaCurveCreateData data)
    {
        var bullet = CreateBullet();
        data.uid = bullet.GetInstanceID();

        //obj.transform.SetPositionAndRotation(data.startPoint, data.startRotation);
        bullet.transform.position = data.startPoint;
        bullet.transform.rotation = data.followRotate ? Quaternion.LookRotation(data.direction, data.startRotation * Vector3.up) : data.startRotation;

        bulletObjectDic.Add(data.uid, bullet);
        parabolaCurveCreateDatas.Add(data);

        var velocity = data.direction * data.speed;
        var bulletUpdateData = new ParabolaCurveUpdateData
        {
            uid = data.uid,
            point = data.startPoint,
            velocity = velocity,
            followRotate = data.followRotate,
            gravity = data.gravity,
            timer = data.duration
        };
        parabolaCurveUpdateDatas.Add(bulletUpdateData);
    }

    public static void Remove(int uid)
    {
        if (bulletObjectDic.TryGetValue(uid, out var bullet))
        {
            bulletObjectDic.Remove(uid);
            ReleaseBullet(bullet);
        }

        for (int i = parabolaCurveCreateDatas.Count - 1; i >= 0; i--)
        {
            var temp = parabolaCurveCreateDatas[i];
            if (temp.uid != uid)
                continue;
            parabolaCurveCreateDatas.Remove(temp);
            break;
        }

        for (int i = parabolaCurveUpdateDatas.Count - 1; i >= 0; i--)
        {
            var temp = parabolaCurveUpdateDatas[i];
            if (temp.uid != uid)
                continue;
            parabolaCurveUpdateDatas.Remove(temp);
            break;
        }
    }

    private static ParabolaCurveUpdateData UpdateData(ParabolaCurveUpdateData data, float deltaTime)
    {
        var gravity = Mathf.Abs(data.gravity);
        var velocity = data.velocity - new Vector3(0, gravity * deltaTime, 0);
        //var gravity_vector = Vector3.up * -1 * gravity;
        //var velocity = data.velocity + gravity_vector;
        var point = data.point + velocity * deltaTime;
        var timer = data.timer - deltaTime;


        var newData = new ParabolaCurveUpdateData
        {
            uid = data.uid,
            point = point,
            velocity = velocity,
            followRotate = data.followRotate,
            gravity = data.gravity,
            timer = timer
        };
        return newData;
    }

    private static void UpdateObject(ParabolaCurveUpdateData curData, ParabolaCurveUpdateData nextData)
    {
        // 判空
        if (!bulletObjectDic.TryGetValue(nextData.uid, out var obj))
            return;

        if (nextData.timer <= 0)
        {
            Remove(nextData.uid);
            return;
        }

        if (IsBounding(curData, nextData, out var hitInfo))
        {
            obj.transform.position = hitInfo.point;
            if (nextData.followRotate)
            {
                var direction = nextData.velocity;
                obj.transform.rotation = Quaternion.LookRotation(direction, obj.transform.up);
            }

            var layer = hitInfo.collider.gameObject.layer;
            switch (layer)
            {
                case Layer.Enemy:
                    {
                        var damageInfo = new DamageInfo()
                        {
                            damage = 10,
                            attacker = null,
                            receiver = hitInfo.collider.GetComponentInParent<BeUnit>(),
                            hitPoint = hitInfo.point
                        };
                        DamageManager.Add(damageInfo);
                    }
                    break;
            }

            Remove(nextData.uid);
            return;
        }

        obj.transform.position = nextData.point;
        if (nextData.followRotate)
        {
            var direction = nextData.velocity;
            obj.transform.rotation = Quaternion.LookRotation(direction, obj.transform.up);
        }
    }

    private static GameObject CreateBullet()
    {
        var bullet = BulletPool.Get();

        bullet.transform.localScale = Vector3.one;

        return bullet;
    }

    private static void ReleaseBullet(GameObject bullet)
    {
        BulletPool.Release(bullet);
    }

    private static bool IsBounding(ParabolaCurveUpdateData curData, ParabolaCurveUpdateData nextData, out RaycastHit hitInfo)
    {
        // 计算出射线方向
        var rayDirection = nextData.point - curData.point;

        return Physics.Raycast(curData.point, rayDirection, out hitInfo, rayDirection.magnitude);
    }
}
