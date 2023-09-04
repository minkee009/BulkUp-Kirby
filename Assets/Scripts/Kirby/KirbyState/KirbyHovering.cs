using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KirbyHovering : KirbyState
{
    public float hoverAcceleration = 24f;
    public float hoverDecceleration = 12f;
    public float hoverMoveSpeed = 4f;

    public float hoverJumpSpeed = 3f;
    public float hoverGravity = 4f;
    WaitForSeconds jumpInputWaitTime = new WaitForSeconds(0.3f);

    public bool playAnimation = false;
    public float currentYVel = 0f; 
    public bool goingJump = false;

    public override void Enter()
    {
        StartCoroutine(WaitForAnimation());
        StartCoroutine(HoverJump());
    }

    public override void OnPostPhysCheck()
    {
        if (!playAnimation && !goingJump && (kc.vInput > 0 || kc.jumpHoldInput))
        {
            StartCoroutine(HoverJump());
        }

        if (!playAnimation && kc.actInput)
        {
            kc.rb.velocity = new Vector2(kc.rb.velocity.x, 0f);
            StartCoroutine(WaitForExit());
        }

        if (kc.isGrounded)
        {
            currentYVel = 0f;
            kc.rb.velocity = new Vector2(kc.rb.velocity.x, currentYVel);
        }
    }

    public override void Excute()
    {
        var h = kc.hInput;

        if(goingJump)
        {
            currentYVel = hoverJumpSpeed;
        }
        else
        {
            currentYVel = Mathf.Lerp(currentYVel,-hoverGravity,4f * Time.deltaTime);
        }
        //가속
        kc.rb.velocity += new Vector2(h, 0f) * hoverAcceleration * Time.deltaTime;

        //감속
        var minus = kc.rb.velocity.x > 0 ? 1 : -1;
        kc.rb.velocity = new Vector2(minus * Mathf.Max(0f, Mathf.Abs(kc.rb.velocity.x) - hoverDecceleration * Time.deltaTime)
            , kc.rb.velocity.y);

        //최수종
        kc.rb.velocity = new Vector2(Mathf.Clamp(kc.rb.velocity.x, -hoverMoveSpeed, hoverMoveSpeed), currentYVel);
    }

    public override void Exit()
    {
        StopAllCoroutines();
        currentYVel = 0f;
        goingJump = false;
        playAnimation = false;
    }

    IEnumerator HoverJump()
    {
        goingJump = true;
        yield return jumpInputWaitTime;
        goingJump = false;
    }

    IEnumerator WaitForAnimation()
    {
        playAnimation = true;
        yield return jumpInputWaitTime;
        playAnimation = false;
    }

    IEnumerator WaitForExit()
    {
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
}
