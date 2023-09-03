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

    public override void Enter()
    {
        
    }

    public override void OnPostPhysCheck()
    {
        //���� ���� ��������Ʈ ü����
        //�뽬 ��������Ʈ ü����

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

        //��Ǯ�� Ʈ������
    }

    public override void Excute()
    {
        var h = kc.hInput;

        var nonYvel = new Vector2(kc.rb.velocity.x, 0f);

        //���� ����
        if (nonYvel.magnitude > 1 && Vector2.Dot(nonYvel.normalized, new Vector2(h, 0)) < 0f)
        {
            isTurning = true;
            kc.validDashInputTimer = 0f;
        }

        h = isTurning ? 0f : h;

        var setFriction = isTurning ? forceStopFriction : groundFriction;

        //����
        kc.rb.velocity += (kc.isDash ? 1.2f : 1f) * acceleration * Time.deltaTime * new Vector2(h, 0f);

        //����
        kc.rb.velocity = kc.rb.velocity.normalized * (Mathf.Max(kc.rb.velocity.magnitude - setFriction * Time.deltaTime, 0f));
        var maxSpeed = kc.isDash ? dashSpeed : moveSpeed;
        kc.rb.velocity = Vector2.ClampMagnitude(kc.rb.velocity, maxSpeed);

        //�뽬 ����
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
            //���� Ʈ������
            kc.GetFSM.SwitchState("Jump");
        }
    }

    public override void Exit()
    {
        isTurning = false;
    }
}
