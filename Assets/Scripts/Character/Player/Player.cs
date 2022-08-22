using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]     //添加刚体组件
public class Player : Character
{
    [SerializeField] private StateBarHUD stateBarHUD;           //UI界面血条
    [SerializeField] private bool regenerateHealth = true;      //是否可持续回血
    [SerializeField] private float healthRegenerateTime;        //回血间隔时间
    [SerializeField, Range(0f, 1f)] private float healthRegeneratePercent;     //回血百分比

    [Header("Input")]

    [SerializeField] private PlayerInput input;

    [Header("Move")]

    [SerializeField] private float moveSpeed = 10f;                             //移动速度
    [SerializeField] private float accelerateTime = 3f, decelerateTime = 3f;    //加速时间 减速时间
    [SerializeField] private float moveRotationAngle = 50f;                     //移动时转动角度

    [Header("Fire")]

    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;            //普通子弹预制体
    [SerializeField] private GameObject projectileOverdrive;    //能量爆发子弹 
    [SerializeField] private Transform muzzleMiddle, muzzleTop, muzzleBottom;   //枪口

    [SerializeField] private AudioData projectileLaunchSFX;       //子弹发射音效数据

    [SerializeField, Range(0, 2)] private int weaponPower = 0;      //武器威力等级
    [SerializeField] private float fireInterval = 0.2f;             //开火间隔时间

    [Header("Dodge")]

    [SerializeField] private AudioData dodgeSFX;    //闪避音效

    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;      //闪避消耗能量值

    [SerializeField] private float maxRoll = 720f;  //最大滚转角度
    [SerializeField] private float rollSpeed = 360; //滚转速度 度/s
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);    //闪避缩放值

    [Header("Overdrive")]

    [SerializeField] private int overdriveDodgeFactor = 2;
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overdriveFireFactor = 1.2f;

    //Player中心到边框距离
    private float paddingX;
    private float paddingY;  

    private float currentRoll;          //当前滚转角
    private float dodgeDuration;        //闪避持续时间

    private bool isDodging = false;     //是否正在闪避
    private bool isOverdriving = false; //是否处于能量爆发状态

    private float slowMotionDuration = 1f;   //子弹时间持续时间

    Vector2 moveDirection;
    Vector2 previousVelocity;
    Quaternion previousRotation;

    WaitForFixedUpdate waitForFixedUpdate;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waitHealthRegenerateTime;
    WaitForSeconds waitForDecelerationTime;

    private Rigidbody2D rigidbody2d;
    private Collider2D collider2d;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    MissileSystem missile;  //导弹系统

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;   //Player模型的尺寸
        paddingX = size.x / 2;
        paddingY = size.y / 2;

        dodgeDuration = maxRoll / rollSpeed;

        rigidbody2d.gravityScale = 0;

        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);  //能量爆发时的开火间隔
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForDecelerationTime = new WaitForSeconds(decelerateTime);       //等待减速时间
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //订阅事件
        input.onMove += Move;           //移动
        input.onStopMove += StopMove;   //停止移动
        input.onFire += Fire;           //开火
        input.onStopFire += StopFire;   //停止开火
        input.onDodge += Dodge;         //闪避
        input.onOverdrive += Overdrive; //能量爆发
        input.onLaunchMissile += LaunchMissile; //发射导弹

        PlayerOverdrive.on += OverdriveOn;      //能量爆发开启
        PlayerOverdrive.off += OverdriveOff;    //能量爆发关闭
    }

    private void OnDisable()
    {
        //取消订阅
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;           //开火
        input.onStopFire -= StopFire;   //停止开火
        input.onDodge -= Dodge;         //闪避
        input.onOverdrive -= Overdrive; //能量爆发
        input.onLaunchMissile -= LaunchMissile; //发射导弹

        PlayerOverdrive.on -= OverdriveOn;      //能量爆发开启
        PlayerOverdrive.off -= OverdriveOff;    //能量爆发关闭
    }


    void Start()
    {
        stateBarHUD.Initialize(health, maxHealth);  //初始化血条

        input.EnableGameplayInput();    //启用Gameplay输入
    }

    #region HEALTH
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        stateBarHUD.UpdateState(health, maxHealth); //更新血条

        if (gameObject.activeSelf)
        {
            Move(moveDirection);    //抵消与敌人子弹碰撞力

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
        GameManager.onGameOver?.Invoke();       //不为空则调用onGameOver处理方法
        GameManager.GameState = GameState.GameOver;     //GameOver
        stateBarHUD.UpdateState(0, maxHealth);     //更新血条
        base.Die();
    }
    #endregion 

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
        moveDirection = moveInput.normalized;
        //变速移动
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerateTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));   //停止减速等待
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
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
        StartCoroutine(nameof(DecelerationCoroutine));  //启用减速等待协程
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
        previousVelocity = rigidbody2d.velocity;
        previousRotation = transform.rotation;

        float t = 0f;
        while (t < 1)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody2d.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);         //变速
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);        //沿x轴旋转
            yield return waitForFixedUpdate;
        }
    }

    /// <summary>
    /// 限制移动范围
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRangeLimitationCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMovablePosition(transform.position, paddingX, paddingY);   //限制Player移动范围
            yield return null;
        }
    }

    /// <summary>
    /// 减速等待
    /// </summary>
    /// <returns></returns>
    IEnumerator DecelerationCoroutine()
    {
        yield return waitForDecelerationTime;   //等待减速时间结束

        StopCoroutine(nameof(MoveRangeLimitationCoroutine));    //停止限制移动协程
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
                    PoolManager.Release(Projectile(projectile1), muzzleMiddle.position);    //生成子弹1
                    break;
                case 1:
                    PoolManager.Release(Projectile(projectile1), muzzleTop.position);       //生成子弹1
                    PoolManager.Release(Projectile(projectile1), muzzleBottom.position);    //生成子弹1
                    break;
                case 2:
                    PoolManager.Release(Projectile(projectile1), muzzleMiddle.position);    //生成子弹1
                    PoolManager.Release(Projectile(projectile2), muzzleTop.position);       //生成子弹2
                    PoolManager.Release(Projectile(projectile3), muzzleBottom.position);    //生成子弹3
                    break;
            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);    //播放射击音效

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;    //根据是否能量爆发 等待相应开火间隔时间
        }
    }

    /// <summary>
    /// 根据是否过速返回对应子弹
    /// </summary>
    /// <param name="projectile"></param>
    /// <returns></returns>
    private GameObject Projectile(GameObject projectile) => isOverdriving ? projectileOverdrive : projectile;

    #endregion

    #region DODGE

    /// <summary>
    /// 闪避
    /// </summary>
    private void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;  //正在闪避||能量值不足
        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;   //闪避开始
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);  //播放闪避音效
        PlayerEnergy.Instance.Use(dodgeEnergyCost);     //消耗能量值

        collider2d.isTrigger = true;    //Player碰撞体设为触发器，敌人子弹无法碰到（无敌状态）

        currentRoll = 0f;

        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);     //子弹时间：时间流速减小增大

        //Player沿x轴转动
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);  //设置Player旋转值为当前滚转角度 沿x轴
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);  //缩放Player大小（贝塞尔曲线插值）

            yield return null;
        }

        collider2d.isTrigger = false;   //取消无敌状态
        isDodging = false;  //闪避结束
    }
    #endregion

    #region OVERDRIVE
    /// <summary>
    /// 能量爆发
    /// </summary>
    private void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();    //能量满 开启能量爆发
    }

    /// <summary>
    /// 开启能量爆发
    /// </summary>
    private void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;    //翻滚能量消耗增加
        moveSpeed *= overdriveSpeedFactor;          //移动速度增加
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);       //子弹时间：时间流逝速度改变
    }

    /// <summary>
    /// 结束能量爆发
    /// </summary>
    private void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;    //翻滚能量消耗减少
        moveSpeed /= overdriveSpeedFactor;          //移动速度恢复
    }

    #endregion

    /// <summary>
    /// 发射导弹
    /// </summary>
    private void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);   //发射导弹
    }
}
