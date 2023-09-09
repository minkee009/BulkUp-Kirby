using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sparky : MonoBehaviour
{
    [SerializeField] private float jumpDistance = 1.5f;
    [SerializeField] private float lowJumpPower = 3.5f; 
    [SerializeField] private float highJumpPower = 5.0f;
    [SerializeField] private float attackRange = 7.0f;
    
    [SerializeField] private GameObject spark;
    
    private float distanceToKirby;

    private bool isJumping = false;
    private bool isMove = false;
    private bool isAttack = false; 

    private Transform kirbyTransform;

    private Rigidbody2D _rigidbody2D;

    private Vector2 direction;

    private float jumpRandomNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating("JumpRandomNumber", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();

        distanceToKirby = Vector2.Distance(kirbyTransform.transform.position, this.transform.position);
        
        if (direction.x < 0 && !isMove)
        {
            LeftMove();

        }
        else if (direction.x > 0 && !isMove)
        {
            RightMove();
        }
        if (!isAttack && distanceToKirby < attackRange)
        {
            isMove = true;

            StartCoroutine(Charge());
        }

    }

    private void LeftMove()
    {
        if (!isJumping)
        {
            isJumping = true;
            if (jumpRandomNumber == 0)
            {
                _rigidbody2D.velocity = new Vector2(-jumpDistance, lowJumpPower);
                Debug.Log("점프");
            }
            else if (jumpRandomNumber == 1)
            {
                _rigidbody2D.velocity = new Vector2(-jumpDistance, highJumpPower);
                Debug.Log("점프");
            }
        }
        else
        {
            return;
        }
    }

    private void RightMove()
    {
        if (!isJumping)
        {
            isJumping = true;
            if (jumpRandomNumber == 0)
            {
                _rigidbody2D.velocity = new Vector2(jumpDistance, lowJumpPower);
                Debug.Log("점프");
            }
            else if (jumpRandomNumber == 1)
            {
                _rigidbody2D.velocity = new Vector2(jumpDistance, highJumpPower);
                Debug.Log("점프");
            }
        }
        else
        {
            return;
        }
    }

    IEnumerator Charge()
    {
        isAttack = true;
        
        yield return new WaitForSeconds(1f);

        GameObject generateSpark = Instantiate(spark);
        generateSpark.transform.position = transform.position;
        
        Destroy(generateSpark, 1.5f);
        
        yield return new WaitForSeconds(1.5f);

        isMove = false;
        
        yield return new WaitForSeconds(3f);

        isAttack = false;
    }

    void JumpRandomNumber()
    {
        jumpRandomNumber = Random.Range(0, 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            isMove = true;
            isJumping = true;
            isAttack = true;
            Destroy(this.gameObject, 0.5f);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
