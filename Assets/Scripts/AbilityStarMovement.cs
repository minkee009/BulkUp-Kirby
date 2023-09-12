using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStarMovement : MonoBehaviour
{
    public float minus = 1f;

    public Rigidbody2D rb;
    public Animator anim;

    public BoxCollider2D col;
    public LayerMask groundMask;

    bool isGrounded;

    public void Initialize()
    {
        anim.Play("VFX_Kirby_Star_Blink");
        rb.AddForce(Vector2.right * minus * 25f);
    }

    private void FixedUpdate()
    {
        //var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }


    public void GroundCheck()
    {
        isGrounded = false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(col.size.x * transform.lossyScale.x, col.size.y * transform.lossyScale.y * 0.5f),
            0f, Vector2.down, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null)
        {
            isGrounded = true;
        }
    }

    public bool CheckCellingHit()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(col.size.x * transform.lossyScale.x - 0.02f, col.size.y * transform.lossyScale.y * 0.5f),
            0f, Vector2.up, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null)
        {
            return true;
        }
        return false;
    }

    public bool CheckWallhit(bool rightDir)
    {
        var realVector = rightDir ? Vector2.right : Vector2.left;

        //히트함수 수정
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(col.size.x * transform.lossyScale.x * 0.5f, col.size.y * (transform.lossyScale.y)),
            0f, realVector, transform.lossyScale.x * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null && Vector2.Dot(-realVector, raycastHit.normal) > 0.7f)
        {
            return true;
        }
        return false;
    }
}
