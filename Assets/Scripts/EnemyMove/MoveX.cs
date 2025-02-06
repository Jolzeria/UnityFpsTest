using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveX : MonoBehaviour
{
    private float collisionInterval;
    private bool canCollision;
    private Vector3 moveDir;
    // 最大移动距离
    private float maxMoveDistance;
    // 总移动距离
    private float totalMoveDistance;
    private Vector3 lastPosition;
    
    public float speed;

    private void Start()
    {
        canCollision = true;
        collisionInterval = 1f;
        moveDir = Vector3.right;
        maxMoveDistance = 10f;
        totalMoveDistance = 0f;
        lastPosition = transform.position;
        speed = 10f;
    }

    private void FixedUpdate()
    {
        collisionInterval -= Time.fixedDeltaTime;
        if (collisionInterval <= 0)
        {
            collisionInterval = 1f;
            canCollision = true;
        }
        
        transform.Translate(moveDir * speed * Time.fixedDeltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(moveDir);

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
