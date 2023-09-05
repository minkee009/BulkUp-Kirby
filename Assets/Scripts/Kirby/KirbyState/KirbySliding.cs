using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbySliding : KirbyState
{
    public float slideTime = 0.6f;
    public float slideSpeed = 12f;
    public float friction = 20f;

    public float slideTimer = 0f;
    public bool currentDirIsRight = false;

    public override void Enter()
    {
        kc.currentXVel = slideSpeed * (kc.isRightDir ? 1f : -1f);
        currentDirIsRight = kc.isRightDir;
    }

    public override void OnPostPhysCheck()
    {
        if (kc.CheckWallhit(currentDirIsRight))
        {
            kc.PlayCollisionAnimation(2);
            kc.GetFSM.SwitchState("Idle");
        }
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
        }
        if (slideTimer > slideTime)
        {
            if(kc.vInput < 0)
            {
                kc.GetFSM.SwitchState("Crouch");
            }
            else
            {
                kc.GetFSM.SwitchState("Idle");
            }
            return;
        }
    }

    public override void Excute()
    {
        slideTimer += Time.deltaTime; 
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
    }

    public override void Exit()
    {
        slideTimer = 0f;
        currentDirIsRight = false;
    }
}
