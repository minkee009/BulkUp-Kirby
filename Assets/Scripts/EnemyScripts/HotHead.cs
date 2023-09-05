using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotHead : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    private int randomNumber;
    
    private float currentTime;
    
    private Rigidbody2D _rigidbody2D;

    public GameObject fireBall;
    private enum State
    {
        Move,
        Charge,
        ShortDistanceAttack,
        LongDistanceAttack,
        Dead
    }

    private State _state = State.Move;
    
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        
        if (randomNumber == 4)
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
            case State.ShortDistanceAttack:
                ShortDistanceAttack();
                break;
            case State.LongDistanceAttack:
                LongDistanceAttack();
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
    }

    private void Charge()
    {
        if (currentTime > 1.5)
        {
            _state = State.Move;
            
            currentTime = 0;
        }
    }

    private void ShortDistanceAttack()
    {
        
    }

    private void LongDistanceAttack()
    {
        GameObject fireBallAttack = Instantiate(fireBall);
        fireBallAttack.transform.position = transform.position;
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }

    private void RandomNumber()
    {
        randomNumber = Random.Range(0, 5);
        
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
            moveSpeed = moveSpeed * -1f; // 방향 전환을 위한 식
            Debug.Log("핫 헤드의 벽 충돌로 인한 방향 전환");
        }
    }
}