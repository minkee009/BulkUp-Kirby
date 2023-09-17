using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    
    public static Gamemanager instance;
    public GameObject[] hpBar = new GameObject[6];
    public KirbyController kirbyController;
    public CameraMovement cameraMove;

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
        
    }

    public void Damaged(int damageAmount)
    {
        if(_currentHpPoint > 0)
        {
            _currentHpPoint -= damageAmount;
            CheckHp();
            Debug.Log(_currentHpPoint);

            if(_currentHpPoint <= 0)
            {
                if (_kirbyLife-- > 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    _currentHpPoint = MAX_HP;
                    CheckHp();
                    Destroy(kirbyController.gameObject);
                    //Die Àç»ý
                }
                else
                    SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    public void IncreaseScore(int num)
    {
        _score += num;
    }

    void CheckHp()
    {
        for(int i = 0; i < 6; i++)
        {
            if(i < _currentHpPoint)
            {
                hpBar[i].SetActive(true);
            }
            else
            {
                hpBar[i].SetActive(false);
            }
        }
    }
}
