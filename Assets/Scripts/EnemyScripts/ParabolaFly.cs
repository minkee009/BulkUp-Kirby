using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class ParabolaFly : MonoBehaviour
{
    [SerializeField] float dist = 8f;
    [SerializeField] float speed = 2.5f;
    [SerializeField] float frequency = 2.5f;
    [SerializeField] float waveHeight = 2.5f;
    
    private Vector3 position;
    private Vector3 localScale;
    private Vector3 direction;

    private bool isFly = false;
    
    private Transform kirbyTransform;
    
    void Start()
    {
        position = transform.position;
        localScale = transform.localScale;

        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();
        
    }

    void Update()
    {
        if (direction.x < 0 && !isFly)
        {
            LeftDirectionFly();
        }
        else if (direction.x > 0 && !isFly)
        {
            RightDirectionFly();
        }
    }

    private void RightDirectionFly()
    {
        localScale.x = 1;
        transform.transform.localScale = localScale;
        position += transform.right * Time.deltaTime * speed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * waveHeight;

    }

    private void LeftDirectionFly()
    {
        localScale.x = -1;
        transform.transform.localScale = localScale;
        position -= transform.right * Time.deltaTime * speed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * waveHeight;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            isFly = true;
            Destroy(this.gameObject, 0.5f);
        }
    }
}

// {
//     public float forceAmount = 10f;
//     
//     private Rigidbody2D _rigidbody2D;
//
//     private float currentTime = 0;
//     public float gravityScaleChangeTime = 1.5f;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
//         
//         _rigidbody2D.AddForce(new Vector2(-0.3f, 0.7f) * forceAmount, ForceMode2D.Impulse);
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         ParabolaFly();
//     }
//     
//     private void ParabolaFly()
//     {
//         currentTime += Time.deltaTime;
//
//         if (currentTime > gravityScaleChangeTime)
//         {
//             _rigidbody2D.gravityScale *= -1;
//
//             currentTime = 0;
//         }    
//     }
//     
//     private void Dead()
//     {
//         this.gameObject.SetActive(false);
//     }
// }
