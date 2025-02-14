using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveX : MonoBehaviour
{
    // 限制碰撞间隔
    private bool canCollision;
    private float collisionInterval;
    // 限制移动距离
    private float maxMoveDistance;
    private float totalMoveDistance;
    private Vector3 lastPosition;
    //移动方向
    private Vector3 moveDir;
    
    public float speed;

    private void Start()
    {
        canCollision = true;
        collisionInterval = 1f;
        maxMoveDistance = 10f;
        totalMoveDistance = 0f;
        lastPosition = transform.position;
        moveDir = Vector3.right;
        
        speed = 10f;
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDir * speed * Time.fixedDeltaTime, Space.World);
        // transform.rotation = Quaternion.LookRotation(moveDir);
        
        collisionInterval -= Time.fixedDeltaTime;
        if (collisionInterval <= 0)
        {
            collisionInterval = 1f;
            canCollision = true;
        }

        totalMoveDistance += Vector3.Distance(lastPosition, transform.position);
        lastPosition = transform.position;
        if (totalMoveDistance >= maxMoveDistance)
        {
            totalMoveDistance = 0;
            canCollision = true;
            ChangeDirection();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (canCollision && other.gameObject.CompareTag("Wall"))
        {
            canCollision = false;
            
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        moveDir *= -1;
    }
}
