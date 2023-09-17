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
    public WaitForSeconds effectTime = new WaitForSeconds(0.4f);

    public FootDustMaker dustMaker;
    public bool isTurning = false;
    public bool isPlayingFX = false;
    public float enterHoverCounter = 0f;

    bool wasDash = false;

    public override void Enter()
    {
        if(Mathf.Abs(kc.currentXVel) > 0f)
        {
            kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Walking" : "Char_Kirby_Inhaled_Walking");
        }
        else
        {
            kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Idle" : "Char_Kirby_Inhaled_Idle");
        }
    }

    public override void OnPrePhysCheck()
    {
        //관성 정지
        if (!isTurning && Mathf.Abs(kc.currentXVel) > 1 && Vector2.Dot(new Vector2(kc.currentXVel, 0f).normalized, new Vector2(kc.hInput, 0)) < 0f)
        {
            isTurning = true;
            kc.validDashInputTimer = 0f;
            kc.isRightDir = !kc.isRightDir;
            kc.lockDir = true;
        }

        //대쉬 조절
        if (isTurning && Mathf.Abs(kc.currentXVel) < 0.2f)
        {
            kc.currentXVel = 0f;
            isTurning = false;
            kc.lockDir = false;
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

    public override void OnWallHit()
    {
        if (Mathf.Abs(kc.currentXVel) > 0.05f)
        {
            kc.PlayStarDust();
            if (kc.hasInhaledObj) return;
            kc.PlayCollisionAnimation(2);
        }
    }

    public override void OnPostPhysCheck()
    {
        //관성 정지 스프라이트 체인지
        if (isTurning)
        {
            dustMaker.transform.localPosition = new Vector3(kc.isRightDir ? 0.25f : -0.25f, -0.5f, -0.15f);
            dustMaker.dustDir = new Vector3(kc.isRightDir ? -0.3f : 0.3f, 0.05f, 0f);
            dustMaker.gameObject.SetActive(true);
            kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Turning" : "Char_Kirby_Inhaled_Idle"); 
        }
        else
        {
            if (!isPlayingFX) dustMaker.gameObject.SetActive(false);
            if(Mathf.Abs(kc.currentXVel) > 0.05f)
            {
                kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Walking" : "Char_Kirby_Inhaled_Walking"); 
                kc.kirbyAnimator.SetFloat("WalkSpeed", Mathf.Abs(kc.currentXVel) / moveSpeed);
            }
            else
            {
                kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Idle" : "Char_Kirby_Inhaled_Idle"); 
            }
        }

        //대쉬 이펙트 재생
        if (!wasDash && kc.isDash)
        {
            StartCoroutine("PlayDashFX");
        }

        if (!kc.isGrounded)
        {
            //Falling 트랜지션
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        else
        {
            if(kc.vInput < 0f && !isTurning)
            {
                //Crouching 트랜지션
                kc.GetFSM.SwitchState("Crouch"); //작업필요
                return;
            }
        }

        //점프 트랜지션
        if (!isTurning && kc.jumpInput)
        {
            kc.GetFSM.SwitchState("Jump");
            return;
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

        if(!kc.hasInhaledObj && enterHoverCounter > 0.2f)
        {
            kc.GetFSM.SwitchState("Hover");
            return;
        }
        wasDash = kc.isDash;
    }

    public override void Excute()
    {
        var h = kc.hInput;

        if (isTurning)
        {
            h = 0f;
        }

        var setFriction = isTurning ? forceStopFriction : groundFriction;
        var maxSpeed = kc.isDash ? dashSpeed : moveSpeed;
        var inhaledScale = kc.hasInhaledObj ? 0.7f : 1f;

        kc.CalculateVelocity(ref kc.currentXVel, h, maxSpeed * inhaledScale, acceleration * inhaledScale, setFriction);
    }

    public override void Exit()
    {
        StopAllCoroutines();
        dustMaker.gameObject.SetActive(false);  
        kc.lockDir = false;
        isTurning = false;
        isPlayingFX = false;
        enterHoverCounter = 0f;
    }

    IEnumerator PlayDashFX()
    {
        isPlayingFX = true;
        dustMaker.gameObject.SetActive(true);
        dustMaker.transform.localPosition = new Vector3(kc.isRightDir ? -0.25f : 0.25f, -0.5f, -0.15f);
        dustMaker.dustDir = new Vector3(kc.isRightDir ? -0.3f : 0.3f, 0.05f, 0f);
        dustMaker.ImmediateActivate();
        yield return effectTime;
        isPlayingFX = false;
        dustMaker.gameObject.SetActive(false);
    }
}
