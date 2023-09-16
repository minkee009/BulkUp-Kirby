using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossScript : MonoBehaviour
{
    private float _bossHp { get; set; } = 7;

    private bool isJumping = false;
    private bool isAttack = false;

    private Transform kirbyTransform;

    [SerializeField] private GameObject fallDumbbell;
    [SerializeField] private GameObject throwDumbbell;

    [SerializeField] private int randomNumber;

    private Rigidbody2D _rigidbody2D;

    private Vector2 direction;

    private float jumpDistance = -2;
    
    private enum State
    {
        Idle,
        Jump,
        FallDumbbell,
        ThrowDumbbell
    }

    [SerializeField] private State _state = State.Idle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("RandomNumber", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        direction = kirbyTransform.transform.position - transform.position;
        direction.Normalize();

        if (randomNumber == 0)
        {
            _state = State.Idle;
            randomNumber = 4;
        }
        if (randomNumber == 1)
        {
            _state = State.Jump;
            randomNumber = 4;

        }
        if (randomNumber == 2)
        {
            _state = State.FallDumbbell;
            randomNumber = 4;

        }
        if(randomNumber == 3)
        {
            _state = State.ThrowDumbbell;
            randomNumber = 4;

        }

        switch (_state)
        {
            case State.Idle:
                StartCoroutine(Idle());
                break;
            case State.Jump:
                StartCoroutine(Jump());
                break;
            case State.FallDumbbell:
                StartCoroutine(FallDumbbell());
                break;
            case State.ThrowDumbbell:
                StartCoroutine(ThrowDumbbel());
                break;
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < 2; i++)
        {
            if (!isJumping)
            {
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(jumpDistance, 4f);
                
                jumpDistance *= -1;
            }
        }
        yield return new WaitForSeconds(5f);

    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1f);
        
        for (int i = 0; i < 2; i++)
        {
            if (!isJumping)
            {
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(0, 8f);
                
            }
            else
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);

        }
        yield return new WaitForSeconds(3f);

    }

    IEnumerator FallDumbbell()
    {
        yield return new WaitForSeconds(1f);

        if (!isAttack)
        {
            isAttack = true;
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);

                GameObject fall = Instantiate(this.fallDumbbell);
                fall.transform.position = kirbyTransform.transform.position + new Vector3(0, 6, 0);
                
                Destroy(fall, 3f);
            }

            yield return new WaitForSeconds(3f);

            // _state = State.Idle;

            isAttack = false;
            
        }
    }

    IEnumerator ThrowDumbbel()
    {
        yield return new WaitForSeconds(1f);
        
        if (!isAttack)
        {
            isAttack = true;

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);

                GameObject wave = Instantiate(throwDumbbell);
                wave.transform.position = transform.position;
            }
            yield return new WaitForSeconds(3f);
        
            // _state = State.Idle;

            isAttack = false;

            Invoke("RandomNumber", 0f);
        }
    }

    void RandomNumber()
    {
        randomNumber = Random.Range(0, 4);
        Debug.Log("random");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}