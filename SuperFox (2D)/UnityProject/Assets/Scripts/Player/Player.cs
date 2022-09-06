using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : MonoBehaviour
{
    Animator animator;
    new Rigidbody2D rigidbody2D;
    Collider2D collider2DBottom;
    Collider2D collider2DTop;
    LayerMask ground;
    LayerMask enemies;
    Transform topPoint, bottomPoint;
    Transform deadLine;
    AudioSource jumpAudio;
    AudioSource hurtAudio;

    Vector3 restartPoint;
    GameObject canvas;
    Text cherryNumber, gemNumber, healthValue;
    Image cdImage;

    CinemachineVirtualCamera camera_1;

    PolygonCollider2D background, hiddenBackground;

    private float speed = 350f;
    private float jumpForce = 555f;

    public static int cherry, gem;
    private int health;

    static public bool isDead;
    private bool isProtected, isHurt, isDashing;

    private bool doubleJump, ableToDash;
    private int jumpCount;
    private bool toJump;

    private float dashCoolDownSet;
    private float dashLeftTime, dashSpeed = 50, dashCoolDown;

    public static List<string> collected = new List<string>();

    private void Awake()
    {
        camera_1 = GameObject.Find("Camera_1").GetComponent<CinemachineVirtualCamera>();

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2DTop = GetComponent<CapsuleCollider2D>();
        collider2DBottom = GetComponent<CircleCollider2D>();

        topPoint = transform.Find("TopPoint").gameObject.transform;
        bottomPoint = transform.Find("BottomPoint").gameObject.transform;
        if (GameObject.Find("DeadLine") != null) deadLine = GameObject.Find("DeadLine").gameObject.transform;

        ground = LayerMask.GetMask("Ground");//获得地面Ground的Layer序号
        enemies = LayerMask.GetMask("Enemies");

        jumpAudio = GetComponents<AudioSource>()[1];
        hurtAudio = GetComponents<AudioSource>()[2];

        canvas = GameObject.Find("LevelCanvas").gameObject;
        cherryNumber = canvas.transform.Find("Cherry").gameObject.transform.Find("Value").GetComponent<Text>();
        gemNumber = canvas.transform.Find("Gem").gameObject.transform.Find("Value").GetComponent<Text>();
        healthValue = canvas.transform.Find("Health").gameObject.transform.Find("Value").GetComponent<Text>();
        cdImage = canvas.transform.Find("CoolDown").gameObject.transform.Find("Image").GetComponent<Image>();

        if (SceneManager.GetActiveScene().buildIndex == 1)//第一关
        {
            background = GameObject.Find("Background_").gameObject.GetComponent<PolygonCollider2D>();
            hiddenBackground = GameObject.Find("Background_").transform.Find("HiddenBackground").gameObject.GetComponent<PolygonCollider2D>();
        }

        if (GameObject.Find("RestartPoint")) restartPoint = GameObject.Find("RestartPoint").transform.position;
    }

    private void Start()
    {
        //初始化关卡数据
        SaveController.Instance.LoadLevelInformation(SceneManager.GetActiveScene().buildIndex);
        //获取技能信息
        doubleJump = SaveController.Instance.save.doubleJump;
        ableToDash = SaveController.Instance.save.dash;


        //根据已有技能进行相应设置
        if (ableToDash)
        {
            dashCoolDown = 0;
            cdImage.fillAmount = 0;
            StartCoroutine(DashCoolDownUI());

            if (SaveController.Instance.save.levelPass[3]) dashCoolDownSet = 2f;
            else dashCoolDownSet = 10f;
        }
        else cdImage.fillAmount = 1;

        if (doubleJump) jumpCount = 2; else jumpCount = 1;



        //初始化数据
        cherry = gem = 0;//只记录当前关卡收集的数量
        health = SaveController.Instance.save.life;
        isDead = isDashing = isHurt = false;
        toJump = false;
        collected.Clear();

        //初始化UI
        cherryNumber.text = cherry.ToString();
        gemNumber.text = gem.ToString();
        healthValue.text = health.ToString() + "/" + SaveController.Instance.save.life.ToString();

        //初始化相机
        if (background != null) camera_1.GetComponent<CinemachineConfiner>().m_BoundingShape2D = background;

        //启动需要的协程
        StartCoroutine("CheckDead");
    }


    private void FixedUpdate()
    {
        Dash();
        if (isDashing) return;//冲锋过程中不受其他控制

        if (!isDead)
        {
            HorizontalMove();
            VerticalMove();
        }
    }

    private void Update()
    {
        JumpCheck();
        DashCheck();
    }

    //注意角色左右移动时偶尔会产生Y方向上的速度

    //纵向移动和横向移动分写两个函数！！

    private void HorizontalMove()//横向移动  
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        if (!isHurt)
        {
            //左右移动
            float horizontal = Input.GetAxis("Horizontal");//使用GetAxis而不是GetAxisRaw避免滑行问题
            float direction = Input.GetAxisRaw("Horizontal");//使用GetAxisRaw而不是GetAxis避免人物显示出现问题

            if (direction != 0)
            {
                transform.localScale = new Vector3(direction, 1, 1);
            }

            if (horizontal != 0)
            {
                rigidbody2D.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rigidbody2D.velocity.y);
            }
        }
    }

    private void VerticalMove()//纵向移动
    {
        if (toJump)
        {
            toJump = false;

            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * Time.fixedDeltaTime);
            animator.SetBool("Falling", false);
            animator.SetBool("Land", false);
            animator.SetBool("Jumping", true);
            jumpAudio.Play();

            if (animator.GetBool("Crouch"))
            {
                animator.SetBool("Crouch", false);
                collider2DTop.enabled = true;
            }
        }

        if (!isHurt && !isDead)
        {
            //下落与下蹲
            if (Physics2D.OverlapCircle(bottomPoint.position, 0.2f, ground))//着地
            {
                if (animator.GetBool("Falling") || (animator.GetBool("Jumping") && rigidbody2D.velocity.y <= 0))
                {
                    if (doubleJump) jumpCount = 2;
                    else jumpCount = 1;
                }


                if (animator.GetBool("Falling") || (animator.GetBool("Jumping") && rigidbody2D.velocity.y <= 0))
                {
                    animator.SetBool("Land", true);
                    animator.SetBool("Jumping", false);
                    animator.SetBool("Falling", false);
                }

                //函数顺序不能调换！

                //下蹲及恢复判断
                if (!Physics2D.OverlapCircle(topPoint.position, 0.1f, ground))
                {
                    if (Input.GetButton("Crouch"))
                    {
                        animator.SetBool("Crouch", true);
                        collider2DTop.enabled = false;
                    }
                    else
                    {
                        animator.SetBool("Crouch", false);
                        collider2DTop.enabled = true;
                    }
                }

                //
            }
            else if (rigidbody2D.velocity.y < 0.1f)//下落判断
            {
                if (animator.GetBool("Falling") == false)
                {
                    if (doubleJump && jumpCount == 2) jumpCount = 1;
                    else if (!doubleJump && jumpCount == 1) jumpCount = 0;
                }

                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
                animator.SetBool("Land", false);

                if (animator.GetBool("Crouch"))
                {
                    animator.SetBool("Crouch", false);
                    collider2DTop.enabled = true;
                }
            }
        }
    }

    private void JumpCheck()
    {
        if (!isHurt)
        {
            if (Input.GetButtonDown("Jump") && !Physics2D.OverlapCircle(topPoint.position, 0.1f, ground))//头顶有障碍物时无法跳跃
            {
                if (jumpCount > 0)
                {
                    jumpCount -= 1;
                    toJump = true;
                }
            }
        }
    }

    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)//人物触碰到Trigger类型的碰撞体时调用
    {

        if (collision.tag == "Collections")
        {
            collision.tag = "Untagged";//避免重复计算
            Collections collection = collision.gameObject.GetComponent<Collections>();
            collection.Collected();
            cherryNumber.text = cherry.ToString();
            gemNumber.text = gem.ToString();
        }
        else if (collision.tag == "HiddenLine")
        {
            AchievementController.Instance.UnlockAchievement(Achievements.list.新世界);

            collision.tag = "Untagged";
            camera_1.GetComponent<CinemachineConfiner>().m_BoundingShape2D = hiddenBackground;
        }
        else if (collision.tag == "Fireball")
        {
            collision.gameObject.GetComponent<Fireball>().Boom();
            GetHurt(collision.gameObject);
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)//人物触碰到非Trigger类型的碰撞体时调用
    {
        if (!isHurt && collision.gameObject.tag == "Enemies" || collision.gameObject.tag == "Boss")
        {
            if (collision.gameObject.tag == "Enemies" && Physics2D.OverlapCircle(bottomPoint.position, 0.4f, enemies))//踩中敌人则消灭
            {
                if (isDashing) AchievementController.Instance.UnlockAchievement(Achievements.list.残影杀手);

                collision.gameObject.tag = "Untagged";

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.Boom();//播放爆炸动画
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce * Time.deltaTime);
                animator.SetBool("Falling", false);
                animator.SetBool("Land", false);
                animator.SetBool("Jumping", true);
            }
            else if (!isProtected)
            {
                GetHurt(collision.gameObject);
            }
        }
    }

    private void Restart()
    {
        //扣减生命后回到出发点
        healthValue.text = (--health).ToString() + "/" + SaveController.Instance.save.life.ToString();
        if (health > 0)
        {
            rigidbody2D.velocity = Vector2.zero;//避免初速度过大
            transform.position = restartPoint;//回到出发点
            GetComponent<AudioSource>().enabled = true;
            StartCoroutine("CheckDead");//重启协程
        }
        else
        {
            isDead = true;
            OverDialog.overDialogShow = true;
            //TODO:播放失败音效
        }
    }
    private void DashCheck()
    {
        if (!isHurt && !isDead && ableToDash && dashCoolDown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                cdImage.fillAmount = 1;
                isDashing = true;
                dashLeftTime = 0.15f;
                dashCoolDown = dashCoolDownSet;
                StartCoroutine(DashCoolDownCount());
            }
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            if (dashLeftTime > 0)
            {
                if (animator.GetBool("Jumping")) rigidbody2D.velocity = new Vector2(gameObject.transform.localScale.x * dashSpeed, jumpForce * Time.fixedDeltaTime);
                else rigidbody2D.velocity = new Vector2(gameObject.transform.localScale.x * dashSpeed, rigidbody2D.velocity.y);

                dashLeftTime -= Time.fixedDeltaTime;
                ShadowPool.Instance.GetFromPool();//生成残影
            }
            else
            {
                rigidbody2D.velocity = Vector2.zero;
                isDashing = false;
            }
        }
    }

    private void GetHurt(GameObject gameObject)
    {
        healthValue.text = (--health).ToString() + "/" + SaveController.Instance.save.life.ToString();
        if (health <= 0)
        {
            StopCoroutine("CheckDead");
            StartCoroutine(PlayDeathAnimation());//调用携程播放死亡动画

            //禁用部分按钮
            canvas.transform.Find("Setting").GetComponent<Button>().interactable = false;
            canvas.transform.Find("RestartButton").GetComponent<Button>().interactable = false;

            if (canvas.transform.Find("TipsButton") != null) canvas.transform.Find("TipsButton").GetComponent<Button>().interactable = false;
        }
        else StartCoroutine(HurtAnimation(gameObject));//受伤效果  

        StartCoroutine(Protected());//触发受伤保护
    }

    IEnumerator CheckDead()
    {
        while (transform.position.y > deadLine.position.y)
        {
            yield return 0;
        }

        GetComponent<AudioSource>().enabled = false;
        Invoke("Restart", 1f);

        yield break;
    }

    IEnumerator PlayDeathAnimation()
    {
        hurtAudio.Play();
        animator.SetBool("Hurt", true);
        isHurt = true;

        isDead = true;

        collider2DBottom.enabled = false;
        collider2DTop.enabled = false;
        camera_1.Follow = null;//停止相机跟随

        rigidbody2D.velocity = Vector3.zero;

        rigidbody2D.AddForce(new Vector3(0, 500, 0));

        while (transform.position.y > deadLine.position.y)
        {
            yield return 0;
        }

        OverDialog.overDialogShow = true;
        yield break;//停止该协程
    }

    IEnumerator HurtAnimation(GameObject gameObject)
    {
        if (gameObject.tag != "Fireball") hurtAudio.Play();
        animator.SetBool("Hurt", true);
        isHurt = true;

        float additionalForce = (gameObject.tag == "Boss" ? 10 : 0);

        if (transform.position.x < gameObject.transform.position.x) rigidbody2D.velocity = new Vector2(-(4 + additionalForce), rigidbody2D.velocity.y);
        else rigidbody2D.velocity = new Vector2(4 + additionalForce, rigidbody2D.velocity.y);

        float leftTime = 0.5f;
        while (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            yield return 0;
        }

        //0.5s后，受伤效果消失
        isHurt = false;
        animator.SetBool("Hurt", false);

        yield break;//结束协程
    }

    IEnumerator Protected()//受伤保护0.5s
    {
        isProtected = true;

        float leftTime = 0.5f;
        while (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            yield return 0;
        }

        isProtected = false;

        yield break;
    }

    IEnumerator DashCoolDownCount()//冲锋cd计时器
    {
        while (dashCoolDown > 0)
        {
            dashCoolDown -= Time.deltaTime;
            yield return 0;
        }
        yield break;
    }

    IEnumerator DashCoolDownUI()
    {
        while (true)
        {
            cdImage.fillAmount -= 1.0f / dashCoolDownSet * Time.deltaTime;
            yield return 0;
        }
    }
}
