using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class ParabolaFly : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f; // 이동 속도
    [SerializeField] [Range(0f, 10f)] private float frequency = 2.5f; // 파동 빈도
    [SerializeField] [Range(0f, 10f)] private float waveHeight = 2.5f; // 파동 높이
    
    [SerializeField] private bool isFly = true;
    
    private Vector3 position;
    private Vector3 direction;
    
    private Transform kirbyTransform;

    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private GameObject dieAnim;
    
    
    void Start()
    {
        position = transform.position;

        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();
    }

    void Update()
    {
        if(isFly)
        {
            Fly();
        }
    }

    private void Fly()
    {
        if (direction.x > 0)
        {
            _spriteRenderer.flipX = true;
            
            position += transform.right * Time.deltaTime * speed;
            transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * waveHeight;
        }
        else
        {
            _spriteRenderer.flipX = false;

            position -= transform.right * Time.deltaTime * speed;
            transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * waveHeight;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
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