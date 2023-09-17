using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayScene : MonoBehaviour
{
    public string nextScene;

    bool _enterTrigger = false;

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
        
        if (!_enterTrigger && Input.GetKey(KeyCode.UpArrow) && other.CompareTag("Kirby"))
        {
            _enterTrigger = true;
            var kc = other.GetComponentInParent<KirbyController>();
            if (kc.EnterDoor(out bool hasExhale))
            {
                LoadScene(hasExhale);
                Debug.Log("완");
            }
        }
    }

    void LoadScene(bool isExhale)
    {
        StartCoroutine("WaitForDoorAction", isExhale);
    }

    IEnumerator WaitForDoorAction(bool hasExhaleAnim)
    {
        var waitTime = hasExhaleAnim ? 0.6f : 0.3f;
        yield return new WaitForSeconds(waitTime);
        //암전효과
        UIManager.instance.PlayFadeFX(0.04f);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(nextScene);
    }
}
