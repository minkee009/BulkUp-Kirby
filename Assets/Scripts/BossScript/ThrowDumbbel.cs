using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDumbbel : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    [SerializeField] private float rotateSpeed = 100;
    
    [SerializeField] private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0,0) * moveSpeed * Time.deltaTime, 0);

        transform.RotateAround(target.position, new Vector3(0, 0, 30), rotateSpeed * Time.deltaTime);
        
    }
}
