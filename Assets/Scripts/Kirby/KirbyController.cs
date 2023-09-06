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

    //������Ʈ
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public LayerMask groundMask;
    public Animator kirbyAnimator;
    public SpriteRenderer kirbySprite;
    public SpriteRenderer colhitSprite;
    public Sprite[] colhitDirSprite; //[0] : �ٴ�, [1] : õ��, [2] : ������,����

    //�Է�
    public float hInput;
    public float vInput;
    public bool jumpInput;
    public bool jumpHoldInput;
    public bool actInput;
    public bool actHoldInput;

    //���� ����
    public bool isGrounded;
    public bool isDash;
    public bool isRightDir;
    public bool isPlayingColHItAnim;
    public bool isWallHit;
    public bool isCellingHit;

    public SpecialAbility ability = SpecialAbility.None;

    //üŷ�� ����
    public float lastTimeJumped;
    public bool playJumpTurn;
    public bool lockDir;

    public float currentXVel;
    public float currentYVel;

    //�뽬 �Է� ��������
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
        #region �Է� ������Ʈ
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
        //Ŀ�� ����üũ
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

        //�׼� Ű ����


        //���� ���� (����üũ ��)
        _fsm.Current.OnPrePhysCheck();

        #region ����üũ

        //�� Ȯ��
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

        //õ�� Ȯ��
        var wasCellingHit = isCellingHit;
        isCellingHit = CheckCellingHit();
        if (isCellingHit)
        {
            if (!wasCellingHit)
            {
                _fsm.Current.OnCellingHit();
            }
        }

        //�� Ȯ��
        var wasGrounded = isGrounded;
        GroundCheck();
        if (!wasGrounded && isGrounded)
        {
            //���� �̺�Ʈ
            _fsm.Current.OnLand();

            //�뽬 ����
            if (isDash && hInput == 0)
            {
                isDash = false;
            }
        }

        #endregion

        //���� ���� (����üũ ��)
        _fsm.Current.OnPostPhysCheck();

        //������ ó��
        _fsm.Current.Excute();
        rb.velocity = new Vector2(currentXVel,currentYVel);

        //�ִϸ��̼� ó��
        kirbySprite.flipX = !isRightDir;

        jumpInput = false;
        actInput = false;
    }

    #region üŷ�� �Լ�

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

        //��Ʈ�Լ� ����
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

    #region �ݹ�

    public void ActDamaged()
    {
        //�ܺο��� OnCollisionEnter()�� ����
        //�ߵ� �� Ŀ���� ������Ʈ�� Damaged�� ��ȯ�ʰ� ���ÿ�
        //Ability�� ������ �־��ٸ� �����Ƽ�� Ǯ����
        //�̶� Ability Star�� ������ Ability Star�� �ش� Ability Ÿ�� ������ �� ����
    }

    public void CalculateXVelocity(float xInput,float maxSpeed,float acceleration,float friction)
    {
        //����
        currentXVel += xInput * acceleration * Time.deltaTime;

        //����
        var minus = currentXVel > 0 ? 1 : -1;
        currentXVel = minus * Mathf.Max(0f, Mathf.Abs(currentXVel) - friction * Time.deltaTime);

        //����
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


