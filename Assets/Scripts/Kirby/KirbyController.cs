using System;
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

    //공용 상태
    public bool isGrounded;
    public bool isDash;
    public bool isRightDir;
    
    //public bool isWallHit;
    //public bool isCellingHit;

    public SpecialAbility ability = SpecialAbility.None;

    //체킹용 변수
    public float lastTimeJumped;

    //입력
    public float hInput;
    public float vInput;
    public bool jumpInput;
    public bool jumpHoldInput;
    public bool actInput;
    public bool actHoldInput;

    //대쉬 입력 보조변수
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
        #endregion

        stateMSG.text = _fsm.Current.GetKey;
    }

    private void FixedUpdate()
    {
        //커비 방향체크
        if (hInput > 0)
        {
            isRightDir = true;
        }
        if (hInput < 0)
        {
            isRightDir = false;
        }

        //전이 실행 (물리체크 전)
        _fsm.Current.OnPrePhysCheck();

        //벽/천장 확인
        //var wasWallHit = isWallHit;
        //WallCheck();
        //if (!wasWallHit && isWallHit)
        //{
        //    _fsm.Current.OnWallHit();
        //}

        //var wasCellingHit = isCellingHit;
        //CellingCheck();
        //if (!wasCellingHit && isCellingHit)
        //{
        //    _fsm.Current.OnCellingHit();
        //}

        //땅 확인
        var wasGrounded = isGrounded;
        GroundCheck();

        #region 착지
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

        jumpInput = false;
        actInput = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundMask && _fsm.Current.interactDamageEffect)
        {
            //콜리전에 따라 꽝 스프라이트 켜고, 현재 스프라이트 끄기
            //코루틴으로 2~3프레임만 그리기
        }
    }

    #region 체킹용 함수

    public void GroundCheck()
    {
        isGrounded = false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(box.size.x * transform.lossyScale.x, box.size.y * transform.lossyScale.y * 0.5f), 0f, Vector2.down, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (Time.time >= lastTimeJumped + 0.2f && raycastHit.collider != null)
        {
            isGrounded = true;
            lastTimeJumped = 0f;
        }
    }

    //private void CellingCheck()
    //{

    //}

    //private void WallCheck()
    //{

    //}

    public void DashCheck()
    {
        if (isDash)
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

    public void ActDamaged()
    {
        //외부에서 OnCollisionEnter()로 실행
        //발동 시 커비의 스테이트가 Damaged로 전환됨과 동시에
        //Ability를 가지고 있었다면 어빌리티가 풀어짐
        //이때 Ability Star를 생성함 Ability Star엔 해당 Ability 타입 정보가 들어가 있음
    }
}


