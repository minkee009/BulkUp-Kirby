using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyDieDoll : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Sprite[] kirbyDieSprites;

    float _velocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DieAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * _velocity * Time.deltaTime;
    }

    IEnumerator DieAnimation()
    {
        spriteRender.sprite = kirbyDieSprites[0];
        yield return new WaitForSeconds(1f);
        UIManager.instance.ChangeAbilityImage(6);
        _velocity = 6f;
        spriteRender.sprite = kirbyDieSprites[1];
        var Timer = 0f;
        while (gameObject.activeSelf)
        {
            Timer += Time.deltaTime;
            _velocity = Mathf.Max(-6f, _velocity - 9f * Time.deltaTime);
            if (Timer > 0.15f)
            {
                transform.rotation= Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + 90f);
                Timer = 0f;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
