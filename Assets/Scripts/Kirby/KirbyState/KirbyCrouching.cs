using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyCrouching : KirbyState
{
    public float friction = 12f;

    public override void Enter()
    {
        transform.localScale = new Vector3(1, 0.5f, 1);
        kc.rb.position += Vector2.down * transform.localScale.y * 0.5f;
    }

    public override void OnPostPhysCheck()
    {
        if (!kc.isGrounded)
        {
            kc.GetFSM.SwitchState("Fall");
            return;
        }
        if(kc.vInput >= 0f)
        {
            kc.GetFSM.SwitchState("Idle");
            return;
        }
        if(kc.actInput || kc.jumpInput)
        {
            kc.GetFSM.SwitchState("Slide");
        }
    }

    public override void Excute()
    {
        //¸¶Âû
        var minus = kc.currentXVel > 0 ? 1 : -1;
        kc.currentXVel = minus * Mathf.Max(0f, Mathf.Abs(kc.currentXVel) - friction * Time.deltaTime);
    }

    public override void Exit()
    {
        transform.position += Vector3.up * transform.localScale.y * 0.5f;
        kc.rb.position = new Vector2(transform.position.x, transform.position.y);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
