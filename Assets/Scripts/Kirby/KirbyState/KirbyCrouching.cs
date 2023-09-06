using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyCrouching : KirbyState
{
    public float friction = 12f;

    public override void Enter()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Crouching");
    }

    public override void OnPostPhysCheck()
    {
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        if(kc.vInput >= 0f)
        {
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if(kc.actInput || kc.jumpInput)
        {
            kc.GetFSM.SwitchState("Slide");
            return;
        }
    }

    public override void Excute()
    {
        //¸¶Âû
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
    }

    public override void Exit()
    {

    }
}
