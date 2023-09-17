using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KDumbbellAtk : MonoBehaviour
{
    public Rigidbody2D rb;
    public float dir;
    public float speed;
    public float gravity;

    private void Start()
    {
        rb.velocity = Vector2.up * 2f;
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dir * speed, Mathf.Max(rb.velocity.y - gravity * Time.deltaTime, -gravity * 3f));
    }

    private void OnDestroy()
    {
        Gamemanager.instance.cameraMove.ShakeCamera(15f, 0.25f, 8f);
    }
}
