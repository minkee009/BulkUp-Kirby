using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBeamAtk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //애너미 제거 이펙트

            //태그 비교
            collision.gameObject.SetActive(false);
        }
    }
}
