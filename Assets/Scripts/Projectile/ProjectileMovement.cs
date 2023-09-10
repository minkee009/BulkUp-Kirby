using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 dir = Vector3.right;

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
}
