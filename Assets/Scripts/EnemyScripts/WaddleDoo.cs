using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaddleDoo : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float moveSpeed = 2.0f;
    [SerializeField] [Range(0f, 10f)] private float jumpPower = 8.0f;
    
    [SerializeField] private bool isMove = true;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isCharge = false;
    [SerializeField] private bool isAttack = false;
    
    [SerializeField] private GameObject beam; // 빔 공격 게임 오브젝트

    private int randomNumber;

    private Vector2 movement;
    
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
        InvokeRepeating("RandomNumber", 0f, 3.5f);
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
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, 0.1f);

            if (movement.x < 0)
            {
                angle += 23.5f;
            }
            else if (movement.x > 0)
            {
                angle -= 23.5f;
            }

            yield return new WaitForSeconds(0.1f);
        }

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
        if (other.gameObject.CompareTag("Kirby"))
        {
            isMove = false;
            isJumping = true;
            isCharge = true;
            isAttack = true;
            Destroy(this.gameObject, 0.5f);
        }
        
        if (other.gameObject.CompareTag("Wall"))
        {
            moveSpeed *= -1.0f;
            Debug.Log("웨이들 두 방향 전환");
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}