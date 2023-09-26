using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingRoll : MonoBehaviour
{
    public float rollSpeed = 5f;

    private bool _endScroll;

    // Update is called once per frame
    void Update()
    {
        var skipRoll = Input.anyKey;

        transform.position += Vector3.up * rollSpeed * (skipRoll ? 5f : 1f) * Time.deltaTime;

        if (!_endScroll && transform.position.y > 11f)
        {
            _endScroll = true;
            StartCoroutine(EndScrollEvent());
        }
    }

    IEnumerator EndScrollEvent()
    {
        UIManager.instance.PlayFadeFX(2f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
    }
}
