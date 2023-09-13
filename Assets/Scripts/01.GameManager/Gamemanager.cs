using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public int hpPoint;
    public int currentHpPoint;
    public int Score;
    public int kirbyLife;
    public static Gamemanager instance;
    public GameObject[] hpBar = new GameObject[6];

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
        currentHpPoint = hpPoint;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHp();
    }

    public void Damaged(int damageAmount)
    {
        if(currentHpPoint > 0)
        {
            currentHpPoint -= damageAmount;
            Debug.Log("-1");

            if(currentHpPoint <= 0)
            {
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }
    void CheckHp()
    {
        for(int i = 0; i < 6; i++)
        {
            if(i < currentHpPoint)
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
