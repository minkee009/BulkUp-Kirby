using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbySliding : KirbyState
{
    public float slideTime = 0.6f;
    public float slideSpeed = 12f;
    public float friction = 20f;

    public float slideTimer = 0f;

    public override void Enter()
    {
        kc.rb.velocity = slideSpeed * (kc.isRightDir ? 1f : -1f) * Vector2.right;
    }

    public override void OnHit()
    {
        base.OnHit();
    }

    public override void OnPostPhysCheck()
    {
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
        kc.rb.velocity = kc.rb.velocity.normalized * (Mathf.Max(kc.rb.velocity.magnitude - friction * Time.deltaTime, 0f));
    }

    public override void Exit()
    {
        slideTimer = 0f;
    }
}
