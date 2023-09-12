using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HotHead : MonoBehaviour
{ 
    [SerializeField] [Range(0f, 10f)] private float moveSpeed = 2f; 
    [SerializeField] [Range(0f, 20f)] private float detectionRange = 7f; // 근거리 공격과 원거리 공격 중 어떤 공격을 할지 정하는 범위
  
    // [SerializeField] private GameObject fire; // 근거리 공격 게임 오브젝트
    // [SerializeField] private GameObject fireBall; // 원거리 공격 게임 오브젝트
    
    [SerializeField] private bool ismove = true;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isLeftMove;
    [SerializeField] private bool isRightMove;

    private float distanceToKirby;
    
    private Rigidbody2D _rigidbody2D;

    private Transform kirbyTransform;

    private Vector2 direction;
    private Vector2 movement;

    
    private GameObject leftFireAttack;
    private GameObject rightFireAttack;
    private GameObject fireBallAttack;
    
    
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        leftFireAttack = transform.GetChild(0).gameObject;
        rightFireAttack = transform.GetChild(1).gameObject;
        fireBallAttack = transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;

        distanceToKirby = Vector2.Distance(kirbyTransform.transform.position, this.transform.position);

        direction = kirbyTransform.transform.position - this.transform.position;
        direction.Normalize();

        if (ismove)
        {
            Move();
        }

        if (!isAttack)
        {
            StartCoroutine(Charge());
        }
        
        if (movement.x < 0)
        {
            isLeftMove = true;
            isRightMove = false;
        }
        else
        {
            isRightMove = true;
            isLeftMove = false;
        }
    }

    private void Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
    }

    IEnumerator Charge()
    {
        isAttack = true;
        ismove = false;

        yield return new WaitForSeconds(1.5f);

        if (distanceToKirby < detectionRange)
        {
            StartCoroutine(ShortDistanceAttack());
        }
        else
        {
            StartCoroutine(LongDistanceAttack());
        }
    }

    IEnumerator ShortDistanceAttack()
    {
        float attackDirection = 0;

        if (kirbyTransform.transform.position.x < 0)
        {
            leftFireAttack.SetActive(true);
        }
        else
        {
            rightFireAttack.SetActive(true);
        }
        
        // GameObject Firespawn = Instantiate(this.fireAttack);
        // Firespawn.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        // Firespawn.transform.rotation = Quaternion.Euler(0,0,attackDirection);
        // Firespawn.SetActive(true);
        //     
        // Destroy(Firespawn, 3f);
        
        yield return new WaitForSeconds(3.1f);
        
        rightFireAttack.SetActive(false);
        leftFireAttack.SetActive(false);
        
        ismove = true;

        if (isLeftMove&& this.transform.position.x < kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
        }

        if (isRightMove && this.transform.position.x > kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
        }

        yield return new WaitForSeconds(2.5f);
        
        isAttack = false;
    }

    IEnumerator LongDistanceAttack()
    {
        GameObject fireBallSpawn = Instantiate(this.fireBallAttack);
        fireBallSpawn.transform.position = transform.position;
        fireBallSpawn.SetActive(true);
        
        Destroy(fireBallSpawn, 5f);
        
        yield return new WaitForSeconds(1.1f);

        ismove = true;
        
        if (isLeftMove == true && this.transform.position.x < kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
        }
        if (isRightMove && this.transform.position.x > kirbyTransform.transform.position.x)
        {
            moveSpeed *= -1;
        }
        
        yield return new WaitForSeconds(2.0f);
        
        isAttack = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            this.gameObject.SetActive(false);
            
            // ismove = false;
            // isAttack = true;
            // Destroy(this.gameObject, 0.5f);
        }
        if (other.gameObject.tag == "Wall")
        {
            moveSpeed *= -1; // 방향 전환을 위한 식
            Debug.Log("핫 헤드의 벽 충돌로 인한 방향 전환");
        }
    }
}