using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 抛物线类，控制子弹轨迹等
/// </summary>
public class ParabolaCurveManager : Singleton<ParabolaCurveManager>
{
    private Dictionary<int, GameObject> bulletObjectDic;
    private List<ParabolaCurveCreateData> parabolaCurveCreateDatas;
    private List<ParabolaCurveUpdateData> parabolaCurveUpdateDatas;

    public override void Init()
    {
        base.Init();

        bulletObjectDic = new Dictionary<int, GameObject>();
        parabolaCurveCreateDatas = new List<ParabolaCurveCreateData>();
        parabolaCurveUpdateDatas = new List<ParabolaCurveUpdateData>();
    }

    public override void UnInit()
    {
        base.UnInit();

        foreach (var kPair in bulletObjectDic)
        {
            var bullet = kPair.Value;
            BulletPool.Instance.Release(bullet);
        }

        bulletObjectDic.Clear();
        bulletObjectDic = null;
        parabolaCurveCreateDatas.Clear();
        parabolaCurveUpdateDatas = null;
        parabolaCurveCreateDatas.Clear();
        parabolaCurveUpdateDatas = null;
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < parabolaCurveUpdateDatas.Count; i++)
        {
            var curData = parabolaCurveUpdateDatas[i];
            var nextData = UpdateData(parabolaCurveUpdateDatas[i], Time.fixedDeltaTime);
            parabolaCurveUpdateDatas[i] = nextData;

            UpdateObject(curData, nextData);
        }
    }

    public void Add(ParabolaCurveCreateData data, GameObject bullet)
    {
        //obj.transform.SetPositionAndRotation(data.startPoint, data.startRotation);
        bullet.transform.position = data.startPoint;
        bullet.transform.rotation = data.followRotate
            ? Quaternion.LookRotation(data.direction, data.startRotation * Vector3.up)
            : data.startRotation;

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

    public void Remove(int uid)
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

    private ParabolaCurveUpdateData UpdateData(ParabolaCurveUpdateData data, float deltaTime)
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

    private void UpdateObject(ParabolaCurveUpdateData curData, ParabolaCurveUpdateData nextData)
    {
        // 判空
        if (!bulletObjectDic.TryGetValue(nextData.uid, out var obj))
            return;

        if (nextData.timer <= 0)
        {
            Remove(nextData.uid);
            return;
        }

        // 碰撞事件
        if (IsBounding(curData, nextData, out var hitInfo))
        {
            var layer = hitInfo.collider.gameObject.layer;

            if (layer == Layer.Player || layer == Layer.Boundary)
            {
                return;
            }

            obj.transform.position = hitInfo.point;

            if (nextData.followRotate)
            {
                var direction = nextData.velocity;
                obj.transform.rotation = Quaternion.LookRotation(direction, obj.transform.up);
            }

            switch (layer)
            {
                case Layer.Enemy:
                {
                    if (bulletObjectDic.TryGetValue(nextData.uid, out var bullet))
                    {
                        var entity = bullet.GetComponent<EntityUnit>();
                        var snapShot = new SnapShot(entity.OriginalCreator);

                        EventHandler.ExecuteEvent(entity, GameEventEnum.OnCollision, hitInfo, snapShot);
                    }
                }
                    break;
            }

            // 产生弹痕
            if (layer != Layer.Player && layer != Layer.UI && layer != Layer.Weapon)
            {
                var bulletMarks = BulletMarksPool.Instance.Get();
                bulletMarks.transform.SetParent(null);
                bulletMarks.transform.position = hitInfo.point + hitInfo.normal * 0.01f;
                bulletMarks.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                
                bulletMarks.transform.SetParent(hitInfo.transform);
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

    private void ReleaseBullet(GameObject bullet)
    {
        var objTag = bullet.tag;
        if (objTag == "Bullet")
            BulletPool.Instance.Release(bullet);
        else if (objTag == "RoundBullet")
            RoundBulletPool.Instance.Release(bullet);
    }

    private bool IsBounding(ParabolaCurveUpdateData curData, ParabolaCurveUpdateData nextData, out RaycastHit hitInfo)
    {
        // 计算出射线方向
        var rayDirection = nextData.point - curData.point;

        return Physics.Raycast(curData.point, rayDirection, out hitInfo, rayDirection.magnitude);
    }
}