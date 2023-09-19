using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseImage : MonoBehaviour
{
    public Image pauseUI;
    public Sprite[] texts;

    int _index;

    private void Start()
    {
        _index = Random.Range(0,texts.Length - 1);
    }

    private void OnEnable()
    {
        _index++;
        _index %= texts.Length;
        pauseUI.sprite = texts[_index];
        pauseUI.SetNativeSize();
    }
}
