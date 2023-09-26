using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    
    public static Gamemanager instance;

    public KirbyController kirbyController;
    public CameraMovement cameraMove;
    public GameObject kirbyDieDoll;

    const int MAX_HP = 6;
    const int MAX_LIFE = 3;

    int _score;
    int _kirbyLife;
    int _currentHpPoint;

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

    void Start()
    {
        _currentHpPoint = MAX_HP;
        _kirbyLife = MAX_LIFE;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine("KirbyGameOverEvent");
            UIManager.instance.kirbyLifeText.text = "-";
        }
    }

    public void Damaged(int damageAmount)
    {
        if(_currentHpPoint > 0)
        {
            _currentHpPoint -= damageAmount;
            UIManager.instance.UpdateHPBar(_currentHpPoint,UIManager.instance.kirbyHpBar);

            if (_currentHpPoint <= 0)
            {
                if (_kirbyLife-- > 0)
                {
                    //Die 재생
                    StartCoroutine("KirbyDieEvent");
                    UIManager.instance.kirbyLifeText.text = _kirbyLife.ToString();
                }
                else
                {
                    StartCoroutine("KirbyGameOverEvent");
                    UIManager.instance.kirbyLifeText.text = "-";
                }
            }
        }
    }

    public void IncreaseHP(int amount)
    {
        _currentHpPoint = Mathf.Min(MAX_HP, _currentHpPoint + amount);
        UIManager.instance.UpdateHPBar(_currentHpPoint, UIManager.instance.kirbyHpBar);
    }

    public void IncreaseScore(int num)
    {
        _score += num;
        UIManager.instance.UpdateScoreText(_score);
    }

    IEnumerator KirbyDieEvent()
    {
        var currentAC = SoundManager.instance.speaker.clip;
        var dieDoll = Instantiate(kirbyDieDoll);
        dieDoll.transform.position = kirbyController.transform.position;
        dieDoll.GetComponent<KirbyDieDoll>().spriteRender.flipX = kirbyController.kirbySprite.flipX;
        Destroy(kirbyController.gameObject);

        SoundManager.instance.speaker.Stop();

        yield return new WaitForSeconds(1f);

        //BGM 변경 - Death Theme
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[2];
        SoundManager.instance.speaker.Play();
        SoundManager.instance.speaker.loop = false;

        yield return new WaitForSeconds(2.5f);
        UIManager.instance.PlayFadeFX(0.2f);
        yield return new WaitForSeconds(0.6f);
        ResetHP();
        UIManager.instance.ChangeAbilityImage(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //BGM 변경 - SCENE Theme
        SoundManager.instance.speaker.clip = currentAC;
        SoundManager.instance.speaker.Play();
        SoundManager.instance.speaker.loop = true;
    }

    IEnumerator KirbyGameOverEvent()
    {
        var currentAC = SoundManager.instance.speaker.clip;
        var dieDoll = Instantiate(kirbyDieDoll);
        dieDoll.transform.position = kirbyController.transform.position;
        dieDoll.GetComponent<KirbyDieDoll>().spriteRender.flipX = kirbyController.kirbySprite.flipX;
        Destroy(kirbyController.gameObject);
        SoundManager.instance.speaker.Stop();

        yield return new WaitForSeconds(1f);
       
        //BGM 변경 - Death Theme
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[2];
        SoundManager.instance.speaker.Play();
        SoundManager.instance.speaker.loop = false;
        yield return new WaitForSeconds(2.5f);
        UIManager.instance.PlayFadeFX(0.8f);
        yield return new WaitForSeconds(1.2f);
        UIManager.instance.ChangeAbilityImage(0);
        SceneManager.LoadScene("GameOverScene");
        UIManager.instance.kirbyLifeText.text =  "3";
        UIManager.instance.inGameUI.SetActive(false);
        //BGM 변경 - SCENE Theme
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[3];
        SoundManager.instance.speaker.Play();
    }

    public void StartEndingScene()
    {
        StartCoroutine(EndingRoll());
    }

    IEnumerator EndingRoll()
    {
        yield return new WaitForSeconds(2f);
        UIManager.instance.PlayFadeFX(2f);
        yield return new WaitForSeconds(0.65f);
        UIManager.instance.ChangeAbilityImage(0);
        UIManager.instance.kirbyLifeText.text = "3";
        UIManager.instance.UpdateHPBar(6, UIManager.instance.kirbyHpBar);
        UIManager.instance.UpdateHPBar(6, UIManager.instance.bossHpBar);
        UIManager.instance.inGameUI.SetActive(false);
        _score = 0;
        UIManager.instance.UpdateScoreText(_score);
        Destroy(kirbyController.gameObject);
        kirbyController = null;
        yield return new WaitForSeconds(1.35f);
        SceneManager.LoadScene("EndingScene");
        SoundManager.instance.speaker.clip = SoundManager.instance.BGM[7];
        SoundManager.instance.speaker.Play();
        yield return new WaitForSeconds(2.5f);
    }


    public void ResetHP()
    {
        _currentHpPoint = MAX_HP;
        UIManager.instance.UpdateHPBar(_currentHpPoint, UIManager.instance.kirbyHpBar);
    }
}
