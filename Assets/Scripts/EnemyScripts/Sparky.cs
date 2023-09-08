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

    private Transform kirbyTransform;

    private Rigidbody2D _rigidbody2D;

    private bool isJumping = false;

    private Vector2 direction;

    private float jumpRandomNumber;
    private float attackRandomNumber;

    private bool isMove = false;
    private bool isAttack = false; 

    public GameObject spark;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating("JumpRandomNumber", 0f, 1f);
        InvokeRepeating("AttackRandomNumber", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();
        
        if (direction.x < 0 && !isMove)
        {
            LeftMove();

        }
        else if (direction.x > 0 && !isMove)
        {
            RightMove();
        }
        if (!isAttack && attackRandomNumber == 1)
        {
            attackRandomNumber = 0;
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
        isAttack = false;
    }

    void JumpRandomNumber()
    {
        jumpRandomNumber = Random.Range(0, 2);
    }

    void AttackRandomNumber()
    {
        attackRandomNumber = Random.Range(0, 3);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
