using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //��Ӹ������
public class Player : Character
{
    [SerializeField] private StateBarHUD stateBarHUD;
    [SerializeField] private bool regenerateHealth = true;      //�Ƿ�ɳ�����Ѫ
    [SerializeField] private float healthRegenerateTime;        //��Ѫ���ʱ��
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;     //��Ѫ�ٷֱ�

    [Header("Input")]

    [SerializeField] private PlayerInput input;

    [Header("Move")]

    [SerializeField] private float moveSpeed = 10f;             //�ƶ��ٶ�
    [SerializeField] private float accelerateTime = 3f, decelerateTime = 3f;     //����ʱ�� ����ʱ��
    [SerializeField] private float moveRotationAngle = 50f;

    [SerializeField] private float paddingX = 0.8f, paddingY = 0.22f;  //Player���ĵ��߿����

    [Header("Fire")]

    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;  //�ӵ�Ԥ����
    [SerializeField] private Transform muzzleMiddle, muzzleTop, muzzleBottom;   //ǹ��

    [SerializeField, Range(0,2)] private int weaponPower = 0;   //��������
    [SerializeField] private float fireInterval = 0.2f;         //������ʱ��

    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitHealthRegenerateTime;

    private Rigidbody2D rigidbody2d;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //�����¼�
        input.onMove += Move;           //�ƶ�
        input.onStopMove += StopMove;   //ֹͣ�ƶ�
        input.onFire += Fire;           //����
        input.onStopFire += StopFire;   //ֹͣ����
    }

    private void OnDisable()
    {
        //ȡ������
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;           //����
        input.onStopFire -= StopFire;   //ֹͣ����
    }


    void Start()
    {
        rigidbody2d.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        stateBarHUD.Initialize(health, maxHealth);  //��ʼ��Ѫ��

        input.EnableGameplayInput();    //����Gameplay����
    }

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

    void Update()
    {
        
    }

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
        StartCoroutine(MovePositionLimitCoroutine());
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
        StopCoroutine(MovePositionLimitCoroutine());
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
        float t = 0f;
        while (t < 1)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity, moveVelocity, t);     //����
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);      //��x����ת
            yield return null;
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

            yield return waitForFireInterval;   //�ȴ�������ʱ��
        }
    }
    #endregion
}
