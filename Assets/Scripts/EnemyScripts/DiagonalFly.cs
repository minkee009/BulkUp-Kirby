using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalFly : MonoBehaviour
{
    [SerializeField] [Range(0f, 0.1f)] private float speed = 0.1f;
    [SerializeField] private float velocityX = 0.05f;
    
    [SerializeField] private bool isFly = false;
    
    private Transform kirbyTransform;

    private Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isFly)
        {
            Fly();
        }
    }

    private void Fly()
    {
        if (direction.x < 0)
        {
            this.transform.Translate(new Vector2(-velocityX, 0.05f) * speed);
        }
        else
        {
            this.transform.Translate(new Vector2(velocityX, 0.05f) * speed);
        }
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
