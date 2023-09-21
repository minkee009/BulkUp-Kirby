using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas canvas;

    public GameObject[] kirbyHpBar = new GameObject[6];
    public GameObject[] bossHpBar = new GameObject[6];

    public GameObject inGameUI;
    public GameObject pauseMenuUI;
    public Image kirbyAbiltyImage;
    public TMP_Text kirbyLifeText;
    public TMP_Text gameScoreText;

    public Sprite[] abiltySprites;

    private bool _isPaused = false;
    private bool _playFadeFX = false;
    private int _currentAbiltyIndex;
    private Coroutine _uiCoroutine;

    public Image blackScreen;

    public GameObject BossHP;
    public GameObject Score;

    

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

        canvas = GetComponent<Canvas>();
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

    public void UpdateHPBar(float curretHP, GameObject[] hpBar)
    {
        for (int i = 0; i < hpBar.Length; i++)
        {
            if (i < curretHP)
            {
                hpBar[i].SetActive(true);
            }
            else
            {
                hpBar[i].SetActive(false);
            }
        }
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
        canvas.sortingOrder = 1;
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
        canvas.sortingOrder = 0;
    }

    public void ChangeAbilityImage(int index)
    {
        _currentAbiltyIndex = index;
        kirbyAbiltyImage.sprite = abiltySprites[_currentAbiltyIndex];
    }

    public void TempChangeAbilityImage(int index, float time)
    {
        StopTempChange();
        _uiCoroutine = StartCoroutine(TempChangeAbility(index, time));
    }

    public void StopTempChange()
    {
        if(_uiCoroutine != null) StopCoroutine(_uiCoroutine);
        kirbyAbiltyImage.sprite = abiltySprites[_currentAbiltyIndex];
    }
    
    public void UpdateScoreText(int score)
    {
        gameScoreText.text = score.ToString();
    }

    public void SwitchingScoreToBossHP()
    {
        Score.SetActive(false);
        BossHP.SetActive(true);
    }

    public void SwitchingBossHPToScore()
    {
        Score.SetActive(true);
        BossHP.SetActive(false);
    }


    IEnumerator TempChangeAbility(int index, float time)
    {
        kirbyAbiltyImage.sprite = abiltySprites[index];
        yield return new WaitForSeconds(time);
        kirbyAbiltyImage.sprite = abiltySprites[_currentAbiltyIndex];
    }
}
