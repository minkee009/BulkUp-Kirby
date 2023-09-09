using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalFly : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
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
        if (direction.x < 0 && !isFly)
        {
            LeftFly();
        }
        else if(direction.x > 0 && !isFly)
        {
            RightFly();
        }
    }

    private void LeftFly()
    {
            this.transform.Translate(new Vector2(-0.05f, 0.05f) * speed);
    }

    private void RightFly()
    {
            this.transform.Translate(new Vector2(0.05f, 0.05f) * speed);
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
