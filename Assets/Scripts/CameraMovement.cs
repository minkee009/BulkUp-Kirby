using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;

    public Vector3 CameraPos;
    public float CameraChaseSpeed = 2f;
    public Transform Center;

    public float MapY;
    public float MapX;

    Vector3 targetPos;
    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        targetPos = PlayerTransform.position + CameraPos;
        targetPos.x = Mathf.Clamp(targetPos.x, -MapX + Center.position.x, MapX + Center.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -MapY + Center.position.y, MapY + Center.position.y);
        currentPos = Vector3.Lerp(currentPos, targetPos, CameraChaseSpeed * Time.deltaTime);
        transform.position = currentPos;
    }

    private void OnDrawGizmos()
    {
        var p1 = Vector3.right * MapX;
        var p2 = Vector3.right * -MapX;
        var p3 = Vector3.up * MapY;
        var p4 = Vector3.up * -MapY;

        Vector3[] points = { p2 + p3, p1 + p3, p1 + p4, p2 + p4 };

        if(Center != null)
        {
            for(int i = 0; i < points.Length; i++)
            {
                points[i] += Center.position;
            }
        }

        Gizmos.color = Color.red;

        for(int i = 0;i<points.Length; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
        }
    }
}
