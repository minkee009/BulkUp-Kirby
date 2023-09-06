using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaddleDoo : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 이동 속도
    public float jumpPower = 10.0f;
    private bool isJumping = false;

    public float beamTime = 10f;

    private Rigidbody2D _rigidbody2D;

    public GameObject beam;

    private Vector2 movement;

    private int randomNumber;

    private float currentTime;

    private bool isAttack;
    private bool isCharge;
    private bool isJump;

    public enum State
    {
        Move,
        Jump,
        Charge,
        Attack,
        Dead
    }

    public State _state = State.Move;

    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating("RandomNumber", 1f, 1f);
        
    }

    private void Update()
    {
        // currentTime += Time.deltaTime;
        //
        if (randomNumber == 1 && _state == State.Move)
        {
            _state = State.Charge;
        }
        else if (randomNumber == 2 && _state == State.Move)
        {
            _state = State.Jump;
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _state = State.Charge;
        // }
        
        switch (_state)
        {
            case State.Move:
                Move();
                break;
            case State.Jump:
                StartCoroutine(Jump());
                break;
            case State.Charge:
                StartCoroutine(Charge());
                break;
            case State.Attack:
                StartCoroutine(Attack());
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    private void Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;

        isAttack = false;
        isCharge = false;
        isJump = false;
    }

    IEnumerator Jump()
    {
        isJump = true;
        if (!isJumping)
        {
            isJumping = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpPower);
            Debug.Log("Jump");
        }
        else
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        
        _state = State.Move;
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(1f);
        
        if (!isCharge)
        {
            isCharge = true;
            _state = State.Attack;
        }
    }

    IEnumerator Attack()
    {
        if (!isAttack && movement.x < 0)
        {
            StartCoroutine(LeftBeam());
        }
        else if (!isAttack && movement.x > 0)
        {
            StartCoroutine(RightBeam());
        }
        
        yield return new WaitForSeconds(1f);
        
        _state = State.Move;
        
    }

    IEnumerator LeftBeam()
    {
        isAttack = true;
        
        float angle = 0;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, beamTime);

            angle += 23.5f;
            yield return new WaitForSeconds(beamTime);
        }
    }

    IEnumerator RightBeam()
    {
        isAttack = true;
        
        float angle = 360;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, beamTime);

            angle -= 23.5f;
            yield return new WaitForSeconds(beamTime);
        }
    }

    private void Dead()
    {
        Destroy(gameObject, 2f);
    }

    private void RandomNumber()
    {
        randomNumber = Random.Range(0, 3);
    }
    



private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            _state = State.Dead;
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            moveSpeed = moveSpeed * -1.0f; // 방향 전환을 위한 식
            Debug.Log("웨이들 두의 벽 충돌로 인한 방향 전환");
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}