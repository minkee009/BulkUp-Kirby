using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaddleDoo : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpPower = 8.0f;
    private int randomNumber;

    private Vector2 movement;
    
    private bool isJumping = false;
    private bool isCharge = false;
    private bool isAttack = false;
    private bool isMove = false;
    

    private Rigidbody2D _rigidbody2D;

    public GameObject beam;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
        InvokeRepeating("RandomNumber", 1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove)
        {
            StartCoroutine(Move());
        }
        
        if (!isCharge && randomNumber == 1)
        {
            isMove = true;
            isCharge = true;
            randomNumber = 0;
            
            StopCoroutine(Move());
            
            StartCoroutine(Charge());
        }
        if (!isJumping && randomNumber == 2)
        {
            randomNumber = 0;
            
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

        if (movement.x < 0)
        {
            StartCoroutine(LeftAttack());
        }
        else if (movement.x > 0)
        {
            StartCoroutine(RightAttack());
        }
    }

    IEnumerator LeftAttack()
    {
        isAttack = true;
        
        float angle = 0;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, 0.1f);

            angle += 23.5f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        isCharge = false;
        isMove = false;
    }
    IEnumerator RightAttack()
    {
        isAttack = true;
        
        float angle = 360;

        for (int i = 0; i < 6; i++)
        {
            GameObject beamAttack = Instantiate(beam);
            beamAttack.transform.position = transform.position;
            beamAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
            Destroy(beamAttack, 0.1f);

            angle -= 23.5f;
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        isCharge = false;
        isMove = false;
    }

    void RandomNumber()
    {
        randomNumber = Random.Range(0, 3);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            moveSpeed *= -1.0f;
            Debug.Log("방향 전환");
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
