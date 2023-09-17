using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public void change()
    {
        UIManager.instance.PlayFadeFX(0f);
        SceneManager.LoadScene("GameScene");
        UIManager.instance.inGameUI.SetActive(true);
    }

    public void MainChange()
    {
        UIManager.instance.PlayFadeFX(0f);
        SceneManager.LoadScene("MainScene");
    }
    
}
