using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 50f;
    //private Rigidbody rb;
    float yaw;

    void Start()
    {
        //    rb = GetComponent<Rigidbody>();
        yaw = transform.rotation.eulerAngles.y;
        Debug.Log(yaw);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //rb.AddTorque(transform.right * -1 * speed * Time.deltaTime);
            yaw -= speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            yaw += speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}
