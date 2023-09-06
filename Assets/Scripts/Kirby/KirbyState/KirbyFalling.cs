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
    public float fallAttackSpeed = 10.8f;
    public float landJumpForce = 15f;

    public float inAirTime = 0f;

    public override void Enter()
    {
        if(!kc.kirbyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Char_Kirby_Falling"))
        {
            kc.kirbyAnimator.Play("Char_Kirby_Falling_Middle");
        }
    }

    public override void OnLand()
    {
        //약한 점프, 스프라이트 애니메이션 재생
        if (kc.currentYVel < -fallAttackSpeed && inAirTime > 0.4f)
        {
            kc.kirbyAnimator.Play("Char_Kirby_Falling",-1,0f);
            kc.isGrounded = false;
            kc.currentYVel = landJumpForce;
            kc.isDash = false;
            kc.lastTimeJumped = Time.time;
        }
        else if(kc.currentYVel < 0.05f)
        {
            kc.PlayCollisionAnimation(0);
        }
    }

    public override void OnPrePhysCheck()
    {
        if(inAirTime > 0.5f && kc.currentYVel < -fallAttackSpeed)
        {
            kc.kirbyAnimator.Play("Char_Kirby_Falling_End");
        }
    }

    public override void OnWallHit()
    {
        if (Mathf.Abs(kc.currentXVel) > 0.05f)
        {
            kc.PlayCollisionAnimation(2);
        }  
    }

    public override void OnCellingHit()
    {
        kc.currentYVel = 0f;
        kc.PlayCollisionAnimation(1);
    }

    public override void OnPostPhysCheck()
    {
        //지상 트랜지션
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
            return;
        }

        //부풀기 트랜지션
        if (kc.jumpInput || kc.vInput > 0)
        {
            kc.GetFSM.SwitchState("Hover");
            return;
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;
        inAirTime += Time.deltaTime;

        kc.CalculateXVelocity(h,airMoveSpeed, airAcceleration, airDecceleration);
        kc.CalculateYVelocity(gravityForce, 4);
    }

    public override void Exit()
    {
        kc.currentYVel = 0f;
        inAirTime = 0f;
    }
}