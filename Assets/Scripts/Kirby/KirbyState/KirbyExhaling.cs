using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyExhaling : KirbyState
{
    public GameObject starPrefab;
    public float shootSpeed = 6.0f;
    WaitForSeconds animTime = new WaitForSeconds(0.5f);

    public bool holdHinput;
    public float setDir;

    public override void Enter()
    {
        kc.lockDir = true;
        kc.hasInhaledObj = false;
        kc.ihObjAbility = 0;
        StartCoroutine("Exhaling");

        holdHinput = Mathf.Abs(kc.hInput) > 0;
        setDir = kc.hInput;
    }

    public override void Excute()
    {
        if(Mathf.Abs(kc.hInput) == 0f || kc.hInput != setDir)
        {
            holdHinput = false;
        }

        var h = holdHinput ? setDir : 0;

        kc.isDash = false;
        kc.CalculateVelocity(ref kc.currentXVel, h, 3, 24, 6);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;
    }

    public override void Exit()
    {
        StopAllCoroutines();
        kc.lockDir = false;
        holdHinput = false;
        setDir = 0;
    }

    IEnumerator Exhaling()
    {
        var projectile = Instantiate(starPrefab);
        projectile.transform.position = transform.position + Vector3.up * 0.25f;

        var projectileMove = projectile.GetComponent<ProjectileMovement>();
        projectileMove.dir = (kc.isRightDir ? 1 : -1) * Vector3.right;
        projectileMove.speed = shootSpeed;

        Destroy(projectile, 3f);
        kc.kirbyAnimator.Play("Char_Kirby_Exhaling_OnGround");

        yield return animTime;

        kc.GetFSM.SwitchState(kc.isGrounded ? "Idle" : "Fall");
    }
}
