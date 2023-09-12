using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyBeamAction : KirbyState
{
    public override void Enter()
    {
        Debug.Log("ปัม๗");
        kc.GetFSM.SwitchState("Idle");
    }

    public override void Excute()
    {
        kc.GetFSM.SwitchState("Idle");
    }

    public override void Exit()
    {
        
    }
}
