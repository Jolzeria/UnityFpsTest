using UnityEngine;

public class MoveStraight : BaseMove
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void MyFixedUpdate()
    {
        base.MyFixedUpdate();
        
        transform.Translate(moveDir * speed * Time.fixedDeltaTime, Space.World);
        // transform.rotation = Quaternion.LookRotation(moveDir);
    }
}
