using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayScene : MonoBehaviour
{
    public float delayTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (Input.GetKey(KeyCode.UpArrow) && other.CompareTag("Kirby"))
        {
            Invoke("LoadScene", delayTime);
            Debug.Log("완");
        }
        
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Game2 Scene");
    }
}
