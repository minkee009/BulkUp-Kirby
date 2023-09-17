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

    public float currentXVel;
    public float currentYVel;

    bool isGrounded;
    bool isWallHit;
    bool isCellingHit;

    public void Initialize()
    {
        anim.Play("VFX_Kirby_Star_Blink");
        currentXVel = minus * 1.5f;
        currentYVel = -3f;
        Destroy(this, 8f);
    }

    private void FixedUpdate()
    {
        currentYVel -= 6f * Time.deltaTime;
        currentYVel = Mathf.Max(-6f, currentYVel);

        rb.velocity = new Vector2(currentXVel, currentYVel);

        BounceVelocity();
    }

    void BounceVelocity()
    {
        var wasGrounded = isGrounded;
        GroundCheck();
        if (!wasGrounded && isGrounded)
        {
            currentYVel = 6f;
        }

        var wasCellingHit = isCellingHit;
        CheckCellingHit();
        if(!wasCellingHit && isCellingHit)
        {
            currentYVel = -currentYVel;
        }

        var wasWallHit = isWallHit;
        isWallHit = CheckWallhit(currentXVel > 0 ? true : false);
        if(!wasWallHit && isWallHit)
        {
            currentXVel = -currentXVel;
        }
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    #region 체크 함수
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
    #endregion
}
