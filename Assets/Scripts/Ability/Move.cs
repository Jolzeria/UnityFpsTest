using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1f;

    private Rigidbody rb;
    private Transform root;
    private Camera mainCam;

    private void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        root = transform.Find("root");
        mainCam = Camera.main;
    }

    private void OnDestroy()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        /*var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontal, 0, vertical) * speed;
        var moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
        var velocity = moveDirection * speed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        var force = new Vector3(horizontal, 0, vertical) * speed;
        rb.AddForce(force);*/


        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        // 转向摄像机朝向的方向
        var cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        root.localRotation = Quaternion.Slerp(root.localRotation, Quaternion.LookRotation(cameraForward), 10f * Time.fixedDeltaTime);
        
        // 合成移动方向
        var direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude > 0.1f)
        {
            //var forward = mainCam.transform.forward.WithY(0);
            //var to = Quaternion.LookRotation(direction) * Quaternion.LookRotation(forward);
            //root.localRotation = Quaternion.RotateTowards(root.localRotation, to, 500f * Time.fixedDeltaTime);
            //transform.Translate(to * Vector3.forward * speed * Time.fixedDeltaTime);

            // 将移动方向先转到主相机的坐标系中得到最终的移动方向
            var moveDir = (mainCam.transform.rotation * direction).WithY(0);
            // 限制每次转动角度的情况下转动方向
            root.localRotation = Quaternion.RotateTowards(root.localRotation, Quaternion.LookRotation(moveDir), 500f * Time.fixedDeltaTime);
            // 移动
            transform.Translate(moveDir * speed * Time.fixedDeltaTime);
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
