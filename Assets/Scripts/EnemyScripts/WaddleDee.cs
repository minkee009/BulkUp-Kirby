using System;
using Unity.VisualScripting;
using UnityEngine;

public class WaddleDee : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    
    private Rigidbody2D _rigidbody2D;
    
    private enum State
    {
        Walk,
        Dead
    }

    private State _state = State.Walk;
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Walk:
                Move();
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

    private void Dead()
    {
        this.gameObject.SetActive(false);
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
            Debug.Log("웨이들 디의 벽 충돌로 인한 방향 전환");
        }
    }
}