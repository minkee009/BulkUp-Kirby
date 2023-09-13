using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyBeamAction : KirbyState
{
    public GameObject beam;

    WaitForSeconds loopTime = new WaitForSeconds(0.1f);

    public override void Enter()
    {
        kc.isPlayingAction = true;
        kc.kirbyAnimator.Play("Char_Kirby_Action_Beam");
        kc.lockDir = true;
        StartCoroutine("BeamAttack");
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

    IEnumerator BeamAttack()
    {
        var angle = 0f;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam, transform);
            beamAttack.transform.position = transform.position + (kc.isRightDir ? 1 : -1) * Vector3.right * 0.5f + Vector3.up * 0.25f;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, 0.1f);

            if (!kc.isRightDir)
            {
                angle += 23.5f;
            }
            else
            {
                angle -= 23.5f;
            }

            yield return loopTime;
        }

        yield return loopTime;

        kc.GetFSM.SwitchState(kc.isGrounded ? "Idle" : "Fall");
    }
}
