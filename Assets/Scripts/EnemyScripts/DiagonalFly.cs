using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalFly : MonoBehaviour
{
    public float speed = 0.1f;
    public float attackRange = 20f;
    private float DistanceToTarget;
    
    private Transform kirbyTransform;

    private Vector3 direction;

    private enum State
    {
        Idle,
        LeftFly,
        RightFly,
        Dead
    }

    private State _state = State.Idle;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.Find("Kirby").transform;
        
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        direction = kirbyTransform.position - transform.position;

        DistanceToTarget = (kirbyTransform.position - transform.position).sqrMagnitude;
        
        if (direction.x < 0 && attackRange > DistanceToTarget)
        {
            _state = State.LeftFly;
        }
        else if(direction.x > 0 && attackRange > DistanceToTarget)
        {
            _state = State.RightFly;
        }
        
        switch (_state)
        {
            case State.Idle:
                Idle();
                break;
            case State.LeftFly:
                LeftFly();
                break;
            case State.RightFly:
                RightFly();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    private void Idle()
    {
    }

    private void LeftFly()
    {
        this.transform.Translate(new Vector2(-0.05f, 0.05f) * speed);
    }

    private void RightFly()
    {      
        this.transform.Translate(new Vector2(0.05f, 0.05f) * speed);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Kirby")
        {
            _state = State.Dead;
        }
    }
}
