using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyCrouching : KirbyState
{
    public float friction = 12f;
    public WaitForSeconds animTime = new WaitForSeconds(0.3f);
    public KirbyState[] actionStates;

    public bool playAnimation;
    public bool stopExcuteInput;

    public override void Enter()
    {
        if (kc.hasInhaledObj)
        {
            //»ïÅ°±â
            stopExcuteInput = true;
            StartCoroutine("Gulp");
        }
        else
        {
            //¾É±â
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
        //¸¶Âû
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;
    }

    public override void Exit()
    {
        StopAllCoroutines();
        playAnimation = false;
        stopExcuteInput = false;
    }

    //º¯½Å ÀÌÇà
    IEnumerator Gulp()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Crouching");
        kc.hitBox.enabled = false;
        StartCoroutine("PlayReaction");
        yield return animTime;

        kc.hitBox.enabled = true;
        kc.ability = kc.ihObjAbility;
        kc.hasInhaledObj = false;

        //´É·Â Ãß°¡
        //kc.GetFSM.AddState("Action", keystate);
        kc.GetFSM.SwitchState("Idle");
    }

    IEnumerator PlayReaction()
    {
        var count = 0f;
        while (count < 12f)
        {
            count += Time.deltaTime * 56f;

            kc.spritePivot.localPosition = Vector3.up * Mathf.Sin(count) * 0.05f;
            yield return null;
        }

        kc.spritePivot.localPosition = Vector3.zero;
    }

}
