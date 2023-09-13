using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Animator projectileAnim;
    public string animationClip;

    public GameObject destroyEffect;

    // Start is called before the first frame update
    void Start()
    {
        projectileAnim.Play(animationClip);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.layer);

        if (other.gameObject.layer == 6 
            || other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
