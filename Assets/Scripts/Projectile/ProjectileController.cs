using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Animator projectileAnim;
    public string animationClip;

    // Start is called before the first frame update
    void Start()
    {
        projectileAnim.Play(animationClip);
    }
}
