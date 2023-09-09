using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotHead : MonoBehaviour
{ 
    [SerializeField] private float moveSpeed = 2f; // 이동 속도
    [SerializeField] private float attackRange = 7f;
  
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject fire;
    
    [SerializeField] private bool ismove = false;
    [SerializeField] private bool isAttack = false;

    private float distanceToKirby;
    
    private Rigidbody2D _rigidbody2D;

    private Transform kirbyTransform;

    private Vector2 direction;
    
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;

        distanceToKirby = Vector2.Distance(kirbyTransform.transform.position, this.transform.position);

        direction = kirbyTransform.transform.position - this.transform.position;
        direction.Normalize();

        if (!ismove)
        {
            Move();
        }

        if (!isAttack)
        {
            StartCoroutine(Charge());
        }
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
    }

    IEnumerator Charge()
    {
        if (!isAttack)
        {
            isAttack = true;
            ismove = true;

            yield return new WaitForSeconds(1.5f);

            if (distanceToKirby > attackRange)
            {
                StartCoroutine(LongDistanceAttack());
            }
            else
            {
                StartCoroutine(ShortDistanceAttack());
            }
        }
    }

    IEnumerator ShortDistanceAttack()
    {
        if (direction.x < 0)
        {
            GameObject leftFireAttack = Instantiate(fire);
            leftFireAttack.transform.position = transform.position;
            leftFireAttack.transform.rotation = Quaternion.Euler(0,0,180);
            
            Destroy(leftFireAttack, 3f);

        }
        else
        {
            GameObject leftFireAttack = Instantiate(fire);
            leftFireAttack.transform.position = transform.position;
            leftFireAttack.transform.rotation = Quaternion.Euler(0,0,0);
            
            Destroy(leftFireAttack, 3f);
        }
        
        yield return new WaitForSeconds(3.1f);

        ismove = false;
        
        yield return new WaitForSeconds(2.5f);

        isAttack = false;
    }

    IEnumerator LongDistanceAttack()
    {
        GameObject fireBallAttack = Instantiate(fireBall);
        fireBallAttack.transform.position = transform.position;
        
        Destroy(fireBallAttack, 5f);
        
        yield return new WaitForSeconds(1.1f);

        ismove = false;
        
        yield return new WaitForSeconds(2.0f);

        isAttack = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            ismove = true;
            isAttack = true;
            Destroy(this.gameObject, 0.5f);
        }
        if (other.gameObject.tag == "Wall")
        {
            moveSpeed = moveSpeed * -1f; // 방향 전환을 위한 식
            Debug.Log("핫 헤드의 벽 충돌로 인한 방향 전환");
        }
    }
}