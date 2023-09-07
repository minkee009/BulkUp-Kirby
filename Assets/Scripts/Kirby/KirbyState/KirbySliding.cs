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

    public override void Enter()
    {
        kc.lockDir = true;
        kc.kirbyAnimator.Play("Char_Kirby_Sliding");
        kc.currentXVel = slideSpeed * (kc.isRightDir ? 1f : -1f);
    }

    public override void OnPostPhysCheck()
    {
        if (kc.CheckWallhit(kc.isRightDir))
        {
            kc.PlayCollisionAnimation(2);
            kc.currentXVel = 0f;
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
            return;
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
        kc.lockDir = false;
        slideTimer = 0f;
    }
}
