using System;
using Unity.VisualScripting;
using UnityEngine;

public class WaddleDee : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float moveSpeed = 2f; // 이동 속도
    
    [SerializeField] private bool isMove = true;
    
    private Rigidbody2D _rigidbody2D;

     
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {                         
        if (isMove)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            isMove = false;
            this.gameObject.SetActive(false);

        }
        if (other.gameObject.tag == "Wall")
        {
            moveSpeed = moveSpeed * -1f; // 방향 전환을 위한 식
            Debug.Log("웨이들 디 방향 전환");
        }
    }
}