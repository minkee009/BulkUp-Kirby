using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private void Start()
    {
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[4];
        SoundManager.instance.speaker.Play();
    }
}
