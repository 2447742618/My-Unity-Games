using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour
{
    Animator animator;
    new Collider2D collider2D;
    new Rigidbody2D rigidbody2D;
    AudioSource dieAudio;
    SpriteRenderer spriteRenderer;

    Vector3 resumePoint;

    private float upPoint, downPoint;
    private bool upMoving = true;
    private float upSpeed = 0.2f;
    private float downSpeed = 0.04f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        dieAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        upPoint = transform.Find("TopPoint").gameObject.transform.position.y;
        downPoint = transform.Find("BottomPoint").gameObject.transform.position.y;
        resumePoint = transform.Find("ResumePoint").gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Die")) Move();
    }

    private void Move()
    {
        if (upMoving) transform.position = new Vector3(transform.position.x, transform.position.y + upSpeed, 0);
        else transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, 0);

        if (transform.position.y > upPoint) upMoving = false;
        if (transform.position.y < downPoint) upMoving = true;
    }

    private void Die()
    {
        transform.position = resumePoint;
        spriteRenderer.enabled = false;

        StartCoroutine(Resume());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss") Boom(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fireball")
        {
            collision.gameObject.GetComponent<Fireball>().Boom();
            Boom(collision.gameObject);
        }
    }

    private void Boom(GameObject gameObject)
    {
        if (gameObject.tag == "Boss") dieAudio.Play();
        rigidbody2D.velocity = Vector2.zero;
        collider2D.enabled = false;
        animator.SetBool("Die", true);
    }


    IEnumerator Resume()
    {
        float leftTime = 30f;
        while (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            yield return 0;
        }

        animator.SetBool("Die", false);
        spriteRenderer.enabled = true;
        collider2D.enabled = true;
        yield break;
    }
}
