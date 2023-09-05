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

    public float beamTime = 10f;

    private Rigidbody2D _rigidbody2D;

    private int randomNumber;

    private float currentTime;

    public GameObject beam;

    private bool isAttacking;
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
        // currentTime += Time.deltaTime;
        //
        // if (randomNumber == 4)
        // {
        //     _state = State.Charge;
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        
        // switch (_state)
        // {
        //     case State.Move:
        //         Move();
        //         break;
        //     case State.Charge:
        //         Charge();
        //         break;
        //     case State.Attack:
        //         Attack();
        //         break;
        //     case State.Dead:
        //         Dead();
        //         break;
        // }
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
        
        if (randomNumber == 1)
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
        if (currentTime > 1.5)
        {
            _state = State.Move;
            
            currentTime = 0;
            
            Debug.Log("Charge");
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(RightBeam());
        }

    }

    IEnumerator LeftBeam()
    {
        isAttacking = true;
        
        
        GameObject beamAttack = Instantiate(beam);

        beamAttack.transform.position = transform.position;
        beamAttack.transform.rotation = Quaternion.Euler(0,0,0);
        Destroy(beamAttack,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack2 = Instantiate(beam);

        beamAttack2.transform.position = transform.position;
        beamAttack2.transform.rotation = Quaternion.Euler(0,0,22.5f);
        Destroy(beamAttack2,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack3 = Instantiate(beam);

        beamAttack3.transform.position = transform.position;
        beamAttack3.transform.rotation = Quaternion.Euler(0,0,45);
        Destroy(beamAttack3,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack4 = Instantiate(beam);

        beamAttack4.transform.position = transform.position;
        beamAttack4.transform.rotation = Quaternion.Euler(0,0,67.5f);
        Destroy(beamAttack4,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack5 = Instantiate(beam);

        beamAttack5.transform.position = transform.position;
        beamAttack5.transform.rotation = Quaternion.Euler(0,0,90);
        Destroy(beamAttack5,beamTime);

        yield return new WaitForSeconds(beamTime);

        isAttacking = false;

        _state = State.Move;
    }

    IEnumerator RightBeam()
    {
        isAttacking = true;
        
        
        GameObject beamAttack = Instantiate(beam);
        beamAttack.transform.position = transform.position;
        beamAttack.transform.rotation = Quaternion.Euler(0,0,0);
        Destroy(beamAttack,beamTime);
        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack2 = Instantiate(beam);
        beamAttack2.transform.position = transform.position;
        beamAttack2.transform.rotation = Quaternion.Euler(0,0,335.5f);
        Destroy(beamAttack2,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack3 = Instantiate(beam);
        beamAttack3.transform.position = transform.position;
        beamAttack3.transform.rotation = Quaternion.Euler(0, 0, 312);
        Destroy(beamAttack3,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack4 = Instantiate(beam);
        beamAttack4.transform.position = transform.position;
        beamAttack4.transform.rotation = Quaternion.Euler(0,0,288.5f);
        Destroy(beamAttack4,beamTime);

        yield return new WaitForSeconds(beamTime);

        GameObject beamAttack5 = Instantiate(beam);
        beamAttack5.transform.position = transform.position;
        beamAttack5.transform.rotation = Quaternion.Euler(0,0,265);
        Destroy(beamAttack5,beamTime);

        yield return new WaitForSeconds(beamTime);

        isAttacking = false;

        _state = State.Move;
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
            moveSpeed = moveSpeed * -1.0f; // 방향 전환을 위한 식
            Debug.Log("웨이들 두의 벽 충돌로 인한 방향 전환");
        }

        if (other.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }
}