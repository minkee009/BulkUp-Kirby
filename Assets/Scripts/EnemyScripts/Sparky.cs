using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Sparky : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float jumpDistance = 1.5f; // 이동 거리
    [SerializeField] [Range(0f, 10f)] private float lowJumpPower = 2.0f; // 최소 점프력
    [SerializeField] [Range(0f, 10f)] private float highJumpPower = 5.0f; // 최대 점프력
    [SerializeField] [Range(0f, 20f)] private float detectionRange = 7.0f; // 범위 내 커비가 있을 시, 공격
    
    [SerializeField] private bool isMove = true;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isAttack = false; 
    
    private float distanceToKirby;
    
    private Transform kirbyTransform;

    private Rigidbody2D _rigidbody2D;

    private Vector2 direction;
    
    private float randomJumpPower; // 점프력을 랜덤으로 할당

    private GameObject attackObject;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating("RandomJumpPower", 0f, 1f);

        attackObject = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();

        distanceToKirby = Vector2.Distance(kirbyTransform.transform.position, this.transform.position);

        if (isMove)
        {
            Move();
        }

        if (!isAttack && distanceToKirby < detectionRange)
        {
            isMove = false;

            StartCoroutine(Charge());
        }
    }

    private void Move()
    {
        if (!isJumping)
        {
            if (direction.x < 0)
            {
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(-jumpDistance, randomJumpPower);
                Debug.Log("점프");
            }
            else
            {
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(jumpDistance, randomJumpPower);
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

        attackObject.SetActive(true);
        
        
        yield return new WaitForSeconds(2f);
        
        attackObject.SetActive((false));

        isMove = true;
        
        yield return new WaitForSeconds(3f);

        isAttack = false;
    }

    void RandomJumpPower()
    {
        randomJumpPower = Random.Range(lowJumpPower, highJumpPower);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            this.gameObject.SetActive(false);
            
            // isMove = false;
            // isJumping = true;
            // isAttack = true;
            // Destroy(this.gameObject, 0.5f);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
