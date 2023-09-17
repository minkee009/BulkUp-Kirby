using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyCrouching : KirbyState
{
    public float friction = 12f;
    public WaitForSeconds animTime = new WaitForSeconds(0.3f);

    public bool playAnimation;
    public bool stopExcuteInput;

    public override void Enter()
    {
        if (kc.hasInhaledObj)
        {
            //ªÔ≈∞±‚
            stopExcuteInput = true;
            kc.lockDir = true;
            StartCoroutine("Gulp");
        }
        else
        {
            //æ…±‚
            kc.hitBox.size = new Vector2(kc.hitBox.size.x, 0.5f);
            kc.hitBox.offset = new Vector2(kc.hitBox.offset.x, -0.25f);
            kc.kirbyAnimator.Play("Char_Kirby_Crouching");
        }
    }

    public override void OnPostPhysCheck()
    {
        if (stopExcuteInput) return;
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        if (kc.vInput >= 0f)
        {
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if(kc.actInput || kc.jumpInput)
        {
            kc.GetFSM.SwitchState("Slide");
            return;
        }
    }

    public override void Excute()
    {
        //∏∂¬˚
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;
    }

    public override void Exit()
    {
        StopAllCoroutines();
        kc.hitBox.size = new Vector2(kc.hitBox.size.x, 1f);
        kc.hitBox.offset = new Vector2(kc.hitBox.offset.x, 0f);
        playAnimation = false;
        stopExcuteInput = false;
        kc.lockDir = false;
    }

    //∫ØΩ≈ ¿Ã«‡
    IEnumerator Gulp()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Swallow");
        kc.PlayReactionYdir();
        yield return animTime;
        kc.ChangeAbility();
    }

}
