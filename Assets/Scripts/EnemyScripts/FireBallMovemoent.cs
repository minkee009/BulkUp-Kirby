using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireBallMovemoent : MonoBehaviour
{ 
    [SerializeField] private float moveSpeedX = 5f;
    [SerializeField] private float moveSpeedY = 1.0f;

    private Transform kirbyTransform;

    private Vector2 direction;

    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.transform.position - transform.position;
        direction.Normalize();

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x < 0)
        {
            if (direction.y > 0)
            {
                transform.Translate(new Vector2(-moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime));
            }
            else if (direction.y < 0)
            {
                transform.Translate(new Vector2(-moveSpeedX * Time.deltaTime, -moveSpeedY * Time.deltaTime));
            }
            else if (direction.y == 0)
            { 
                transform.Translate(new Vector2(-moveSpeedX * Time.deltaTime, 0));
            }
        }
        else
        {
            if (direction.y > 0)
            {
                transform.Translate(new Vector2(moveSpeedX * Time.deltaTime, moveSpeedY * Time.deltaTime));
            }
            else if (direction.y < 0)
            {
                transform.Translate(new Vector2(moveSpeedX * Time.deltaTime, -moveSpeedY * Time.deltaTime));
            }
            else if (direction.y == 0)
            { 
                transform.Translate(new Vector2(moveSpeedX * Time.deltaTime, 0));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kirby"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
