using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyHitbox : MonoBehaviour
{
    public KirbyController kc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!kc.isInvincibility 
            && !kc.isStopExcuteFSM 
            && (collision.gameObject.layer == 10 || collision.gameObject.layer == 9))
        {
            //damaged
            Gamemanager.instance.Damaged(1);
            kc.isRightDir = (collision.transform.position.x - transform.position.x) > 0 ? true : false;
            kc.StartCoroutine("LowDamaged");
        }
    }
}
