using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KirbyInhaling : KirbyState
{
    public LayerMask inhaleLayer;
    public float checkDist = 3f;
    public float inhaleAccel = 3f;
    public InhaleableObj tmpIhObj;
    public Collider2D[] overlapedColls = new Collider2D[5];
    public WaitForSeconds animStartTime = new WaitForSeconds(0.45f);
    public WaitForSeconds animEndTime = new WaitForSeconds(0.3f);

    public int captureCount = 0;
    public bool captureStart;
    public bool playAnimation;
    public bool endExhale;
    public InhaleableObj[] capturedIhObjs = new InhaleableObj[5];
    
    //private 변수
    const float MAX_INHALE_SPEED = 12f;

    float _xSpeed = 0f;
    float _ySpeed = 0f;

    public float xSpeed
    {
        get { return _xSpeed; }
        set
        {
            _xSpeed = value;
            _xSpeed = Mathf.Clamp(_xSpeed, 0f, MAX_INHALE_SPEED);
        }
    }

    public float ySpeed
    {
        get { return _ySpeed; }
        set
        {
            _ySpeed = value;
            _ySpeed = Mathf.Clamp(_ySpeed, 0f, 1f);
        }
    }

    public override void Enter()
    {
        kc.isPlayingAction = true;
        StartCoroutine("StartAnimation");
        kc.lockDir = true;
    }

    public override void OnPrePhysCheck()
    {
        if (endExhale) return;
        if (!captureStart && !playAnimation && captureCount == 0 && !kc.actHoldInput)
        {
            endExhale = true;
            StartCoroutine("EndAnimation");
        }
        if (captureStart && captureCount == 0)
        {
            captureStart = false;
            endExhale = true;
            StartCoroutine("EndState");
        }
    }

    public override void Excute()
    {
        //캐릭터 물리
        kc.CalculateVelocity(ref kc.currentXVel, 0, 3, 24, 6);
        kc.CalculateVelocity(ref kc.currentYVel, -1, 6f, 12f, 0f);
        if (kc.isGrounded)
            kc.currentYVel = 0f;

        if (endExhale)
            return;

        //박스 체킹
        if (!captureStart && captureCount == 0)
        {
            var minus = kc.isRightDir ? 1 : -1;
            var kirbyPos = new Vector2(transform.position.x, transform.position.y);
            var checkStartPos = kirbyPos + Vector2.right * (minus * checkDist + minus * 0.5f);
            var overlapcount = Physics2D.OverlapBoxNonAlloc(checkStartPos, new Vector2(checkDist * 2f, 1f), 0f, overlapedColls, inhaleLayer);
            if (overlapcount > 0)
            {
                //에너미 체킹
                for (int i = 0; i < overlapcount; i++)
                {
                    if (overlapedColls[i] == kc.hitBox
                        || overlapedColls[i] == kc.physBox
                        || !overlapedColls[i].TryGetComponent(out tmpIhObj))
                    {
                        continue;
                    }
                    
                    if(tmpIhObj.TryGetComponent(out Rigidbody2D rb))
                    {
                        rb.velocity = Vector2.zero;
                    }

                    var instanceDoll = tmpIhObj.CreateDoll();
                    capturedIhObjs[captureCount] = instanceDoll.GetComponent<InhaleableObj>();
                    tmpIhObj.gameObject.SetActive(false);

                    captureCount++;
                }

                //스타박스 체킹 - 바닥면의 스타박스는 최하단으로 보내기
                if(captureCount == 0)
                {

                }

                //바닥면의 스타박스를 리스트에 담기
                if (captureCount == 0)
                {

                }

                captureStart = captureCount > 0 ? true : false;
            }
        }

        //빨아들이기
        if (captureStart)
        {
            kc.hitBox.enabled = false;
            ySpeed += Time.deltaTime;
            xSpeed += inhaleAccel * Time.deltaTime;
            xSpeed += xSpeed * 0.5f;

            for (int i = 0; i < capturedIhObjs.Length; i++)
            {
                if (capturedIhObjs[i] == null) continue;
                if ((transform.position - capturedIhObjs[i].transform.position).magnitude <= 0.5f)
                {
                    if (capturedIhObjs[i].isItem)
                    {
                        //아이템 기능 실행 함수
                    }
                    else if (!kc.hasInhaledObj)
                    {
                        kc.hasInhaledObj = true;
                        kc.ihObjAbility = capturedIhObjs[i].ability;
                    }
                    else if (kc.hasInhaledObj
                        && kc.ihObjAbility == SpecialAbility.None
                        && capturedIhObjs[i].ability != SpecialAbility.None)
                    {
                        kc.ihObjAbility = capturedIhObjs[i].ability;
                    }

                    //삼킨 오브젝트 삭제
                    kc.PlayReactionXdir();
                    Destroy(capturedIhObjs[i].gameObject);
                    captureCount--;
                    capturedIhObjs[i] = null;
                    continue;
                }
                var toDir = kc.isRightDir ? -1 : 1f;

                var yPos = Mathf.Lerp(capturedIhObjs[i].transform.position.y, transform.position.y + 0.25f, ySpeed / 0.8f);
                var xPos = capturedIhObjs[i].transform.position.x + (toDir * xSpeed * Time.deltaTime);

                xPos = kc.isRightDir ? Mathf.Max(transform.position.x, xPos) : Mathf.Min(transform.position.x, xPos);

                capturedIhObjs[i].transform.position = new Vector3(xPos, yPos);
            }
        }
    }

    public override void Exit()
    {
        StopAllCoroutines();
        kc.spritePivot.localPosition = Vector3.zero;
        kc.isPlayingAction = false;
        kc.lockDir = false;
        kc.hitBox.enabled = true;
        captureCount = 0;
        captureStart = false;
        playAnimation = false;
        endExhale = false;
        xSpeed = 0;
        ySpeed = 0;
        Array.Clear(capturedIhObjs,0, capturedIhObjs.Length);
    }

    IEnumerator StartAnimation()
    {
        playAnimation = true;
        kc.kirbyAnimator.Play("Char_Kirby_Inhaling_OnGround");
        yield return animStartTime;
        playAnimation = false;
    }

    IEnumerator EndAnimation()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Exhaling_OnGround");
        yield return animEndTime;
        kc.GetFSM.SwitchState("Idle");
    }

    IEnumerator EndState()
    {
        kc.kirbyAnimator.Play("Char_Kirby_Inhaling_Stop");  
        yield return animEndTime;
        kc.GetFSM.SwitchState(kc.isGrounded ? "Idle" : "Fall");
    }

    private void OnDrawGizmos()
    {
        if (kc == null) return;
        var minus = kc.isRightDir ? 1 : -1;
        var kirbyPos = transform.position;
        var checkStartPos = kirbyPos + Vector3.right * (minus * checkDist + minus * 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(checkStartPos, new Vector3(checkDist * 2f, 1f, 0f));
    }
}
