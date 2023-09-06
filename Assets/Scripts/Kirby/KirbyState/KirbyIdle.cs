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
        kc.kirbyAnimator.Play("Char_Kirby_Idle");
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
        if(Mathf.Abs(kc.currentXVel) > 0.05f)
        {
            kc.PlayCollisionAnimation(2);
        }
    }

    public override void OnPostPhysCheck()
    {

        //���� ���� ��������Ʈ ü����
        if (isTurning)
        {
            kc.kirbyAnimator.Play("Char_Kirby_Turning");
        }
        else
        {
            if(Mathf.Abs(kc.currentXVel) > 0.05f)
            {
                kc.kirbyAnimator.Play("Char_Kirby_Walking");
                kc.kirbyAnimator.SetFloat("WalkSpeed", Mathf.Abs(kc.currentXVel) / moveSpeed);
            }
            else
            {
                kc.kirbyAnimator.Play("Char_Kirby_Idle");
            }
            
        }

        if (kc.isDash)
        {
            //�뽬 ����Ʈ ���
        }

        if (!kc.isGrounded)
        {
            //Falling Ʈ������
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
            //���� Ʈ������
            kc.GetFSM.SwitchState("Jump");
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

        if(enterHoverCounter > 0.2f)
        {
            kc.GetFSM.SwitchState("Hover");
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

        kc.CalculateXVelocity(h, maxSpeed,acceleration, setFriction);
    }

    public override void Exit()
    {
        kc.lockDir = false;
        isTurning = false;
        enterHoverCounter = 0f;
    }
}
