using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyFalling : KirbyState
{
    //공중 움직임

    //움직임 속성
    public float airAcceleration = 32f;
    public float airDecceleration = 8f;
    public float airMoveSpeed = 6f;
    public float gravityForce = 12f;

    public float currentYvel = 0f;
    public float inAirTime = 0f;

    public override void Enter()
    {
        currentYvel = kc.rb.velocity.y;
    }

    public override void OnLand()
    {
        //약한 점프, 스프라이트 애니메이션 재생
        if (currentYvel < -10.8f && inAirTime > 0.7f)
        {
            kc.isGrounded = false;
            currentYvel = 15f;
            kc.isDash = false;
            kc.lastTimeJumped = Time.time;
        }
    }

    public override void OnPostPhysCheck()
    {
        //지상 트랜지션
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }

        //부풀기 트랜지션
        if (kc.jumpInput || kc.vInput > 0)
        {
            kc.GetFSM.SwitchState("Hover");
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;
        inAirTime += Time.deltaTime;

        currentYvel = Mathf.Lerp(currentYvel, -gravityForce, Time.deltaTime * 4f);

        //가속
        kc.rb.velocity += new Vector2(h, 0f) * airAcceleration * Time.deltaTime;

        //감속
        var minus = kc.rb.velocity.x > 0 ? 1 : -1;

        kc.rb.velocity = new Vector2(minus * Mathf.Max(0f,Mathf.Abs(kc.rb.velocity.x) - airDecceleration * Time.deltaTime)
            ,kc.rb.velocity.y);

        //최수종
        kc.rb.velocity = new Vector2(Mathf.Clamp(kc.rb.velocity.x, -airMoveSpeed, airMoveSpeed), currentYvel);
    }

    public override void Exit()
    {
        currentYvel = 0f;
        inAirTime = 0f;
    }
}