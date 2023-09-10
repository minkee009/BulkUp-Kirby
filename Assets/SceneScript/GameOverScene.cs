using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public void change()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainChange()
    {
        SceneManager.LoadScene("MainScene");
    }
    // Start
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
