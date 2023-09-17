using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyFireAction : KirbyState
{
    public GameObject fire;

    WaitForSeconds animTime1 = new WaitForSeconds(0.24f);
    WaitForSeconds animTime2 = new WaitForSeconds(0.36f);


    public override void Enter()
    {
        kc.isPlayingAction = true;
        kc.kirbyAnimator.Play("Char_Kirby_Action_ThrowDumbbell");
        kc.lockDir = true;
        StartCoroutine("FireAttack");
    }

    public override void Excute()
    {
        //캐릭터 물리
        kc.CalculateVelocity(ref kc.currentXVel, 0, 3, 24, 6);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;
    }

    public override void Exit()
    {
        StopAllCoroutines();
        kc.lockDir = false;
        kc.isPlayingAction = false;
    }

    IEnumerator FireAttack()
    {
        yield return animTime1;
        var fireObj = Instantiate(fire);
        fireObj.transform.position = transform.position;
        fireObj.transform.rotation = transform.rotation;
        fireObj.GetComponent<KDumbbellAtk>().dir = kc.isRightDir ? 1 : -1;
        yield return animTime2;

        kc.GetFSM.SwitchState(kc.isGrounded ? "Idle" : "Fall");
    }
}
