using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //��Ӹ������
public class Player : Character
{
    [SerializeField] private StateBarHUD stateBarHUD;           //UI����Ѫ��
    [SerializeField] private bool regenerateHealth = true;      //�Ƿ�ɳ�����Ѫ
    [SerializeField] private float healthRegenerateTime;        //��Ѫ���ʱ��
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;     //��Ѫ�ٷֱ�

    [Header("Input")]

    [SerializeField] private PlayerInput input;

    [Header("Move")]

    [SerializeField] private float moveSpeed = 10f;                             //�ƶ��ٶ�
    [SerializeField] private float accelerateTime = 3f, decelerateTime = 3f;    //����ʱ�� ����ʱ��
    [SerializeField] private float moveRotationAngle = 50f;                     //�ƶ�ʱת���Ƕ�

    [SerializeField] private float paddingX = 0.8f, paddingY = 0.22f;  //Player���ĵ��߿����

    [Header("Fire")]

    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;  //�ӵ�Ԥ����
    [SerializeField] private Transform muzzleMiddle, muzzleTop, muzzleBottom;   //ǹ��

    [SerializeField] private AudioData projectileLaunchSFX;       //�ӵ�������Ч����

    [SerializeField, Range(0, 2)] private int weaponPower = 0;      //���������ȼ�
    [SerializeField] private float fireInterval = 0.2f;             //������ʱ��

    [Header("Dodge")]

    [SerializeField] private AudioData dodgeSFX;    //������Ч

    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;      //������������ֵ

    [SerializeField] private float maxRoll = 720f;  //����ת�Ƕ�
    [SerializeField] private float rollSpeed = 360; //��ת�ٶ� ��/s
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);    //��������ֵ

    [Header("Overdrive")]

    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overdriveFireFactor = 1.2f;

    private float currentRoll;          //��ǰ��ת��
    private float dodgeDuration;        //���ܳ���ʱ��

    private bool isDodging = false;     //�Ƿ���������
    private bool isOverdriving = false; //�Ƿ�����������״̬

    Vector2 previousVelocity;
    Quaternion previousRotation;

    WaitForFixedUpdate waitForFixedUpdate;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waitHealthRegenerateTime;

    private Rigidbody2D rigidbody2d;
    private Collider2D collider2d;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;

        rigidbody2d.gravityScale = 0;

        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);  //��������ʱ�Ŀ�����
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�����¼�
        input.onMove += Move;           //�ƶ�
        input.onStopMove += StopMove;   //ֹͣ�ƶ�
        input.onFire += Fire;           //����
        input.onStopFire += StopFire;   //ֹͣ����
        input.onDodge += Dodge;         //����
        input.onOverdrive += Overdrive; //��������

        PlayerOverdrive.on += OverdriveOn;      //������������
        PlayerOverdrive.off += OverdriveOff;    //���������ر�
    }

    private void OnDisable()
    {
        //ȡ������
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;           //����
        input.onStopFire -= StopFire;   //ֹͣ����
        input.onDodge -= Dodge;         //����
        input.onOverdrive -= Overdrive; //��������

        PlayerOverdrive.on -= OverdriveOn;      //������������
        PlayerOverdrive.off -= OverdriveOff;    //���������ر�
    }


    void Start()
    {
        stateBarHUD.Initialize(health, maxHealth);  //��ʼ��Ѫ��

        input.EnableGameplayInput();    //����Gameplay����
    }

    #region HEALTH
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        stateBarHUD.UpdateState(health, maxHealth); //����Ѫ��

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)   //�ɳ�����Ѫ
            {
                //Я�̲�Ϊ�գ�ͣ�þ�Э��
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));   //��Ѫ
            }
        }
    }

    public override void RestoreHealth(int value)
    {
        base.RestoreHealth(value);
        stateBarHUD.UpdateState(health, maxHealth);     //����Ѫ��
    }

    public override void Die()
    {
        stateBarHUD.UpdateState(health, maxHealth);     //����Ѫ��
        base.Die();
    }
    #endregion 

    #region MOVE
    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="moveInput">�����ź�</param>
    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);   //ֹͣ��Э��
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        //�����ƶ�
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerateTime, moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    /// <summary>
    /// ֹͣ�ƶ�
    /// </summary>
    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);   //ֹͣ��Э��
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerateTime, Vector2.zero, Quaternion.identity));    //���ٵ�0
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }

    /// <summary>
    /// �����ƶ�����ת
    /// </summary>
    /// <param name="time">����ʱ��</param>
    /// <param name="moveVelocity">Ŀ���ٶ�</param>
    /// <param name="moveRotation">Ŀ����ת</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        previousVelocity = rigidbody2d.velocity;
        previousRotation = transform.rotation;

        float t = 0f;
        while (t < 1)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody2d.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);         //����
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);        //��x����ת
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// �����ƶ�λ��
    /// </summary>
    /// <returns></returns>
    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMovablePosition(transform.position, paddingX, paddingY);   //����Player�ƶ���Χ
            yield return null;
        }
    }
    #endregion

    #region FIRE
    private void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);    //�����ӵ�1
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);       //�����ӵ�1
                    PoolManager.Release(projectile1, muzzleBottom.position);    //�����ӵ�1
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);    //�����ӵ�1
                    PoolManager.Release(projectile2, muzzleTop.position);       //�����ӵ�2
                    PoolManager.Release(projectile3, muzzleBottom.position);    //�����ӵ�3
                    break;
            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);    //���������Ч

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;    //�����Ƿ��������� �ȴ���Ӧ������ʱ��
        }
    }
    #endregion

    #region DODGE

    /// <summary>
    /// ����
    /// </summary>
    private void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;  //��������||����ֵ����
        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;   //���ܿ�ʼ
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);  //����������Ч
        PlayerEnergy.Instance.Use(dodgeEnergyCost);     //��������ֵ

        collider2d.isTrigger = true;    //Player��ײ����Ϊ�������������ӵ��޷��������޵�״̬��

        currentRoll = 0f;

        //Player��x��ת��
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);  //����Player��תֵΪ��ǰ��ת�Ƕ� ��x��
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);  //����Player��С�����������߲�ֵ��

            yield return null;
        }

        collider2d.isTrigger = false;   //ȡ���޵�״̬
        isDodging = false;  //���ܽ���
    }
    #endregion

    #region OVERDRIVE
    /// <summary>
    /// ��������
    /// </summary>
    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();    //������ ������������
    }

    /// <summary>
    /// ������������
    /// </summary>
    private void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;    //����������������
        moveSpeed *= overdriveSpeedFactor;          //�ƶ��ٶ�����

    }

    /// <summary>
    /// ������������
    /// </summary>
    private void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;    //�����������ļ���
        moveSpeed /= overdriveSpeedFactor;          //�ƶ��ٶȻָ�
    }

    #endregion
}
