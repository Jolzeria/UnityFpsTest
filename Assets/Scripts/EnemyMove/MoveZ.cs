using UnityEngine;

public class MoveZ : BaseMove
{
    protected override void Init()
    {
        base.Init();
        
        moveDir = Vector3.forward;
    }

    protected override void MyFixedUpdate()
    {
        base.MyFixedUpdate();
        
        transform.Translate(moveDir * speed * Time.fixedDeltaTime, Space.World);
        // transform.rotation = Quaternion.LookRotation(moveDir);
    }
}