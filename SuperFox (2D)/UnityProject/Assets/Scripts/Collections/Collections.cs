using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collections : MonoBehaviour
{
    protected new Collider2D collider2D;
    protected Animator animator;
    protected AudioSource collectedAudio;

    protected void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        collectedAudio = GetComponent<AudioSource>();
    }

    private void Disappear()//动画播放结束后调用
    {
        Destroy(gameObject);
    }

    public virtual void Collected()
    {
        Player.collected.Add(gameObject.name);
        collectedAudio.Play();
        animator.SetBool("Collected", true);
    }
}
