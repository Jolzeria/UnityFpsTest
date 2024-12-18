using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 0f;
    public bool followRotate = false;

    private Transform muzzle;

    private void Start()
    {
        muzzle = transform.Find("root/muzzle");
    }

    private void Update()
    {
        //TODO:发射子弹
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
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

        var bulletCreateData = new ParabolaCurveCreateData
        {
            startPoint = muzzle.transform.position,
            startRotation = muzzle.transform.rotation,
            speed = speed,
            direction = muzzle.transform.forward,
            followRotate = followRotate,
            gravity = gravity,
            duration = 5f
        };

        BulletManager.Add(bulletCreateData);
    }
}
