using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbySparkAction : KirbyState
{
    public GameObject sparkFX;
    public float attackTime = 0.6f;
    public float timer = 0;
    public bool isAttacking;
    public bool isEndAnim;

    public override void Enter()
    {
        kc.isPlayingAction = true;
        kc.kirbyAnimator.Play("Char_Kirby_Action_SparkReady");
        kc.lockDir = true;
    }

    public override void OnPrePhysCheck()
    {
        if(!isEndAnim && timer == attackTime && !kc.actHoldInput)
        {
            StartCoroutine("ExitAnimation");
        }
    }

    public override void Excute()
    {
        timer = Mathf.Min(attackTime, timer + Time.deltaTime);

        //캐릭터 물리
        kc.CalculateVelocity(ref kc.currentXVel, 0, 3, 24, 6);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;

        if (!isAttacking && timer > 0.1f)
        {
            kc.kirbyAnimator.Play("Char_Kirby_Action_Spark");
            isAttacking = true;
            sparkFX.SetActive(true);
            kc.hitBox.enabled = false;
            kc.atkBox.enabled = true;
            kc.atkBox.offset = new Vector2(0f, 0f);
            kc.atkBox.size = new Vector2(2f, 2f);
        }
    }

    public override void Exit()
    {
        StopAllCoroutines();
        timer = 0;
        kc.lockDir = false;
        kc.isPlayingAction = false;
        isAttacking = false;
        kc.hitBox.enabled = true;
        kc.atkBox.enabled = false;
        sparkFX.SetActive(false);
        isEndAnim = false;
    }

    IEnumerator ExitAnimation()
    {
        isEndAnim = true;
        kc.hitBox.enabled = true;
        kc.atkBox.enabled = false;
        sparkFX.SetActive(false);
        kc.kirbyAnimator.Play("Char_Kirby_Action_SparkReady");
        kc.kirbyAnimator.Update(0f);
        yield return new WaitForSeconds(0.2f);
        kc.GetFSM.SwitchState("Idle");
        isEndAnim = false;
    }
}
