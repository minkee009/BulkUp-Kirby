using System.Collections;
using System.Collections.Generic;
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
            UIManager.instance.UpdateHPBar(_currentHpPoint);

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
        UIManager.instance.UpdateHPBar(_currentHpPoint);
    }

    public void IncreaseScore(int num)
    {
        _score += num;
    }

    IEnumerator KirbyDieEvent()
    {
        var dieDoll = Instantiate(kirbyDieDoll);
        dieDoll.transform.position = kirbyController.transform.position;
        dieDoll.GetComponent<KirbyDieDoll>().spriteRender.flipX = kirbyController.kirbySprite.flipX;
        Destroy(kirbyController.gameObject);

        //BGM 변경 - Death Theme
        yield return new WaitForSeconds(2.5f);
        UIManager.instance.PlayFadeFX(0.2f);
        yield return new WaitForSeconds(0.6f);
        ResetHP();
        UIManager.instance.ChangeAbilityImage(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //BGM 변경 - SCENE Theme
    }

    IEnumerator KirbyGameOverEvent()
    {
        var dieDoll = Instantiate(kirbyDieDoll);
        dieDoll.transform.position = kirbyController.transform.position;
        dieDoll.GetComponent<KirbyDieDoll>().spriteRender.flipX = kirbyController.kirbySprite.flipX;
        Destroy(kirbyController.gameObject);
        yield return new WaitForSeconds(1f);
        //BGM 변경 - Death Theme
        yield return new WaitForSeconds(2.5f);
        UIManager.instance.PlayFadeFX(0.8f);
        yield return new WaitForSeconds(1.2f);
        UIManager.instance.ChangeAbilityImage(0);
        SceneManager.LoadScene("GameOverScene");
        UIManager.instance.kirbyLifeText.text =  "3";
        UIManager.instance.inGameUI.SetActive(false);
        //BGM 변경 - SCENE Theme
    }

    public void ResetHP()
    {
        _currentHpPoint = MAX_HP;
        UIManager.instance.UpdateHPBar(_currentHpPoint);
    }
}
