using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalFly : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    [SerializeField] private bool isFly = false;
    
    private Transform kirbyTransform;

    private Vector2 direction;

    private Rigidbody2D _rigidbody2D;

    [SerializeField] private GameObject dieAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;
        
        direction = kirbyTransform.position - transform.position;
        direction.Normalize();

        _rigidbody2D = GetComponent<Rigidbody2D>();

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
            this.transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime));
        }
        else
        {
            this.transform.Translate(new Vector2(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime));
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            this.gameObject.SetActive(false);

            GameObject die = Instantiate(dieAnim);
            die.transform.position = transform.position;

            Gamemanager.instance.IncreaseScore(40);

            Destroy(die, 0.5f);
        }   
    }
}
