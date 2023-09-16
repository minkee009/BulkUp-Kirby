using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDumbbell : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    [SerializeField] private float rotateSpeed = 60f;

    [SerializeField] private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime, 0);
        
        transform.RotateAround(target.position, new Vector3(0, 0, 30) ,rotateSpeed * Time.deltaTime);
    }
}