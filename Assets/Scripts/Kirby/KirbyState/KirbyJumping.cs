using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyJumping : KirbyState
{
    public float jumpSpeed = 8f;
    public float airAcceleration = 32f;
    public float airDecceleration = 8f;
    public float airMoveSpeed = 6f;
    public float airJumpTime = 0.2f;
    public float gravityForce = 8f;
    public float gravityAccel = 12f;

    public float jumpTimer = 0;

    public override void Enter()
    {
        kc.kirbyAnimator.Play(!kc.hasInhaledObj ? "Char_Kirby_Jumping" : "Char_Kirby_Inhaled_Jumping"); //작업필요
        kc.currentYVel = jumpSpeed;
        kc.lastTimeJumped = Time.time;
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

    public override void OnCellingHit()
    {
        kc.currentYVel = 0f;

        kc.PlayStarDust();
        kc.GetFSM.SwitchState("Fall");
        if (kc.hasInhaledObj) return;
        kc.PlayCollisionAnimation(1);
    }

    public override void OnPostPhysCheck()
    {
        if (!kc.hasInhaledObj && (kc.jumpInput || kc.vInput > 0f))
        {
            kc.GetFSM.SwitchState("Hover");
            return;
        }
        if (jumpTimer > 0.02f && kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if (!kc.jumpHoldInput || kc.currentYVel <= 0f)
        {
            if (!kc.hasInhaledObj)
                kc.kirbyAnimator.Play("Char_Kirby_Falling");
            kc.kirbyAnimator.Update(0f);
            kc.currentYVel = Mathf.Lerp(kc.currentYVel,0f,0.4f);
            kc.GetFSM.SwitchState("Fall");
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        jumpTimer += Time.deltaTime;
        var inhaledScale = kc.hasInhaledObj ? 0.7f : 1f;
        kc.CalculateVelocity(ref kc.currentXVel,h,airMoveSpeed * inhaledScale, airAcceleration * inhaledScale, airDecceleration);
        kc.CalculateVelocity(ref kc.currentYVel, -1, gravityForce, gravityAccel, 0f);
    }

    public override void Exit()
    {
        jumpTimer = 0f;
    }
}
