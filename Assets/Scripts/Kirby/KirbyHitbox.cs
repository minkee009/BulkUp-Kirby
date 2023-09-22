using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyHitbox : MonoBehaviour
{
    public KirbyController kc;
    public LayerMask kirbyHitMask;

    int _debugDamage = 1;
    bool _debugMode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _debugMode = !_debugMode;
            _debugDamage = _debugMode ? 0 : 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (!kc.isInvincibility 
            && !kc.isStopExcuteFSM 
            && ((1 << collision.gameObject.layer & kirbyHitMask) > 0))
        {
            //damaged
            Gamemanager.instance.Damaged(_debugDamage);
            Gamemanager.instance.cameraMove?.ShakeCamera(15f, 1.2f, 8f);
            kc.isRightDir = (collision.transform.position.x - transform.position.x) > 0 ? true : false;
            kc.StartCoroutine("LowDamaged");
        }
    }
}
