using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyCopy : MonoBehaviour
{
    public float speed = 6f;
    public float jumpSpeed = 12.5f;
    public float flySpeed = 7f;
    
    public float KnockBackSpeed = 8f;
    public float KnockBackTime = 0.2f;
    public float lastKnockBack = 0; 
    
    public VerticalState _verticalState = VerticalState.Jumping;
    private InhaleState _inhaleState = InhaleState.Not_Inhaling;

    private Rigidbody2D _rigidbody2D;

    private Animator animator;
    public enum VerticalState
    {
        Ground,
        Jumping,
        Flying,
        KnockBack
    }

    private enum InhaleState
    {
        Not_Inhaling,
        Inhaling,
        Inhaled
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HandleKnockBack();
    }
    void HandleKnockBack()
    {
        if (_verticalState == VerticalState.KnockBack)
        {
            if (Time.time > lastKnockBack + KnockBackTime)
            {
                _verticalState = VerticalState.Ground;
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInhaling();
        HandleHorizontalMovement();
        HandleJumping();
        HandleFlying();
    }

    void HandleInhaling()
    {
        
    }

    void HandleHorizontalMovement()
    {
        if (_inhaleState == InhaleState.Inhaling || 
            _verticalState == VerticalState.KnockBack)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");

        if (h > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (h < 0)
        {
            animator.SetInteger("Direction", 0);

        }
            
        Vector2 vel = _rigidbody2D.velocity;
        vel.x = h * speed;
        _rigidbody2D.velocity = vel;
    }
    void HandleJumping() 
    {
        if (_inhaleState == InhaleState.Inhaling || _verticalState == VerticalState.KnockBack) 
        {
            return;
        }

        Vector2 vel = _rigidbody2D.velocity;
        if (Input.GetKey(KeyCode.X)) 
        {
            if (_verticalState == VerticalState.Ground)
            {
                vel.y = jumpSpeed;
            }

            _rigidbody2D.velocity = vel;
            _verticalState = VerticalState.Jumping;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            if (_verticalState == VerticalState.Jumping)
            {
                vel.y = Mathf.Min(vel.y, 0);
                Debug.Log(vel.y);
            }
        }

        _rigidbody2D.velocity = vel;
    }

    void HandleFlying()
    {
        if (_inhaleState == InhaleState.Inhaling
            || _inhaleState == InhaleState.Inhaled || _verticalState == VerticalState.KnockBack)
        {
            return;
        }

        Vector2 vel = _rigidbody2D.velocity;
        if (_verticalState == VerticalState.Flying)
        {
            vel.y = -1 * flySpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            vel.y = flySpeed;
            _verticalState = VerticalState.Flying;
        }
        _rigidbody2D.velocity = vel;

    }

    void KnockBack(Collision2D other)
    {
        _verticalState = VerticalState.KnockBack;
        float velX = KnockBackSpeed;
        if (other.transform.position.x > transform.position.x)
        {
            velX *= -1;
        }

        lastKnockBack = Time.time;
        _rigidbody2D.velocity = new Vector2(velX, 3);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "enemy") 
        {
            Destroy (collision.gameObject);
            KnockBack(collision);
        } else {
            _verticalState = VerticalState.Ground;
        }
    }
}
