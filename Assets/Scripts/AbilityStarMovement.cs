using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStarMovement : MonoBehaviour
{
    public float minus = 1f;

    public Rigidbody2D rb;
    public Animator anim;


    public void Initialize()
    {
        anim.Play("VFX_Kirby_Star_Blink");
        rb.AddForce(Vector2.right * minus * 25f);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
