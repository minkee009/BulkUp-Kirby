using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void Change()
    {
        StartCoroutine("StartGame");
    }
    public void MainChange()
    {
        UIManager.instance.PlayFadeFX(0f);
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator StartGame()
    {
        UIManager.instance.PlayFadeFX(0.2f);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene("GameScene");
        UIManager.instance.inGameUI.SetActive(true);
        yield return new WaitForSeconds(0.8f);
       
    }
}
