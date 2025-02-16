using UnityEngine;

public class MoveXHalfRound : BaseMove
{
    private float startTime;
    public float amplitude; // Y轴波动幅度
    public float frequency; // 波动频率

    protected override void Init()
    {
        base.Init();

        moveDir = Vector3.right;
        startTime = Time.time;
        amplitude = 5f;
        frequency = 3f;
    }

    protected override void MyFixedUpdate()
    {
        base.MyFixedUpdate();
        
        float x;
        float y;
        if (moveDir.x == 1f)
        {
            x = transform.position.x + speed * Time.fixedDeltaTime;
            y = Mathf.Abs(Mathf.Sin((Time.time - startTime) * frequency)) * amplitude;
        }
        else
        {
            x = transform.position.x - speed * Time.fixedDeltaTime;
            y = Mathf.Abs(Mathf.Sin((Time.time - startTime) * frequency)) * amplitude;
        }
        transform.position = new Vector3(x, y, transform.position.z);
        // transform.rotation = Quaternion.LookRotation(moveDir);
    }
}