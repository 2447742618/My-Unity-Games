using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected new Collider2D collider2D;
    protected new Rigidbody2D rigidbody2D;
    protected AudioSource dieAudio;


    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        dieAudio = GetComponent<AudioSource>();
    }

    private void Die()//在爆炸动画播放后调用，销毁物体
    {
        Destroy(gameObject);
    }

    public void Boom()
    {
        dieAudio.Play();
        rigidbody2D.velocity = Vector2.zero;
        collider2D.isTrigger = true;//避免再次产生碰撞
        Destroy(rigidbody2D);//避免再受重力影响
        animator.SetBool("Die", true);
    }
}
