using System;
using UnityEngine;

public enum MoveDirection
{
    forward,
    back,
    right,
    left
}

public class BaseMove : MonoBehaviour
{
    // 限制碰撞间隔
    protected bool canCollision;
    protected float collisionInterval;

    // 限制移动距离
    protected bool moveDistanceSwitch;
    protected float maxMoveDistance;
    protected float totalMoveDistance;
    protected Vector3 lastPosition;

    // 移动方向
    public Vector3 moveDir;

    // 移动速度
    public float speed;

    // 是否启用撞墙销毁
    public bool enableCollisionMode;
    // 撞几次墙销毁
    public int collisionLimit;
    private int collisionCount;

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        MyFixedUpdate();
    }

    protected virtual void Init()
    {
        canCollision = true;
        collisionInterval = 1f;
        moveDistanceSwitch = false;
        maxMoveDistance = 10f;
        totalMoveDistance = 0f;
        lastPosition = transform.position;
        
        collisionCount = 1;
    }

    protected virtual void MyFixedUpdate()
    {
        collisionInterval -= Time.fixedDeltaTime;
        if (collisionInterval <= 0)
        {
            collisionInterval = 1f;
            canCollision = true;
        }

        totalMoveDistance += Vector3.Distance(lastPosition, transform.position);
        lastPosition = transform.position;
        if (moveDistanceSwitch && totalMoveDistance >= maxMoveDistance)
        {
            totalMoveDistance = 0;
            ChangeDirection();
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        
        if (canCollision && other.gameObject.CompareTag("TargetMoveLimit"))
        {
            if (enableCollisionMode && collisionCount >= collisionLimit)
            {
                TargetSpawnManager.Instance.Release(gameObject);
                return;
            }
            
            canCollision = false;
            ChangeDirection();
            collisionCount += 1;
        }
    }

    protected virtual void ChangeDirection()
    {
        moveDir *= -1;
    }
}