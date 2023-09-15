using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HotHead : MonoBehaviour
{ 
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] [Range(0f, 20f)] private float detectionRange = 7f; // 근거리 공격과 원거리 공격 중 어떤 공격을 할지 정하는 범위
  
    // [SerializeField] private GameObject fire; // 근거리 공격 게임 오브젝트
    // [SerializeField] private GameObject fireBall; // 원거리 공격 게임 오브젝트
    
    [SerializeField] private bool ismove = true;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isLeftMove;
    [SerializeField] private bool isRightMove;

    private float distanceToKirby;
    
    private Rigidbody2D _rigidbody2D;

    private Transform kirbyTransform;

    private Vector2 direction;
    private Vector2 movement;

    
    private GameObject leftFireAttack;
    private GameObject rightFireAttack;
    private GameObject fireBallAttack;

    private LayerMask _layerMask = 1 << 6;
    private Vector2 rayDirection = Vector2.left;

    private SpriteRenderer _spriteRenderer;

    private Animator _animator;

    [SerializeField] private GameObject dieAnim;
    
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

        leftFireAttack = transform.GetChild(0).gameObject;
        rightFireAttack = transform.GetChild(1).gameObject;
        fireBallAttack = transform.GetChild(2).gameObject;
        
    }

    private void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;

        distanceToKirby = Vector2.Distance(kirbyTransform.transform.position, this.transform.position);

        direction = kirbyTransform.transform.position - this.transform.position;
        direction.Normalize();

        if (ismove)
        {
            Move();
        }

        if (!isAttack)
        {
            StartCoroutine(Charge());
        }
        
        if (movement.x < 0)
        {
            isLeftMove = true;
            isRightMove = false;
        }
        else
        {
            isRightMove = true;
            isLeftMove = false;
        }
        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), rayDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), rayDirection, 0.3f, _layerMask);
        
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

    private void Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
    }

    IEnumerator Charge()
    {
        isAttack = true;
        ismove = false;
        
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttackReady", true);
        _animator.SetBool("isAttack", false);
        
        if (isLeftMove && this.transform.position.x < kirbyTransform.transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
        
        yield return new WaitForSeconds(1.5f);

        if (distanceToKirby < detectionRange)
        {
            StartCoroutine(ShortDistanceAttack());
        }
        else
        {
            StartCoroutine(LongDistanceAttack());
        }
    }

    IEnumerator ShortDistanceAttack()
    {
        float attackDirection = 0;
        
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttackReady", false);
        _animator.SetBool("isAttack", true);

        if (kirbyTransform.transform.position.x < this.transform.position.x)
        {
            leftFireAttack.SetActive(true);
        }
        else
        {
            rightFireAttack.SetActive(true);
        }
        
        // GameObject Firespawn = Instantiate(this.fireAttack);
        // Firespawn.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        // Firespawn.transform.rotation = Quaternion.Euler(0,0,attackDirection);
        // Firespawn.SetActive(true);
        //     
        // Destroy(Firespawn, 3f);
        
        yield return new WaitForSeconds(3.1f);
        
        rightFireAttack.SetActive(false);
        leftFireAttack.SetActive(false);
        
        ismove = true;
        _animator.SetBool("isWalk", true);
        _animator.SetBool("isAttackReady", false);
        _animator.SetBool("isAttack", false);
        
        if (isLeftMove&& this.transform.position.x < kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
            rayDirection *= -1;

            _spriteRenderer.flipX = true;
        }

        if (isRightMove && this.transform.position.x > kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
            rayDirection *= -1;

            _spriteRenderer.flipX = false;
        }

        yield return new WaitForSeconds(4f);
        
        isAttack = false;
        

    }

    IEnumerator LongDistanceAttack()
    {
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isAttackReady", false);
        _animator.SetBool("isAttack", true);
        
        GameObject fireBallSpawn = Instantiate(this.fireBallAttack);
        fireBallSpawn.transform.position = transform.position;
        fireBallSpawn.SetActive(true);
        
        Destroy(fireBallSpawn, 5f);
        
        yield return new WaitForSeconds(1.1f);

        ismove = true;
        _animator.SetBool("isWalk", true);
        _animator.SetBool("isAttackReady", false);
        _animator.SetBool("isAttack", false);
        
        if (isLeftMove == true && this.transform.position.x < kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
            rayDirection *= -1;
            
            _spriteRenderer.flipX = true;
        }
        if (isRightMove && this.transform.position.x > kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
            rayDirection *= -1;

            _spriteRenderer.flipX = false;
        }
        
        yield return new WaitForSeconds(4f);
        
        isAttack = false;
        

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            this.gameObject.SetActive(false);
            
            GameObject die = Instantiate(dieAnim);
            die.transform.position = transform.position;
            
            Destroy(die, 0.5f);
        }    
    }
}