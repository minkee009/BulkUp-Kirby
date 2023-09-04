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
        kc.rb.velocity = kc.rb.velocity.normalized * (Mathf.Max(kc.rb.velocity.magnitude - friction * Time.deltaTime, 0f));
    }

    public override void Exit()
    {
        transform.position += Vector3.up * transform.localScale.y * 0.5f;
        kc.rb.position = new Vector2(transform.position.x, transform.position.y);
        transform.localScale = new Vector3(1, 1f, 1);
    }
}
