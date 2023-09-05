using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KirbyHovering : KirbyState
{
    public float hoverAcceleration = 24f;
    public float hoverDecceleration = 12f;
    public float hoverMoveSpeed = 4f;

    public float hoverJumpSpeed = 3f;
    public float hoverGravity = 4f;
    WaitForSeconds jumpInputWaitTime = new WaitForSeconds(0.3f);

    public bool playAnimation = false;
    public bool goingJump = false;

    public override void Enter()
    {
        kc.ForceStopCollisionAnimation();
        StartCoroutine(WaitForAnimation());
        StartCoroutine(HoverJump());
    }

    public override void OnPostPhysCheck()
    {
        if (!playAnimation && !goingJump && (kc.vInput > 0 || kc.jumpHoldInput))
        {
            StartCoroutine(HoverJump());
        }

        if (!playAnimation && kc.actInput)
        {
            kc.currentYVel = 0f;
            StartCoroutine(WaitForExit());
        }

        if (kc.isGrounded)
        {
            kc.currentYVel = 0f;
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        if(goingJump)
        {
            kc.currentYVel = hoverJumpSpeed;
        }
        else
        {
            kc.CalculateYVelocity(hoverGravity, 4f);
        }

        kc.CalculateXVelocity(h, hoverMoveSpeed, hoverAcceleration, hoverDecceleration);
    }

    public override void Exit()
    {
        StopAllCoroutines();
        kc.currentYVel = 0f;
        goingJump = false;
        playAnimation = false;
    }

    IEnumerator HoverJump()
    {
        goingJump = true;
        yield return jumpInputWaitTime;
        goingJump = false;
    }

    IEnumerator WaitForAnimation()
    {
        playAnimation = true;
        yield return jumpInputWaitTime;
        playAnimation = false;
    }

    IEnumerator WaitForExit()
    {
        playAnimation = true;
        yield return jumpInputWaitTime;
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }
        else
        {
            kc.GetFSM.SwitchState("Fall");
        }
        playAnimation = false;
    }
}
