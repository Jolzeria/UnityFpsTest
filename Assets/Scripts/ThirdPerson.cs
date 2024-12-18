using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    private GameObject player;

    private Quaternion charaRotation;
    private float m_Pitch;
    private float m_Yaw;

    public float rotateSpeed = 10f;
    public float cameraDisChara = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        // 记录角色初始rotation、pitch、yaw
        charaRotation = player.transform.rotation;
        m_Pitch = transform.rotation.x;
        m_Yaw = transform.rotation.y;
    }

    private void FixedUpdate()
    {
        //transform.position = player.transform.position + player.transform.forward * -3 + player.transform.up * 2;

        // 相机跟随角色转动
        // 先将Vector3(0, 2, -3)转向到跟玩家一个方向，再移动到玩家的位置，最后将Vector3(0, 2, -3)这个世界向量转为了相对于玩家的本地向量
        //transform.position = player.transform.position + player.transform.rotation * new Vector3(0, 2, -3);

        // B-A坐标相减得到A→B向量
        //transform.rotation = Quaternion.LookRotation(player.transform.position + new Vector3(0, 1, 0) - transform.position);


        var horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        var vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

        Rotate(horizontal, vertical);
        Move();
        //transform.rotation = Quaternion.LookRotation(player.transform.position + new Vector3(0, 1, 0) - transform.position);
    }

    private void Rotate(float horizontal, float vertical)
    {
        if (Input.GetMouseButton(1))
        {
            //transform.rotation = Quaternion.Euler(oriRotation.x, oriRotation.y + mouseX, oriRotation.z);
            m_Yaw += horizontal;
            // pitch向下时值增大，相反
            m_Pitch -= vertical;
            m_Pitch = Mathf.Clamp(m_Pitch, -70, 70);
        }
        // 先将当前物体坐标系转向世界坐标系z轴正方向
        // 再绕 Z 轴旋转 0 角度，绕 X 轴旋转 m_Pitch 角度，绕 Y 轴旋转 m_Yaw 角度
        transform.rotation = charaRotation * (Quaternion.Euler(m_Pitch, m_Yaw, 0) * Quaternion.LookRotation(Vector3.forward));
    }

    private void Move()
    {
        transform.position = player.transform.position + new Vector3(0, 3, 0) - transform.forward * cameraDisChara;
    }
}
