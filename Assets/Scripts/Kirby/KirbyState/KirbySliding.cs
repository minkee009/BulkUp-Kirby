using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbySliding : KirbyState
{
    public float slideTime = 0.6f;
    public float slideSpeed = 12f;
    public float friction = 20f;

    public float slideTimer = 0f;
    public FootDustMaker dustmaker;

    public override void Enter()
    {
        dustmaker.gameObject.SetActive(true);
        dustmaker.transform.localPosition = new Vector3(kc.isRightDir ? -0.5f : 0.5f, -0.5f, 0f);
        dustmaker.dustDir = new Vector3(kc.isRightDir ? -0.3f : 0.3f, 0.05f, 0f);
        kc.lockDir = true;
        kc.hitBox.size = new Vector2(0.5f, 0.5f);
        kc.hitBox.offset = new Vector2(kc.isRightDir ? -.25f : .25f, -0.25f);
        kc.atkBox.enabled = true;
        
        kc.kirbyAnimator.Play("Char_Kirby_Sliding");
        kc.currentXVel = slideSpeed * (kc.isRightDir ? 1f : -1f);
    }

    public override void OnPostPhysCheck()
    {
        if (kc.CheckWallhit(kc.isRightDir))
        {
            kc.PlayCollisionAnimation(2);
            kc.PlayStarDust();
            kc.currentXVel = 0f;
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        if (slideTimer > slideTime)
        {
            if(kc.vInput < 0)
            {
                kc.GetFSM.SwitchState("Crouch");
            }
            else
            {
                kc.GetFSM.SwitchState("Idle");
            }
            return;
        }
    }

    public override void Excute()
    {
        slideTimer += Time.deltaTime; 
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
        
    }

    public override void Exit()
    {
        dustmaker.gameObject.SetActive(false);
        kc.hitBox.size = new Vector2(1f, 1f);
        kc.hitBox.offset = new Vector2(0f, 0f);
        kc.lockDir = false;
        slideTimer = 0f;
    }
}
