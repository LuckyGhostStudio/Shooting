using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //添加刚体组件
public class Player : Character
{
    [SerializeField] private StateBarHUD stateBarHUD;
    [SerializeField] private bool regenerateHealth = true;      //是否可持续回血
    [SerializeField] private float healthRegenerateTime;        //回血间隔时间
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;     //回血百分比

    [Header("Input")]

    [SerializeField] private PlayerInput input;

    [Header("Move")]

    [SerializeField] private float moveSpeed = 10f;             //移动速度
    [SerializeField] private float accelerateTime = 3f, decelerateTime = 3f;     //加速时间 减速时间
    [SerializeField] private float moveRotationAngle = 50f;

    [SerializeField] private float paddingX = 0.8f, paddingY = 0.22f;  //Player中心到边框距离

    [Header("Fire")]

    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;  //子弹预制体
    [SerializeField] private Transform muzzleMiddle, muzzleTop, muzzleBottom;   //枪口

    [SerializeField, Range(0,2)] private int weaponPower = 0;   //武器威力
    [SerializeField] private float fireInterval = 0.2f;         //开火间隔时间

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

        //订阅事件
        input.onMove += Move;           //移动
        input.onStopMove += StopMove;   //停止移动
        input.onFire += Fire;           //开火
        input.onStopFire += StopFire;   //停止开火
    }

    private void OnDisable()
    {
        //取消订阅
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;           //开火
        input.onStopFire -= StopFire;   //停止开火
    }


    void Start()
    {
        rigidbody2d.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        stateBarHUD.Initialize(health, maxHealth);  //初始化血条

        input.EnableGameplayInput();    //启用Gameplay输入
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        stateBarHUD.UpdateState(health, maxHealth); //更新血条

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)   //可持续回血
            {
                //携程不为空，停用旧协程
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));   //回血
            }
        }
    }

    public override void RestoreHealth(int value)
    {
        base.RestoreHealth(value);
        stateBarHUD.UpdateState(health, maxHealth);     //更新血条
    }

    public override void Die()
    {
        stateBarHUD.UpdateState(health, maxHealth);     //更新血条
        base.Die();
    }

    void Update()
    {
        
    }

    #region MOVE
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="moveInput">输入信号</param>
    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);   //停止旧协程
        }

        //Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        //变速移动
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerateTime, moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));    
        StartCoroutine(MovePositionLimitCoroutine());
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    private void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);   //停止旧协程
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerateTime, Vector2.zero, Quaternion.identity));    //减速到0
        StopCoroutine(MovePositionLimitCoroutine());
    }

    /// <summary>
    /// 变速移动、旋转
    /// </summary>
    /// <param name="time">变速时间</param>
    /// <param name="moveVelocity">目标速度</param>
    /// <param name="moveRotation">目标旋转</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity, moveVelocity, t);     //变速
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);      //沿x轴旋转
            yield return null;
        }
    }

    /// <summary>
    /// 限制移动位置
    /// </summary>
    /// <returns></returns>
    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMovablePosition(transform.position, paddingX, paddingY);   //限制Player移动范围
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
                    PoolManager.Release(projectile1, muzzleMiddle.position);    //生成子弹1
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);       //生成子弹1
                    PoolManager.Release(projectile1, muzzleBottom.position);    //生成子弹1
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);    //生成子弹1
                    PoolManager.Release(projectile2, muzzleTop.position);       //生成子弹2
                    PoolManager.Release(projectile3, muzzleBottom.position);    //生成子弹3
                    break;
            }

            yield return waitForFireInterval;   //等待开火间隔时间
        }
    }
    #endregion
}
