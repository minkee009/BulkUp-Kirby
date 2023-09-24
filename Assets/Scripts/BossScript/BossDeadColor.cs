using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadColor : MonoBehaviour
{
    public SpriteRenderer sprite;
    float _colorTime = 0f;
    // Update is called once per frame
    void Update()
    {
        _colorTime += 18f * Time.deltaTime;
        sprite.color = Color.Lerp(new Color(1f, 0.45f, 0f), Color.white, Mathf.Sin(_colorTime));
    }
}
