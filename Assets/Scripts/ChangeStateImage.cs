using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStateImage : MonoBehaviour
{
    public Image currentStateImage;

    public Sprite[] stateImages;

    int debugNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            debugNum++;
            debugNum %= stateImages.Length;

            ChangeSprite(debugNum);
        }
    }

    public void ChangeSprite(int index)
    {
        index = Math.Clamp(index, 0, stateImages.Length - 1);
        currentStateImage.sprite = stateImages[index];
    }
}
