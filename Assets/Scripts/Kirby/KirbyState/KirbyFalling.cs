using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyFalling : KirbyState
{
    //���� ������

    //������ �Ӽ�
    public float airAcceleration = 32f;
    public float airDecceleration = 8f;
    public float airMoveSpeed = 6f;
    public float gravityForce = 12f;

    public bool isWallhit = false;
    public float inAirTime = 0f;

    public override void Enter()
    {
        isWallhit = kc.CheckWallhit(kc.isRightDir);
    }

    public override void OnLand()
    {
        //���� ����, ��������Ʈ �ִϸ��̼� ���
        if (kc.currentYVel < -10.8f && inAirTime > 0.7f)
        {
            kc.isGrounded = false;
            kc.currentYVel = 15f;
            kc.isDash = false;
            kc.lastTimeJumped = Time.time;
        }
        else
        {
            kc.PlayCollisionAnimation(0);
        }
    }

    public override void OnWallHit()
    {
        kc.PlayCollisionAnimation(2);
    }

    public override void OnCellingHit()
    {
        kc.currentYVel = 0f;
        kc.PlayCollisionAnimation(1);
    }

    public override void OnPostPhysCheck()
    {
        var wasWallhit = isWallhit;
        isWallhit = kc.CheckWallhit(kc.isRightDir);
        if (!wasWallhit && isWallhit)
        {
            kc.PlayCollisionAnimation(2);
        }

        //���� Ʈ������
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }

        //��Ǯ�� Ʈ������
        if (kc.jumpInput || kc.vInput > 0)
        {
            kc.GetFSM.SwitchState("Hover");
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
        isWallhit = false;
    }
}