using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireBallMovemoent : MonoBehaviour
{ 
    [SerializeField] [Range(0.01f, 2f)] private float speed = 0.01f;
    
    private float velocityX = 1.5f;
    // [SerializeField] private float velocityY = 1.5f;

    private Transform kirbyTransform;

    private Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.FindWithTag("Kirby").transform;

        direction = kirbyTransform.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x < 0)
        {
            if (direction.y > 0)
            {
                transform.Translate(new Vector2(-velocityX, 0.2f) * speed);
            }
            else if (direction.y < 0)
            {
                transform.Translate(new Vector2(-velocityX, -0.2f) * speed);
            }
            else
            { 
                transform.Translate(new Vector2(-velocityX, 0) * speed);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                transform.Translate(new Vector2(velocityX, 0.2f) * speed);
            }
            else if (direction.y < 0)
            {
                transform.Translate(new Vector2(velocityX, -0.2f) * speed);
            }
            else
            { 
                transform.Translate(new Vector2(velocityX, 0) * speed);
            }
        }
    }
}
