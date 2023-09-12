using System;
using Unity.VisualScripting;
using UnityEngine;

public class WaddleDee : MonoBehaviour
{
    [SerializeField] private float moveSpeed = -2f; // 이동 속도
    
    [SerializeField] private bool isMove = true;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 movement;

    private LayerMask _layerMask = 1 << 6;
    private Vector2 rayDirection = Vector2.left;
    
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

        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), rayDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), rayDirection, 0.3f, _layerMask);
        
        if (hit)
        {
            moveSpeed *= -1;
            rayDirection *= -1;
        }
    }

    private void Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            isMove = false;
            this.gameObject.SetActive(false);

        }
    }
}