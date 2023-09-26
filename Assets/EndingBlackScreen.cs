using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBlackScreen : MonoBehaviour
{
    public SpriteRenderer blackScreen;

    public float setTime = 1.5f;

    public float changeSpeed = 4f;

    private float timer = 0f;

    private void Update()
    {
        timer = Mathf.Min(setTime, timer + Time.deltaTime);

        if(timer == setTime)
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0f, 0f, 0f, 0.8f), changeSpeed * Time.deltaTime);
    }
}
