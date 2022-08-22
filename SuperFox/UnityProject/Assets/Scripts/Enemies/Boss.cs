using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class Boss : MonoBehaviour
{
    new Collider2D collider2D;
    Animator animator;
    new Rigidbody2D rigidbody2D;

    LayerMask ground;
    Transform target, deadLine;
    Vector3 restartPoint;

    AudioSource smallBoomAudio, bigBoomAudio;

    public GameObject skullPrefab;

    static public int HP;
    static public int specificJumpCount = 0;

    private bool leftMoving = true;
    private float speed = 12f, jumpForce = 8f;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        smallBoomAudio = GetComponents<AudioSource>()[0];
        bigBoomAudio = GetComponents<AudioSource>()[1];

        ground = LayerMask.GetMask("Ground");
        target = GameObject.Find("Player").transform;
        deadLine = GameObject.Find("DeadLine").transform;

        restartPoint = GameObject.Find("BossRestartPoint").transform.position;

        HP = 100;
        specificJumpCount = 0;
    }

    private void Start()
    {
        StartCoroutine("CheckFall");
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Boom")) SwitchAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fireball")
        {
            collision.gameObject.GetComponent<Fireball>().Boom();

            GetHurt(5);
        }
    }

    private void Move()
    {
        if (specificJumpCount > 0)
        {
            leftMoving = false;
            transform.localScale = new Vector3(-5, 5, 1);

            rigidbody2D.velocity = new Vector2(speed * 0.65f, jumpForce * 0.65f);
            animator.SetBool("Jumping", true);

            specificJumpCount--;
            return;
        }

        if (collider2D.IsTouchingLayers(ground))
        {
            if (transform.position.x < target.transform.position.x) leftMoving = false;
            else leftMoving = true;
        }

        if (leftMoving) transform.localScale = new Vector3(5, 5, 1);
        else transform.localScale = new Vector3(-5, 5, 1);

        System.Random rd = new System.Random();
        float multiplier = (float)rd.NextDouble();

        multiplier = Mathf.Max(0.15f, multiplier);

        if (leftMoving) rigidbody2D.velocity = new Vector2(-speed * multiplier, jumpForce * multiplier);
        else rigidbody2D.velocity = new Vector2(speed * multiplier, jumpForce * multiplier);

        animator.SetBool("Jumping", true);
    }

    private void SwitchAnimation()
    {
        if (animator.GetBool("Jumping"))
        {
            if (rigidbody2D.velocity.y < 0.1)
            {
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
            }
        }

        if (animator.GetBool("Falling"))
        {
            if (collider2D.IsTouchingLayers(ground))
            {
                animator.SetBool("Falling", false);
            }
        }
    }

    public void GetHurt(int damage)
    {
        BloodBar.totalDamage += damage;
        HP -= damage;

        if (HP <= 0) ToDie();
    }

    public void ToDie()
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        collider2D.enabled = false;
        animator.SetBool("Boom", true);

        AchievementController.Instance.UnlockAchievement(Achievements.list.屠龙者);

        GenerateSkull();
    }

    public void Die()
    {
        Level3Controller.Instance.BossDie();

        gameObject.SetActive(false);
    }

    public void GenerateSkull()
    {
        //生成骸骨
        GameObject skull = Instantiate(skullPrefab, GameObject.Find("Environment").transform);
        skull.name = "Skull";
        skull.transform.position = new Vector2(this.transform.position.x, skull.transform.position.y);
        skull.GetComponent<Skull>().enabled = false;//无法立即复活BOSS
    }

    public void PlaySmallBoomAudio()
    {
        smallBoomAudio.Play();
    }

    public void PlayBigBoomAudio()
    {
        bigBoomAudio.Play();
    }

    IEnumerator CheckFall()
    {
        while (true)
        {
            if (transform.position.y < deadLine.position.y)
            {
                transform.position = restartPoint;
                GetHurt(Mathf.Min(10, HP - 1));

                AchievementController.Instance.UnlockAchievement(Achievements.list.另辟蹊径);
            }
            yield return 0;
        }
    }

}
