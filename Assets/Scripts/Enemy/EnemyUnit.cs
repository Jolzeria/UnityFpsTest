using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BeUnit
{
    protected override void Init()
    {
        base.Init();

        attribute = new EnemyAttribute();
        attribute.Init();
    }

    protected override void UnInit()
    {
        base.UnInit();

        attribute.UnInit();
    }
}
