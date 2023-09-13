using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 dir = Vector3.right;
    public float slowDownSpeed = 0f;
    public bool slowDownDestroy = false;
    public bool hasDestroyTime = false;
    public float destroyTime;

    private void Start()
    {
        if (hasDestroyTime)
            Destroy(this.gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        speed = Mathf.Lerp(speed, 0f, slowDownSpeed * Time.deltaTime);
        if (slowDownDestroy && speed < 0.01f)
            Destroy(this.gameObject);
    }
}
