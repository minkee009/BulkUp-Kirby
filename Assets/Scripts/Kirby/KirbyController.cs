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

    //������Ʈ
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public LayerMask groundMask;
    public Animator kirbyAnimator;
    public SpriteRenderer kirbySprite;

    //���� ����
    public bool isGrounded;
    public bool isDash;
    public bool isRightDir;
    
    //public bool isWallHit;
    //public bool isCellingHit;

    public SpecialAbility ability = SpecialAbility.None;

    //üŷ�� ����
    public float lastTimeJumped;

    //�Է�
    public float hInput;
    public float vInput;
    public bool jumpInput;
    public bool jumpHoldInput;
    public bool actInput;
    public bool actHoldInput;

    //�뽬 �Է� ��������
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
        #endregion

        stateMSG.text = _fsm.Current.GetKey;
    }

    private void FixedUpdate()
    {
        //Ŀ�� ����üũ
        if (hInput > 0)
        {
            isRightDir = true;
        }
        if (hInput < 0)
        {
            isRightDir = false;
        }

        //���� ���� (����üũ ��)
        _fsm.Current.OnPrePhysCheck();

        //��/õ�� Ȯ��
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

        //�� Ȯ��
        var wasGrounded = isGrounded;
        GroundCheck();

        #region ����
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

        jumpInput = false;
        actInput = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundMask && _fsm.Current.interactDamageEffect)
        {
            //�ݸ����� ���� �� ��������Ʈ �Ѱ�, ���� ��������Ʈ ����
            //�ڷ�ƾ���� 2~3�����Ӹ� �׸���
        }
    }

    #region üŷ�� �Լ�

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
        //�ܺο��� OnCollisionEnter()�� ����
        //�ߵ� �� Ŀ���� ������Ʈ�� Damaged�� ��ȯ�ʰ� ���ÿ�
        //Ability�� ������ �־��ٸ� �����Ƽ�� Ǯ����
        //�̶� Ability Star�� ������ Ability Star�� �ش� Ability Ÿ�� ������ �� ����
    }
}


