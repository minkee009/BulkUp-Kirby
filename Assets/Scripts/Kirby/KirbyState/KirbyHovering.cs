using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KirbyHovering : KirbyState
{
    public float hoverAcceleration = 24f;
    public float hoverDecceleration = 12f;
    public float hoverMoveSpeed = 4f;
    public GameObject airBlastPrefab;
    public float hoverJumpSpeed = 3f;
    public float hoverGravity = 4f;
    WaitForSeconds jumpInputWaitTime = new WaitForSeconds(0.3f);

    public bool playAnimation = false;
    public bool goingJump = false;

    public override void Enter()
    {
        kc.isDash = false;
        kc.dontUseDashInput = true;
        kc.ForceStopCollisionAnimation();
        StartCoroutine("WaitForAnimation");
    }

    public override void OnPostPhysCheck()
    {

        if (!playAnimation && kc.actInput)
        {
            StartCoroutine("WaitForExit");
            return;
        }

        if (!playAnimation && !goingJump && (kc.vInput > 0 || kc.jumpHoldInput))
        {
            StartCoroutine("HoverJump");
        }

        if (kc.isGrounded)
        {
            kc.currentYVel = 0f;
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        if(goingJump)
        {
            kc.currentYVel = hoverJumpSpeed;
        }
        else
        {
            kc.CalculateVelocity(ref kc.currentYVel,-1,hoverGravity, 8f, 0f);
        }

        kc.CalculateVelocity(ref kc.currentXVel,h, hoverMoveSpeed, hoverAcceleration, hoverDecceleration);
    }

    public override void Exit()
    {
        kc.dontUseDashInput = false;
        StopCoroutine("HoverJump");
        StopCoroutine("WaitForAnimation");
        StopCoroutine("WaitForExit");
        goingJump = false;
        playAnimation = false;
    }

    IEnumerator HoverJump()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Inhaled_Flying");
        goingJump = true;
        yield return jumpInputWaitTime;
        goingJump = false;
        if(!playAnimation) 
            kc.kirbyAnimator.Play("Char_Kirby_Inhaled_Hovering");
    }

    IEnumerator WaitForAnimation()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Inhaling_OnSky");
        playAnimation = true;
        goingJump = true;
        yield return jumpInputWaitTime;
        playAnimation = false;
        goingJump = false;
        kc.kirbyAnimator.Play("Char_Kirby_Inhaled_Hovering");
    }

    IEnumerator WaitForExit()
    {
        CreateAirBlast();
        kc.kirbyAnimator.Play("Char_Kirby_exhaling_OnSky");
        playAnimation = true;
        yield return jumpInputWaitTime;
        if (kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Idle");
        }
        else
        {
            kc.GetFSM.SwitchState("Fall");
        }
        playAnimation = false;
    }

    ProjectileMovement CreateAirBlast()
    {
        var projectile = Instantiate(airBlastPrefab);
        projectile.transform.position = transform.position + Vector3.up * 0.25f;

        var projectileMove = projectile.GetComponent<ProjectileMovement>();
        projectileMove.dir = (kc.isRightDir ? 1 : -1) * Vector3.right;
        projectileMove.speed += Mathf.Abs(kc.currentXVel);
        projectileMove.slowDownSpeed = 8f;
        projectileMove.slowDownDestroy = true;

        var projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.spriteRender.flipX = kc.isRightDir ? false : true;

        return projectileMove;
    }
}
