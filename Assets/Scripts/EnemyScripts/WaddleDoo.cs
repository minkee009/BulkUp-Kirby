using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaddleDoo : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 이동 속도
    public float jumpPower = 10.0f;
    private bool isJumping = false;

    private Rigidbody2D _rigidbody2D;

    private int randomNum;

    private float currentTime;
    private enum State
    {
        Move,
        Charge,
        Attack,
        Dead
    }

    private State _state = State.Move;
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        
        InvokeRepeating("RandomNumber", 0f,2f);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        
        if (randomNum == 4)
        {
            _state = State.Charge;
        }
        
        switch (_state)
        {
            case State.Move:
                Move();
                break;
            case State.Charge:
                Charge();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
        
        if (randomNum == 1)
        {
            if (!isJumping)
            {
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpPower);
                Debug.Log("Jump");
            }
            else
            {
                return;
            }
        }
    }

    private void Charge()
    {
        if (currentTime > 2)
        {
            _state = State.Move;
            
            currentTime = 0;
            
            Debug.Log("Charge");
        }
    }

    private void Attack()
    {
        
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }

    private void RandomNumber()
    {
        randomNum = Random.Range(0, 5);
        
        Debug.Log("RandomNumber");
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            _state = State.Dead;
        }
        if (other.gameObject.tag == "Wall")
        {
            moveSpeed = moveSpeed * -1.0f; // 방향 전환을 위한 식
            Debug.Log("웨이들 두의 벽 충돌로 인한 방향 전환");
        }

        if (other.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}