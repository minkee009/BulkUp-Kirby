using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;

    public Vector3 cameraPos;
    public float cameraChaseSpeed = 2f;
    public Transform center;

    public float mapY;
    public float mapX;

    Vector3 _targetPos;
    Vector3 _currentPos;
    Vector3 _shakePos;

    float _seed;
    float _trauma;
    float _shakeRecovery;
    float _shakeSpeed;
    float _shakeAmount;

    // Start is called before the first frame update
    void Start()
    {
        _currentPos = transform.position;
        Gamemanager.instance.cameraMove = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (center == null) return;

        if(playerTransform == null && Gamemanager.instance.kirbyController != null)
        {
            playerTransform = Gamemanager.instance.kirbyController.transform;
            _targetPos = playerTransform.position + cameraPos;
            _currentPos = _targetPos;
            transform.position = _targetPos;
        }
        else if (playerTransform == null)
        {
            return;
        }

        _trauma = Mathf.Lerp(_trauma, 0, _shakeRecovery * Time.deltaTime);

        _targetPos = playerTransform.position + cameraPos;
        _targetPos.x = Mathf.Clamp(_targetPos.x, -mapX + center.position.x, mapX + center.position.x);
        _targetPos.y = Mathf.Clamp(_targetPos.y, -mapY + center.position.y, mapY + center.position.y);
        _currentPos = Vector3.Lerp(_currentPos, _targetPos, cameraChaseSpeed * Time.deltaTime);
        _shakePos = new Vector3(_shakeAmount * Mathf.PerlinNoise(_seed, Time.time * _shakeSpeed) * 2 - 1,
            _shakeAmount * Mathf.PerlinNoise(_seed + 1, Time.time * _shakeSpeed) * 2 - 1,
            _shakeAmount * Mathf.PerlinNoise(_seed + 2, Time.time * _shakeSpeed) * 2 - 1);

        transform.position = _currentPos + (_shakePos * _trauma);
    }

    public void ShakeCamera(float speed , float amount, float recovery)
    {
        _shakeSpeed = speed;
        _shakeAmount = amount;
        _shakeRecovery = recovery;
        _trauma = 1f;
    }

    private void OnDrawGizmos()
    {
        var p1 = Vector3.right * mapX;
        var p2 = Vector3.right * -mapX;
        var p3 = Vector3.up * mapY;
        var p4 = Vector3.up * -mapY;

        Vector3[] points = { p2 + p3, p1 + p3, p1 + p4, p2 + p4 };

        if(center != null)
        {
            for(int i = 0; i < points.Length; i++)
            {
                points[i] += center.position;
            }
        }

        Gizmos.color = Color.red;

        for(int i = 0;i<points.Length; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
        }
    }
}
