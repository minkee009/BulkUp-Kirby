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

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private GameObject dieAnim;
    
    private void Start()
    {
        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isMove)
        {
            Move();
        }

        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), rayDirection);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), rayDirection, 0.27f, _layerMask);
        
        if (hit)
        {
            moveSpeed *= -1;
            rayDirection *= -1;
            if (!_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;

            }
        }
    }

    private void Move()
    {
        movement = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            this.gameObject.SetActive(false);

            GameObject die = Instantiate(dieAnim);
            die.transform.position = transform.position;

            Destroy(die, 0.5f);
        }
    }
}