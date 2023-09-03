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

    public override void Enter()
    {
        
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

        //부풀기 트랜지션
    }

    public override void Excute()
    {
        var h = kc.hInput;

        var nonYvel = new Vector2(kc.rb.velocity.x, 0f);

        //관성 정지
        if (nonYvel.magnitude > 1 && Vector2.Dot(nonYvel.normalized, new Vector2(h, 0)) < 0f)
        {
            isTurning = true;
            kc.validDashInputTimer = 0f;
        }

        h = isTurning ? 0f : h;

        var setFriction = isTurning ? forceStopFriction : groundFriction;

        //가속
        kc.rb.velocity += (kc.isDash ? 1.2f : 1f) * acceleration * Time.deltaTime * new Vector2(h, 0f);

        //마찰
        kc.rb.velocity = kc.rb.velocity.normalized * (Mathf.Max(kc.rb.velocity.magnitude - setFriction * Time.deltaTime, 0f));
        var maxSpeed = kc.isDash ? dashSpeed : moveSpeed;
        kc.rb.velocity = Vector2.ClampMagnitude(kc.rb.velocity, maxSpeed);

        //대쉬 조절
        if (isTurning && kc.rb.velocity.magnitude < 0.2f)
        {
            kc.rb.velocity = Vector2.zero;
            isTurning = false;
            if (kc.hInput == 0)
            {
                kc.isDash = false;
            }
        }
        else if (kc.rb.velocity.magnitude < 2f && kc.hInput == 0)
        {
            kc.isDash = false;
        }

        if (!isTurning && kc.jumpInput)
        {
            //점프 트랜지션
            kc.GetFSM.SwitchState("Jump");
        }
    }

    public override void Exit()
    {
        isTurning = false;
    }
}
