using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public AudioSource gg;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.PlayFadeFX(0f);
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[0];
        SoundManager.instance.speaker.Play();
        SoundManager.instance.speaker.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("StartGame");
        }
    }

    IEnumerator StartGame()
    {
        gg.Play();
        UIManager.instance.PlayFadeFX(0.2f);
        yield return new WaitForSeconds(0.6f);
        UIManager.instance.inGameUI.SetActive(true);
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[1];
        SoundManager.instance.speaker.Play();
        SceneManager.LoadScene("GameScene");
    }
}
