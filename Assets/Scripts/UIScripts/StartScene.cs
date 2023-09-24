using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public AudioSource gg;
    
    bool _skipInput = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SkipInput");
        UIManager.instance.PlayFadeFX(0f);
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[0];
        SoundManager.instance.speaker.Play();
        SoundManager.instance.speaker.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        var startInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);

        if (!_skipInput && startInput)
        {
            StartCoroutine("StartGame");
        }
    }

    IEnumerator StartGame()
    {
        _skipInput = true;
        gg.Play();
        UIManager.instance.PlayFadeFX(0.2f);
        yield return new WaitForSeconds(0.6f);
        UIManager.instance.inGameUI.SetActive(true);
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[1];
        SoundManager.instance.speaker.Play();
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator SkipInput()
    {
        yield return new WaitForSeconds(1.5f);
        _skipInput = false;
    }
}
