using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaddleDoo : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] [Range(0f, 10f)] private float jumpPower = 8.0f;
    
    [SerializeField] private bool isMove = true;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isCharge = false;
    [SerializeField] private bool isAttack = false;
    
    [SerializeField] private GameObject beam; // 빔 공격 게임 오브젝트

    private int randomNumber = 0;

    private Vector2 movement;
    
    private Rigidbody2D _rigidbody2D;

    private LayerMask _layerMask = 1 << 6;
    private Vector2 rayDirection = Vector2.left;

    private SpriteRenderer _spriteRenderer;

    private Animator _animator;

    [SerializeField] private GameObject dieAnim;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();
        
        InvokeRepeating("RandomNumber", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            StartCoroutine(Move());
        }
        
        if (!isCharge && randomNumber == 0)
        {
            isMove = false;
            isCharge = true;
            randomNumber = 2;
            
            StopCoroutine(Move());
            
            StartCoroutine(Charge());
        }
        if (!isJumping && randomNumber == 1)
        {
            randomNumber = 2;
            
            Jump();
        }
        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), rayDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), rayDirection, 0.27f, _layerMask);
        
        if (hit)
        {
            moveSpeed *= -1;
            rayDirection *= -1;

            if (!_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

    IEnumerator Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;

        yield return null;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpPower);
            Debug.Log("점프");

            isJumping = true;
        }
        else
        {
            return;
        }
    }

    IEnumerator Charge()
    {
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(1f);

        if (!isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        
        float angle = 0;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position + new Vector3(0, 0.25f, 0);
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            beamAttack.transform.parent = transform;
            
            if (movement.x < 0)
            {
                angle += 23.5f;
            }
            else if (movement.x > 0)
            {
                angle -= 23.5f;
            }
            
            Destroy(beamAttack, 0.1f);

            yield return new WaitForSeconds(0.1f);
        }
        _animator.SetBool("isWalk", true);
        _animator.SetBool("isAttack", false);
        
        yield return new WaitForSeconds(0.5f);
        
        isMove = true;
        isCharge = false;
        isAttack = false;
        

    }
    
    void RandomNumber()
    {
        randomNumber = Random.Range(0, 2);
        Debug.Log("RandomNumber");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            this.gameObject.SetActive(false);

            GameObject die = Instantiate(dieAnim);
            die.transform.position = transform.position;
            
            Destroy(die, 0.5f);
        }
    }
}
