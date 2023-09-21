using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BossScript : MonoBehaviour
{
    [SerializeField] private float _bossHp = 7;

    private bool isJumping = false;
    private bool isAttack = false;

    private Transform kirbyTransform;

    [SerializeField] private GameObject fallDumbbell;
    [SerializeField] private GameObject throwDumbbell;

    [SerializeField] private int randomNumber = 0;

    private Rigidbody2D _rigidbody2D;

    private Vector2 direction;

    private float jumpDistance = -2;
    
    private int jumpCount = 0;

    private LayerMask _layerMask = 1 << 6;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform spritePivot;

    private bool isDieAnim;

    [SerializeField] private GameObject dieInhaleObj;
    [SerializeField] private GameObject dieAnim;

    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        UIManager.instance.SwitchingScoreToBossHP();
    }

    // Update is called once per frame
    void Update()
    {
        if(kirbyTransform == null)
        {
            kirbyTransform = Gamemanager.instance.kirbyController?.transform;
            return;
        }
        if (isDieAnim)
            return;
        direction = kirbyTransform.transform.position - transform.position;
        direction.Normalize();

        if (randomNumber == 0)
        {
            StartCoroutine(Idle());
            randomNumber = 4;
        }
        if (randomNumber == 1)
        {
            StartCoroutine(Jump());
            randomNumber = 4;
        }
        if (randomNumber == 2)
        {
            StartCoroutine(FallDumbbell());
            randomNumber = 4;
        }
        if(randomNumber == 3)
        {
            StartCoroutine(ThrowDumbbel());
            randomNumber = 4;
        }
        
        Debug.DrawRay(transform.position, Vector2.down);

        if (_bossHp == 0)
        {
            StopAllCoroutines();
            StartCoroutine(PlayDieAnim());
        }

    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(3f);
        
        Invoke("RandomNumber", 0f);
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1f);

        jumpCount = 0;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -2f), 1, _layerMask);
        
        while (jumpCount < 3)
        {
            if (!isJumping)
            {
                
                isJumping = true;
                _rigidbody2D.velocity = new Vector2(0, 8f);

                jumpCount++;

                yield return new WaitForSeconds(1.7f);
                if (hit)
                {
                    float dumbbellTransformX = -7.0f;
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject JumpAttack = Instantiate(fallDumbbell);
                        JumpAttack.transform.position = kirbyTransform.transform.position + new Vector3(dumbbellTransformX, 9f, 0);

                        dumbbellTransformX += 3.5f;
                    }
                }

            }
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(3f);
        
        Invoke("RandomNumber", 0f);
    }

    IEnumerator FallDumbbell()
    {
        yield return new WaitForSeconds(1f);

        if (!isAttack)
        {
            isAttack = true;
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);

                GameObject fall = Instantiate(this.fallDumbbell);
                fall.transform.position = kirbyTransform.transform.position + new Vector3(0, 9f, 0);
                
                Destroy(fall, 4f);
            }

            yield return new WaitForSeconds(3f);
            
            isAttack = false;
            
            Invoke("RandomNumber", 0f);
        }
    }

    IEnumerator ThrowDumbbel()
    {
        yield return new WaitForSeconds(1f);
        
        if (!isAttack)
        {
            isAttack = true;

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);

                GameObject wave = Instantiate(throwDumbbell);
                wave.transform.position = transform.position + new Vector3(-1,1.5f,0);
                
                Destroy(wave, 4f);
            }
            yield return new WaitForSeconds(3f);
            
            isAttack = false;
            
            Invoke("RandomNumber", 0f);
        }
    }

    void RandomNumber()
    {
        randomNumber = Random.Range(0, 4);
        Debug.Log(randomNumber);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Kirby") && collision.gameObject.layer == 8)
        {
            Gamemanager.instance.cameraMove.ShakeCamera(24f, 0.3f, 8f);
            _bossHp--;
            UIManager.instance.UpdateHPBar(_bossHp, UIManager.instance.bossHpBar);
        }
    }

    IEnumerator PlayReactY()
    {
        var count = 0f;
        while (count < 12f)
        {
            count += Time.deltaTime * 56f;
            var yValue = Mathf.Sin(count) * 0.12f;
            spritePivot.localPosition = new Vector3(spritePivot.localPosition.x, yValue, spritePivot.localPosition.z);
            yield return null;
        }

        spritePivot.localPosition = new Vector3(spritePivot.localPosition.x, 0f, spritePivot.localPosition.z);
    }


    IEnumerator PlayDieAnim()
    {
        isDieAnim = true;
        SoundManager.instance.speaker.Stop();
        StartCoroutine(PlayReactY());
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayReactY());
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayReactY());
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);

        GameObject die = Instantiate(dieAnim);
        die.transform.position = transform.position;

        GameObject obj = Instantiate(dieInhaleObj);
        obj.transform.position = transform.position;

        Destroy(die, 0.5f);
    }

}