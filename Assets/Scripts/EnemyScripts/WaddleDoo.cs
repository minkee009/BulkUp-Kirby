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

    private int[] randomNumber;

    private float currentTime;

    private bool isAttacking;

    private enum State
    {
        Move,
        Jump,
        Charge,
        Attack,
        Dead
    }

    private State _state = State.Move;

    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        if (_state == State.Move)
        {
            InvokeRepeating("RandomNumber", 0f, 1f);
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (randomNumber[0] == 0)
        {
            _state = State.Jump;
        }
        else if (randomNumber[1] == 1)
        {
            _state = State.Charge;
        }

        switch (_state)
        {
            case State.Move:
                Move();
                break;
            case State.Jump:
                Jump();
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
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
    }

    private void Jump()
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

        _state = State.Move;
    }

    private void Charge()
    {
        if (currentTime > 1.5)
        {
            Debug.Log("Charge");

            _state = State.Attack;

            currentTime = 0;
        }
    }

    private void Attack()
    {
        if (!isAttacking && movement.x < 0)
        {
            StartCoroutine(LeftBeam());
        }
        else if (!isAttacking && movement.x > 0)
        {
            StartCoroutine(RightBeam());
        }

        _state = State.Move;
    }

    IEnumerator LeftBeam()
    {
        isAttacking = true;

        float angle = 0;

        for (int i = 0; i < 5; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, beamTime);

            angle += 23.5f;
            yield return new WaitForSeconds(beamTime);
        }

        isAttacking = false;

        _state = State.Move;
    }

    IEnumerator RightBeam()
    {
        isAttacking = true;

        float angle = 360;

        for (int i = 0; i < 5; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, beamTime);

            angle -= 23.5f;
            yield return new WaitForSeconds(beamTime);
        }

        isAttacking = false;

        _state = State.Move;
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }

    private void RandomNumber()
    {
        List<string> GachaList = new List<string>() { "치킨", "탕수육", "햄버거", "피자", "라면" };

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, GachaList.Count);
            print(GachaList[rand]);
            GachaList.RemoveAt(rand);
        }
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