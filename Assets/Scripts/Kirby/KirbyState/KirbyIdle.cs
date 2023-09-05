using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyIdle : KirbyState
{
    //통상 (지상 움직임)

    //움직임 속성
    public float groundFriction = 8f;
    public float forceStopFriction = 16f;
    public float acceleration = 24f;
    public float moveSpeed = 5.0f;
    public float dashSpeed = 6.0f;

    public bool isTurning = false;
    public float enterHoverCounter = 0f;

    public override void Enter()
    {

    }

    public override void OnWallHit()
    {
        kc.PlayCollisionAnimation(2);
    }

    public override void OnPostPhysCheck()
    {
        //관성 정지 스프라이트 체인지
        //대쉬 스프라이트 체인지

        if (!kc.isGrounded)
        {
            //Falling 트랜지션
            kc.GetFSM.SwitchState("Fall");
        }
        else
        {
            if(kc.vInput < 0f && !isTurning)
            {
                kc.GetFSM.SwitchState("Crouch");
            }
        }

        if (!isTurning && kc.jumpInput)
        {
            //점프 트랜지션
            kc.GetFSM.SwitchState("Jump");
        }

        //부풀기 트랜지션
        if (kc.vInput > 0f)
        {
            enterHoverCounter += Time.deltaTime;
        }
        else
        {
            enterHoverCounter = 0f;
        }

        if(enterHoverCounter > 0.2f)
        {
            kc.GetFSM.SwitchState("Hover");
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        //관성 정지
        if (Mathf.Abs(kc.currentXVel) > 1 && Vector2.Dot(new Vector2(kc.currentXVel,0f).normalized, new Vector2(h, 0)) < 0f)
        {
            isTurning = true;
            kc.validDashInputTimer = 0f;
        }

        if (isTurning)
        {
            h = 0f;
        }

        var setFriction = isTurning ? forceStopFriction : groundFriction;
        var maxSpeed = kc.isDash ? dashSpeed : moveSpeed;

        kc.CalculateXVelocity(h, maxSpeed,acceleration, setFriction);

        //대쉬 조절
        if (isTurning && Mathf.Abs(kc.currentXVel) < 0.2f)
        {
            kc.currentXVel = 0f;
            isTurning = false;
            if (kc.hInput == 0)
            {
                kc.isDash = false;
            }
        }
        else if (Mathf.Abs(kc.currentXVel) < 2f && kc.hInput == 0)
        {
            kc.isDash = false;
        }
    }

    public override void Exit()
    {
        isTurning = false;
        enterHoverCounter = 0f;
    }
}
