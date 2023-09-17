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
    public float gravityForce = 8f;
    public float gravityAccel = 12f;
    public float fallattackTime = 0.8f;
    public float landJumpForce = 15f;
    public bool stopExcuteXAccel;

    public float inAirTime = 0f;

    public override void Enter()
    {
        if(!kc.kirbyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Char_Kirby_Falling"))
        {
            kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Falling_Middle" : "Char_Kirby_Inhaled_Falling"); //작업필요
        }
    }

    public override void OnLand()
    {
        kc.PlayStarDust();
        //약한 점프, 스프라이트 애니메이션 재생
        if (!kc.hasInhaledObj && kc.currentYVel < -gravityForce + 0.01f && inAirTime > fallattackTime)
        {
            inAirTime = 0f;
            interactActionInput = false;
            kc.kirbyAnimator.Play("Char_Kirby_Falling_End", -1,0f);
            kc.isGrounded = false;
            kc.currentYVel = landJumpForce;
            kc.isDash = false;
            kc.lastTimeJumped = Time.time;
            stopExcuteXAccel = true;
            kc.lockDir = true;
        }
        else if(kc.currentYVel < 0.05f)
        {
            interactActionInput = true;
            if (kc.hasInhaledObj) return;
            kc.PlayCollisionAnimation(0);
        }
    }

    public override void OnPrePhysCheck()
    {
        if (inAirTime > fallattackTime && kc.currentYVel < -gravityForce + 0.01f)
        {
            if (kc.hasInhaledObj) return;
            kc.kirbyAnimator.Play("Char_Kirby_Falling_End");
        }
    }

    public override void OnWallHit()
    {
        if (Mathf.Abs(kc.currentXVel) > 0.05f)
        {
            if (kc.hasInhaledObj) return;

            kc.PlayStarDust();
            kc.PlayCollisionAnimation(2);
        }  
    }

    public override void OnCellingHit()
    {

        kc.PlayStarDust();
        kc.currentYVel = 0f;
        if (kc.hasInhaledObj) return;
        kc.PlayCollisionAnimation(1);
    }

    public override void OnPostPhysCheck()
    {
        //지상 트랜지션
        if (kc.isGrounded)
        {
            kc.currentYVel = 0f;
            kc.GetFSM.SwitchState("Idle");
            return;
        }

        //부풀기 트랜지션
        if (!kc.hasInhaledObj && (kc.vInput > 0))
        {
            kc.GetFSM.SwitchState("Hover");
            return;
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;
        inAirTime += Time.deltaTime;
        var inhaledScale = kc.hasInhaledObj ? 0.7f : 1f;
        
        if(!stopExcuteXAccel)
            kc.CalculateVelocity(ref kc.currentXVel,h,airMoveSpeed * inhaledScale, airAcceleration * inhaledScale, airDecceleration);
        else
            kc.CalculateVelocity(ref kc.currentXVel, 0, airMoveSpeed, airAcceleration * inhaledScale, airDecceleration);

        kc.CalculateVelocity(ref kc.currentYVel, -1, gravityForce, gravityAccel, 0f);
    }

    public override void Exit()
    {
        interactActionInput = true;
        kc.lockDir = false;
        stopExcuteXAccel = false;
        inAirTime = 0f;
    }
}