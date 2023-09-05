using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sparky : MonoBehaviour
{
    public float jumpPower = 5f;

    private Transform kirbyTransform;

    private Rigidbody2D _rigidbody2D;
    enum State
    {
        Move,
        Attack,
        Dead
    }

    private State _state = State.Move;
    
    // Start is called before the first frame update
    void Start()
    {
        kirbyTransform = GameObject.Find("Kirby").transform;

        _rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
