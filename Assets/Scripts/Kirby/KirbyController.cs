using System;
using System.Collections;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Animations;
using UnityEditor.Timeline;
using UnityEngine;

public enum SpecialAbility
{
    None = 0,
    Beam = 1,
    Spark = 2,
    Fire = 3
}

public class KirbyController : MonoBehaviour
{
    KirbyFSM<string, KirbyState> _fsm;
    public KirbyFSM<string, KirbyState> GetFSM => _fsm;

    // �ִϸ����� (����)
    public AnimatorController[] animController;

    //������Ʈ
    public BoxCollider2D physBox;
    public BoxCollider2D hitBox;
    public BoxCollider2D atkBox;
    public Rigidbody2D rb;
    public LayerMask groundMask;
    public GameObject abilityStarPrefab;
    public Transform spritePivot;
    public Animator kirbyAnimator;
    public SpriteRenderer kirbySprite;
    public SpriteRenderer colhitSprite;
    public Sprite[] colhitDirSprite;
    public GameObject starDustPrefab;
    public GameObject morphFX;

    public SpecialAbility ihObjAbility;

    //�Է�
    public float hInput;
    public float vInput;
    public bool jumpInput;
    public bool jumpHoldInput;
    public bool actInput;
    public bool actHoldInput;
    public bool selectInput;

    //���� ����
    public bool isGrounded;
    public bool isDash;
    public bool isInvincibility; //�۾� �ʿ�
    public bool hasInhaledObj;

    public bool isRightDir;
    public bool isPlayingAction;

    public bool isPlayingColHItAnim;
    public bool isWallHit;
    public bool isCellingHit;

    public bool isStopReadInput;
    public bool isStopExcuteFSM;


    public SpecialAbility ability = SpecialAbility.None;

    //üŷ�� ����
    public float lastTimeJumped;
    public bool lockDir;

    public float currentXVel;
    public float currentYVel;

    //�뽬 �Է� ��������
    public bool dontUseDashInput;
    public bool checkDashInput;
    public int dashInput;
    public float validDashInputTimer;

    Coroutine _collisionAnimCoroutine;
    GameObject _createdAbilityStar;

    //debug
    public TMPro.TMP_Text stateMSG;

    public void Awake()
    {
        _fsm = new KirbyFSM<string, KirbyState>();

        var states = GetComponents<KirbyState>();
        foreach (var state in states)
        {
            state.Initialize(this);
        }

        _fsm.SetState("Idle");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    StartCoroutine("LowDamaged");
        //}

        if (isStopReadInput)
        {
            hInput = 0f;
            vInput = 0f;
            jumpInput = false;
            jumpHoldInput = false;
            actInput = false;
            actHoldInput = false;
            selectInput = false;
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            selectInput = true;
        }

        DashCheck();
        isDash = dontUseDashInput ? false : isDash;
        #endregion

        //Debug�� ���� ���
        if (stateMSG != null) stateMSG.text = _fsm.Current.GetKey;
    }

    private void FixedUpdate()
    {
        if (isStopExcuteFSM)
        {
            rb.velocity = new Vector2(currentXVel, currentYVel);
            return;
        }

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

        //����Ʈ Ű ����
        if (!isPlayingAction && ability != SpecialAbility.None && selectInput)
        {
            // ability star ���� �Լ� ����
            CreateAbilityStar();
            // �� ������� (�⺻���� ���ư��� ��ɸ����)
            var clipName = (kirbyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            ability = SpecialAbility.None;
            ChangeKirbySprite();
            kirbyAnimator.Play(clipName);
        }

        //�׼� Ű ����
        if (!(_fsm.Current.interactActionInput && actInput) || isPlayingAction)
        {
            goto SkipActionExcute;
        }

        PlayAbilityAction();

    SkipActionExcute:


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
        rb.velocity = new Vector2(currentXVel, currentYVel);

        //�ִϸ��̼� ó��
        kirbySprite.flipX = !isRightDir;

        jumpInput = false;
        actInput = false;
        selectInput = false;
    }

    public void CreateAbilityStar()
    {
        var currentInstance = Instantiate(abilityStarPrefab);
        var starMove = currentInstance.GetComponent<AbilityStarMovement>();
        starMove.minus = isRightDir ? -1 : 1;
        starMove.Initialize();
        currentInstance.GetComponent<InhaleableObj>().ability = ability;
        currentInstance.transform.position = transform.position;

        if (_createdAbilityStar != null)
        {
            Destroy(_createdAbilityStar);
        }
        _createdAbilityStar = currentInstance;
    }

    #region üŷ�� �Լ�

    public void GroundCheck()
    {
        isGrounded = false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(physBox.size.x * transform.lossyScale.x, physBox.size.y * transform.lossyScale.y * 0.5f),
            0f, Vector2.down, transform.lossyScale.y * 0.25f + 0.02f, groundMask);
        if (Time.time >= lastTimeJumped + 0.2f && raycastHit.collider != null)
        {
            isGrounded = true;
            lastTimeJumped = 0f;
        }
    }

    public bool CheckCellingHit()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(physBox.size.x * transform.lossyScale.x - 0.02f, physBox.size.y * transform.lossyScale.y * 0.5f),
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, new Vector2(physBox.size.x * transform.lossyScale.x * 0.5f, physBox.size.y * (transform.lossyScale.y)),
            0f, realVector, transform.lossyScale.x * 0.25f + 0.02f, groundMask);
        if (raycastHit.collider != null && Vector2.Dot(-realVector, raycastHit.normal) > 0.7f)
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

    #region �ܺ� ȣ�� �Լ�

    public void ActDamaged()
    {
        //�ܺο��� OnCollisionEnter()�� ����
        //�ߵ� �� Ŀ���� ������Ʈ�� Damaged�� ��ȯ�ʰ� ���ÿ�
        //Ability�� ������ �־��ٸ� �����Ƽ�� Ǯ����
        //�̶� Ability Star�� ������ Ability Star�� �ش� Ability Ÿ�� ������ �� ����
    }

    public void CalculateVelocity(ref float velocity, float input, float maxSpeed, float acceleration, float friction)
    {
        //����
        velocity += input * acceleration * Time.deltaTime;

        //����
        var minus = velocity > 0 ? 1 : -1;
        velocity = minus * Mathf.Max(0f, Mathf.Abs(velocity) - friction * Time.deltaTime);

        //�ִ�ӵ� ����
        velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
    }

    public void PlayCollisionAnimation(int dirNum)
    {
        if (!isPlayingColHItAnim)
        {
            _collisionAnimCoroutine = StartCoroutine("PlayColAnim",dirNum);
        }
    }

    public void ForceStopCollisionAnimation()
    {
        if (_collisionAnimCoroutine != null)
            StopCoroutine(_collisionAnimCoroutine);
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
                colhitSprite.sprite = colhitDirSprite[0 + (int)ability * 2];
                colhitSprite.transform.localPosition = new Vector3(0, -0.5f, 0);
                break;
            case 1:
                colhitSprite.sprite = colhitDirSprite[0 + (int)ability * 2];
                colhitSprite.transform.localPosition = Vector3.zero;
                break;
            case 2:
                colhitSprite.sprite = colhitDirSprite[1 + (int)ability * 2];
                colhitSprite.transform.localPosition = new Vector3(isRightDir ? 0.25f : -0.25f, -0.5f, 0f);
                break;
        }
        colhitSprite.flipX = !isRightDir;

        yield return new WaitForSeconds(0.1f);
        colhitSprite.transform.localPosition = new Vector3(0, -0.5f, 0);

        isPlayingColHItAnim = false;
        colhitSprite.enabled = false;
        kirbySprite.enabled = true;
    }

    //���Ű���
    IEnumerator PlayMorpAct()
    {
        StopInvincible();
        isPlayingAction = true;
        hitBox.enabled = false;
        if(_createdAbilityStar != null) 
            Destroy(_createdAbilityStar);

        yield return new WaitForSeconds(0.15f);
        morphFX.SetActive(true);
        PlayReactionYdir();
        ChangeKirbySprite();
        kirbyAnimator.Play("Char_Kirby_Jumping");

        yield return new WaitForSeconds(0.3f);

        isPlayingAction = false;
        PlayAbilityAction();

        var stopInputTime = 0f;
        isStopReadInput = true;
        //�������� ����,Ű���� ���� �����������
        switch (ability)
        {
            case SpecialAbility.Beam:
                //stopInputTime = 0.6f;
                //break;
            case SpecialAbility.Fire:
                //stopInputTime = 0.6f;
                //break;
            case SpecialAbility.Spark:
                stopInputTime = 0.6f;
                break;
        }

        yield return new WaitForSeconds(stopInputTime);
        morphFX.SetActive(false);
        hitBox.enabled = true;
        isStopReadInput = false;
    }

    public void ChangeAbility()
    {
        hasInhaledObj = false;
        if (ihObjAbility != SpecialAbility.None)
        {
            ability = ihObjAbility;
            StartCoroutine("PlayMorpAct");
        }
        else
        {
            GetFSM.SwitchState("Idle");
        }
        ihObjAbility = SpecialAbility.None;
    }

    public void ChangeKirbySprite()
    {
        switch (ability)
        {
            case SpecialAbility.None:
                kirbyAnimator.runtimeAnimatorController = animController[0];
                break;
            case SpecialAbility.Beam:
                kirbyAnimator.runtimeAnimatorController = animController[1];
                break;
            case SpecialAbility.Spark:
                kirbyAnimator.runtimeAnimatorController = animController[2];
                break;
            case SpecialAbility.Fire:
                kirbyAnimator.runtimeAnimatorController = animController[3];
                break;
        }

        //colhitSprite.color = kirbySprite.color;
    }

    public void PlayAbilityAction()
    {
        var stateString = "";
        switch (ability)
        {
            case SpecialAbility.None:
                stateString = !hasInhaledObj ? "Inhale" : "Exhale";
                break;
            case SpecialAbility.Beam:
                stateString = "Beam";
                break;
            case SpecialAbility.Fire:
                stateString = "Fire";
                break;
            case SpecialAbility.Spark:
                stateString = "Spark";
                break;
        }
        _fsm.SwitchState(stateString);

    }

    public void PlayReactionXdir()
    {
        StartCoroutine("PlayReactX");
    }

    public void PlayReactionYdir()
    {
        StartCoroutine("PlayReactY");
    }

    IEnumerator PlayReactY()
    {
        var count = 0f;
        while (count < 12f)
        {
            count += Time.deltaTime * 56f;
            var yValue = Mathf.Sin(count) * 0.05f;
            spritePivot.localPosition = new Vector3(spritePivot.localPosition.x, yValue, spritePivot.localPosition.z);
            yield return null;
        }

        spritePivot.localPosition = new Vector3(spritePivot.localPosition.x, 0f, spritePivot.localPosition.z);
    }

    IEnumerator PlayReactX()
    {
        var count = 0f;
        while (count < 12f)
        {
            count += Time.deltaTime * 56f;
            var xValue = Mathf.Sin(count) * 0.05f;
            spritePivot.localPosition = new Vector3(xValue ,spritePivot.localPosition.y, spritePivot.localPosition.z);
            yield return null;
        }

        spritePivot.localPosition = new Vector3(0f, spritePivot.localPosition.y, spritePivot.localPosition.z);
    }

    IEnumerator StopReadInput(float time)
    {
        var count = 0f;
        isStopReadInput = true;
        while (count < time)
        {
            count += Time.deltaTime;
            yield return null;
        }
        isStopReadInput = false;
    }

    public void PlayStarDust()
    {
        var star = Instantiate(starDustPrefab);
        star.transform.position = transform.position + Vector3.forward * -2f;
        star.GetComponent<ProjectileMovement>().dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1, 1f), 0).normalized;
    }

    IEnumerator LowDamaged()
    {
        var clipName = (kirbyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        var count = 0f;
        if (ability != SpecialAbility.None)
        {
            CreateAbilityStar();
            ability = SpecialAbility.None;
            ChangeKirbySprite();
        }
        if (isPlayingAction || _fsm.Current.GetKey == "Slide")
        {
            _fsm.SwitchState("Idle");
            clipName = !hasInhaledObj ? "Char_Kirby_Idle" : "Char_Kirby_Inhaled_Idle";
        }
        hitBox.enabled = false;
        kirbyAnimator.Play("Char_Kirby_Hurt_nomal");
        isStopExcuteFSM = true;
        isStopReadInput = true;
        while (count < 0.4f)
        {
            currentXVel = isRightDir ? -2f : 2f;
            currentYVel = 0f;
            count += Time.deltaTime;
            yield return null;
        }
        StartCoroutine("Invincible");
        isStopExcuteFSM = false;
        isStopReadInput = false;
        hitBox.enabled = true;
        kirbyAnimator.Play(clipName);
    }

    IEnumerator Invincible()
    {
        isInvincibility = true;
        var timer = 0f;
        var colorTime = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            colorTime += 18f * Time.deltaTime;
            kirbySprite.color = Color.Lerp(new Color(1f, 0.85f, 0.5f),Color.yellow, Mathf.Sin(colorTime));
            yield return null;
        }
        kirbySprite.color = Color.white;
        isInvincibility = false;
    }

    public void StopInvincible()
    {
        StopCoroutine("Invicible");
        kirbySprite.color = Color.white;
        isInvincibility = false;
    }

    public bool EnterDoor(out bool excuteExhale)
    {
        if(isStopExcuteFSM || isPlayingAction)
        {
            excuteExhale = false;
            return false;
        }
        excuteExhale = hasInhaledObj || _fsm.Current.GetKey == "Hover";
        StopAllCoroutines();
        StartCoroutine("DoorAction");
        return true;
    }

    IEnumerator DoorAction()
    {
        hitBox.enabled = false;
        isStopExcuteFSM = true;
        isStopReadInput = true;
        currentXVel = 0f;
        currentYVel = 0f;
        if (hasInhaledObj)
        {
            _fsm.SwitchState("Exhale");
            yield return new WaitForSeconds(0.3f);
            _fsm.SwitchState("Idle");
        }
        else if(_fsm.Current.GetKey == "Hover")
        {
            actInput = true;
            _fsm.Current.OnPostPhysCheck();
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            _fsm.SwitchState("Idle");
        }
       
        kirbyAnimator.Play("Char_Kirby_Enter");
        kirbyAnimator.Update(0f);
        yield return new WaitForSeconds(0.3f);
        _fsm.SwitchState("Idle");
        kirbySprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.5f);
        kirbySprite.color = new Color(1, 1, 1, 1);
        isStopExcuteFSM = false;
        isStopReadInput = false;
        hitBox.enabled = true;
    }
    #endregion



}


