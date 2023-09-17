using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FireBallMovemoent : MonoBehaviour
{ 
    [SerializeField] private float moveSpeedX = 5f;
    private float moveSpeedY;

    private Transform kirbyTransform;

    private Vector2 direction;

    private Rigidbody2D _rigidbody2D;

    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.transform.position - transform.position;
        direction.Normalize();

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (this.transform.position.x < kirbyTransform.transform.position.x)
        {

            _spriteRenderer.flipX = true;
        }

        if (this.transform.position.x > kirbyTransform.transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        
        Invoke("RandomNumber", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x < 0)
        {
            if (direction.y >= 0)
            {
                transform.Translate(new Vector2(-moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime));
            }
            else
            {
                transform.Translate(new Vector2(-moveSpeedX * Time.deltaTime, -moveSpeedY * Time.deltaTime));
            }
        }
        else
        {
            if (direction.y >= 0)
            {
                transform.Translate(new Vector2(moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime));
            }
            else
            {
                transform.Translate(new Vector2(moveSpeedX * Time.deltaTime, -moveSpeedY * Time.deltaTime));
            }
        }

    }

    void RandomNumber()
    {
        moveSpeedY = Random.Range(0, 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
