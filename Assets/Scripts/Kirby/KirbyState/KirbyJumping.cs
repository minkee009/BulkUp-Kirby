using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KirbyJumping : KirbyState
{
    public float jumpSpeed = 12f;
    public float airAcceleration = 32f;
    public float airDecceleration = 8f;
    public float airMoveSpeed = 6f;
    public float airJumpTime = 0.2f;

    public float jumpTimer = 0;

    public override void Enter()
    {
        kc.rb.velocity += Vector2.up * 12f;
        kc.rb.velocity += new Vector2(kc.hInput, 0f) * airAcceleration * Time.deltaTime;
        kc.lastTimeJumped = Time.time;
    }

    public override void OnPostPhysCheck()
    {
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }
        if (!kc.jumpHoldInput || jumpTimer > airJumpTime)
        {
            kc.GetFSM.SwitchState("Fall");
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;
        jumpTimer += Time.deltaTime;

        //가속
        kc.rb.velocity += new Vector2(h, 0f) * airAcceleration * Time.deltaTime;

        //감속
        kc.rb.velocity += -kc.rb.velocity.normalized * airDecceleration * Time.deltaTime;

        //최수종
        kc.rb.velocity = new Vector2(Mathf.Clamp(kc.rb.velocity.x, -airMoveSpeed, airMoveSpeed), jumpSpeed);
    }

    public override void Exit()
    {
        jumpTimer = 0f;
    }
}
