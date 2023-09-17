using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenuUI;

    private bool _isPaused = false;
    private bool _playFadeFX = false;

    public Image blackScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {  
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playFadeFX && Input.GetKeyDown(KeyCode.Escape)) // 또는 원하는 키를 사용
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        _isPaused = true;
        pauseMenuUI.SetActive(true);
    }

    void Resume()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        pauseMenuUI.SetActive(false);
    }

    /// <summary>
    /// 페이드 아웃 후 WaitTime을 기다린 후 페이드 인
    /// </summary>
    /// <param name="waitTime"></param>
    public void PlayFadeFX(float waitTime)
    {
        if (_playFadeFX) return;
        StartCoroutine("FadeForBlackScreen", waitTime);
    }

    IEnumerator FadeForBlackScreen(float waitTime)
    {
        _playFadeFX = true;
        var screenT = 0f;
        while (screenT != 0.65f)
        {
            screenT = Mathf.Min(0.65f, screenT + Time.deltaTime);
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 1), screenT / 0.65f);
            yield return null;
        }

        screenT = 0f;
        yield return new WaitForSeconds(waitTime);

        while (screenT != 0.65f)
        {
            screenT = Mathf.Min(0.65f, screenT + Time.deltaTime);
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 0), screenT / 0.65f);
            yield return null;
        }
        _playFadeFX = false;
    }
}
