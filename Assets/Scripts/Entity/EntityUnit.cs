using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityUnit : BeUnit
{
    protected override void RegisterEvent()
    {
        base.RegisterEvent();

        EventHandler.RegisterEvent<RaycastHit, SnapShot>(this, GameEventEnum.OnCollision, Collision);
    }

    public void Collision(RaycastHit hitInfo, SnapShot snapShot)
    {
        var damageInfo = new DamageInfo()
        {
            damage = 10,
            snapShot = snapShot,
            receiver = hitInfo.collider.GetComponentInParent<BeUnit>(),
            hitPoint = hitInfo.point
        };
        DamageManager.Instance.Add(damageInfo);
    }
}