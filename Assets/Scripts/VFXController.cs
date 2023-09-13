using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public Animator vfxCon;
    public string clipName;

    private void Start()
    {
        vfxCon.Play(clipName);
    }

    private void OnEnable()
    {
        vfxCon.Play(clipName);
    }
}
