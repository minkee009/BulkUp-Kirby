using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyIdle : KirbyState
{
    //��� (���� ������)

    //������ �Ӽ�
    public float groundFriction = 8f;
    public float forceStopFriction = 16f;
    public float acceleration = 24f;
    public float moveSpeed = 5.0f;
    public float dashSpeed = 6.0f;

    public bool isTurning = false;
    public float enterHoverCounter = 0f;

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
        //���� ����
        if (!isTurning && Mathf.Abs(kc.currentXVel) > 1 && Vector2.Dot(new Vector2(kc.currentXVel, 0f).normalized, new Vector2(kc.hInput, 0)) < 0f)
        {
            isTurning = true;
            kc.validDashInputTimer = 0f;
            kc.isRightDir = !kc.isRightDir;
            kc.lockDir = true;
        }

        //�뽬 ����
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
            if (kc.hasInhaledObj) return;
            kc.PlayCollisionAnimation(2);
        }
    }

    public override void OnPostPhysCheck()
    {

        //���� ���� ��������Ʈ ü����
        if (isTurning)
        {
            kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Turning" : "Char_Kirby_Inhaled_Idle"); 
        }
        else
        {
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

        //�뽬 ����Ʈ ���
        if (kc.isDash)
        {
            
        }
        else
        {

        }

        if (!kc.isGrounded)
        {
            //Falling Ʈ������
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        else
        {
            if(kc.vInput < 0f && !isTurning)
            {
                //Crouching Ʈ������
                kc.GetFSM.SwitchState("Crouch"); //�۾��ʿ�
                return;
            }
        }

        //���� Ʈ������
        if (!isTurning && kc.jumpInput)
        {
            kc.GetFSM.SwitchState("Jump");
            return;
        }

        //��Ǯ�� Ʈ������
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
        kc.lockDir = false;
        isTurning = false;
        enterHoverCounter = 0f;
    }
}
