using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyJumping : KirbyState
{
    public float jumpSpeed = 12f;
    public float airAcceleration = 32f;
    public float airDecceleration = 8f;
    public float airMoveSpeed = 6f;
    public float airJumpTime = 0.2f;
    public float airSlowDownSpeed = 15f;

    public bool slowDownJump = false;
    public float jumpTimer = 0;

    public override void Enter()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Jumping");
        kc.rb.velocity += Vector2.up * 12f;
        kc.rb.velocity += new Vector2(kc.hInput, 0f) * airAcceleration * Time.deltaTime;
        kc.lastTimeJumped = Time.time;
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
        kc.GetFSM.SwitchState("Fall");
    }

    public override void OnPostPhysCheck()
    {
        if (slowDownJump && (kc.jumpInput || kc.vInput > 0f))
        {
            kc.GetFSM.SwitchState("Hover");
        }
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }
        if (!kc.jumpHoldInput || jumpTimer > airJumpTime)
        {
            slowDownJump = true;
        }
        if (kc.currentYVel < 0.2f)
        {
            kc.playJumpTurn = true;
            kc.GetFSM.SwitchState("Fall");
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        jumpTimer += Time.deltaTime;

        if (slowDownJump)
        {
            kc.CalculateYVelocity(0f, airSlowDownSpeed);
        }
        else
        {
            kc.currentYVel = jumpSpeed;
        }
        kc.CalculateXVelocity(h,airMoveSpeed, airAcceleration, airDecceleration);
    }

    public override void Exit()
    {
        slowDownJump = false;
        jumpTimer = 0f;
    }
}
