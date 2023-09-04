using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;

    public Vector3 CameraPos;
    public float CameraChaseSpeed = 2f;

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
        targetPos.x = Mathf.Clamp(targetPos.x, -MapX, MapX);
        targetPos.y = Mathf.Clamp(targetPos.y, -MapY, MapY);
        currentPos = Vector3.Lerp(currentPos, targetPos, CameraChaseSpeed * Time.deltaTime);
        transform.position = currentPos;
    }

    private void OnDrawGizmos()
    {
        var p1 = Vector3.right * MapX;
        var p2 = Vector3.right * -MapX;
        var p3 = Vector3.up * MapY;
        var p4 = Vector3.up * -MapY;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(p1 + p3, p2 + p3);
        Gizmos.DrawLine(p1 + p4, p2 + p4);
        Gizmos.DrawLine(p3 + p1, p4 + p1);
        Gizmos.DrawLine(p3 + p2, p4 + p2);

    }
}
