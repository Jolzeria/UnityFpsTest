using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Jump.canJump = true;
    }
}
