using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum SpecialAbility
{
    None = 0,
}

public class KirbyController : MonoBehaviour
{
    KirbyFSM<string, KirbyState> _fsm;
    public KirbyFSM<string,KirbyState> GetFSM => _fsm;

    //컴포넌트
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public LayerMask groundMask;
    public Animator kirbyAnimator;
    public SpriteRenderer kirbySprite;
    public SpriteRenderer colhitSprite;
    public Sprite[] colhitDirSprite; //[0] : 바닥, [1] : 천장, [2] : 오른쪽,왼쪽

    //입력
    public float hInput;
    public float vInput;
    public bool jumpInput;
    public bool jumpHoldInput;
    public bool actInput;
    public bool actHoldInput;

    //공용 상태
    public bool isGrounded;
    public bool isDash;
    public bool isRightDir;
    public bool isPlayingColHItAnim;
    public bool isWallHit;
    public bool isCellingHit;

    public SpecialAbility ability = SpecialAbility.None;

    //체킹용 변수
    public float lastTimeJumped;
    public bool playJumpTurn;
    public bool lockDir;

    public float currentXVel;
    public float currentYVel;

    //대쉬 입력 보조변수
    public bool dontUseDashInput;
    public bool checkDashInput;
    public int dashInput;
    public float validDashInputTimer;


    //debug
    public TMPro.TMP_Text stateMSG;

    public void Awake()
    {
        _fsm = new KirbyFSM<string, KirbyState>();

        var states = GetComponents<KirbyState>();
        foreach ( var state in states )
        {
            state.Initialize(this);
        }

        _fsm.SetState("Idle");
    }

    private void Update()
    {
        #region 입력 업데이트
        hInput = (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f) + (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        vInput = (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f) + (Input.GetKey(KeyCode.UpArrow) ? 1f : 0f);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpInput = true;
        }
        
        jumpHoldInput = Input.GetKey(KeyCode.Z);

        if (Input.GetKeyDown(KeyCode.X))
        {
            actInput = true;
        }

        actHoldInput = Input.GetKey(KeyCode.X);

        DashCheck();
        isDash = dontUseDashInput ? false : isDash;
        #endregion

        stateMSG.text = _fsm.Current.GetKey;
    }

    private void FixedUpdate()
    {
        //커비 방향체크
        if (!lockDir)
        {
            if (hInput > 0)
            {
                isRightDir = true;
            }
            if (hInput < 0)
            {
                isRightDir = false;
            }
        }

        //액션 키 실행


        //전이 실행 (물리체크 전)
        _fsm.Current.OnPrePhysCheck();

        #region 물리체크

        //벽 확인
        var wasWallHit = isWallHit;
        isWallHit = CheckWallhit(isRightDir);
        if (isWallHit)
        {
            hInput = 0;
            isDash = false;
            if (!wasWallHit)
            {
                _fsm.Current.OnWallHit();
                currentXVel = 0f;
            }
        }

        //천장 확인
        var wasCellingHit = isCellingHit;
        isCellingHit = CheckCellingHit();
        if (isCellingHit)
        {
            if (!wasCellingHit)
            {
                _fsm.Current.OnCellingHit();
            }
        }

        //땅 확인
        var wasGrounded = isGrounded;
        GroundCheck();
        if (!wasGrounded && isGrounded)
        {
            //착지 이벤트
            _fsm.Current.OnLand();

            //대쉬 조절
            if (isDash && hInput == 0)
            {
                isDash = false;
            }
        }

        #endregion

        //전이 실행 (물리체크 후)
        _fsm.Current.OnPostPhysCheck();

        //움직임 처리
        _fsm.Current.Excute();
        rb.velocity = new Vector2(currentXVel,currentYVel);

        //애니메이션 처리
        kirbySprite.flipX = !isRightDir;

        jumpInput = false;
        actInput = false;
    }

    #region 체킹용 함수

    public void GroundCheck()
    {
        isGrounded = false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(box.size.x * transform.lossyScale.x, box.size.y * transform.lossyScale.y * 0.5f), 
            0f, Vector2.down, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (Time.time >= lastTimeJumped + 0.2f && raycastHit.collider != null)
        {
            isGrounded = true;
            lastTimeJumped = 0f;
        }
    }

    public bool CheckCellingHit()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(box.size.x * transform.lossyScale.x, box.size.y * transform.lossyScale.y * 0.5f), 
            0f, Vector2.up, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null)
        {
            return true;
        }
        return false;
    }

    public bool CheckWallhit(bool rightDir)
    {
        var realVector = rightDir ? Vector2.right : Vector2.left;

        //히트함수 수정
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(box.size.x * transform.lossyScale.x * 0.5f, box.size.y * (transform.lossyScale.y)),
            0f, realVector, transform.lossyScale.x * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null && Vector2.Dot(-realVector,raycastHit.normal) > 0.7f)
        {
            return true;
        }
        return false;
    }

    public void DashCheck()
    {
        if (isDash && dontUseDashInput)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!checkDashInput)
                checkDashInput = true;
            else
            {
                if (dashInput == 2)
                {
                    isDash = true;
                    checkDashInput = false;
                    validDashInputTimer = 0;
                    dashInput = 0;
                    return;
                }
            }
            dashInput = 2;
            validDashInputTimer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!checkDashInput)
                checkDashInput = true;
            else
            {
                if (dashInput == 1)
                {
                    isDash = true;
                    checkDashInput = false;
                    validDashInputTimer = 0;
                    dashInput = 0;
                    return;
                }
            }
            dashInput = 1;
            validDashInputTimer = 0f;
        }

        if (checkDashInput)
        {
            validDashInputTimer += Time.deltaTime;
        }

        if (validDashInputTimer > 0.6f)
        {
            dashInput = 0;
            validDashInputTimer = 0f;
            checkDashInput = false;
        }
    }

    #endregion

    #region 콜백

    public void ActDamaged()
    {
        //외부에서 OnCollisionEnter()로 실행
        //발동 시 커비의 스테이트가 Damaged로 전환됨과 동시에
        //Ability를 가지고 있었다면 어빌리티가 풀어짐
        //이때 Ability Star를 생성함 Ability Star엔 해당 Ability 타입 정보가 들어가 있음
    }

    public void CalculateXVelocity(float xInput,float maxSpeed,float acceleration,float friction)
    {
        //가속
        currentXVel += xInput * acceleration * Time.deltaTime;

        //감속
        var minus = currentXVel > 0 ? 1 : -1;
        currentXVel = minus * Mathf.Max(0f, Mathf.Abs(currentXVel) - friction * Time.deltaTime);

        //최종
        currentXVel = Mathf.Clamp(currentXVel, -maxSpeed, maxSpeed);
    }

    public void CalculateYVelocity(float gravityForce,float sharpness)
    {
        currentYVel = Mathf.Lerp(currentYVel, -gravityForce, sharpness * Time.deltaTime);
    }

    public void PlayCollisionAnimation(int dirNum)
    {
        if(!isPlayingColHItAnim)
            StartCoroutine(PlayColAnim(dirNum));
    }

    public void ForceStopCollisionAnimation()
    {
        StopCoroutine(PlayColAnim(0));
        isPlayingColHItAnim = false;
        colhitSprite.enabled = false;
        kirbySprite.enabled = true;
    }

    IEnumerator PlayColAnim(int dirNum)
    {
        colhitSprite.enabled = true;
        kirbySprite.enabled = false;
        isPlayingColHItAnim = true;

        switch (dirNum)
        {
            case 0:
                colhitSprite.sprite = colhitDirSprite[0];
                break;
            case 1:
                colhitSprite.sprite = colhitDirSprite[1];
                colhitSprite.transform.localPosition = Vector3.zero;
                break;
            case 2:
                colhitSprite.sprite = colhitDirSprite[2];
                colhitSprite.transform.localPosition = new Vector3(isRightDir ? 0.5f : -0.5f, -0.5f, 0f);
                break;
        }
        colhitSprite.flipX = !isRightDir;

        yield return new WaitForSeconds(0.1f);
        colhitSprite.transform.localPosition = new Vector3(0,-0.5f,0);

        isPlayingColHItAnim = false;
        colhitSprite.enabled = false;
        kirbySprite.enabled = true;
    }

    #endregion
}


