using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fireball : MonoBehaviour
{
    private float speed = 10;
    private int startY = 10, targetY = -45;
    private int XMin = -20, XMAX = 50;

    ParticleSystem coreParticle, flameParticle;
    TrailRenderer trailRenderer;
    Animator animator;
    AudioSource boomAudio;
    new CircleCollider2D collider2D;

    LayerMask plant;

    private void Awake()
    {
        coreParticle = transform.Find("Core").GetComponent<ParticleSystem>();
        flameParticle = transform.Find("Flame").GetComponent<ParticleSystem>();
        trailRenderer = transform.Find("Trail").GetComponent<TrailRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        boomAudio = GetComponent<AudioSource>();
        plant = LayerMask.GetMask("Plant");
    }

    private void OnEnable()
    {
        System.Random rd = new System.Random();
        Vector2 targetPos = new Vector2(rd.Next(XMin, XMAX), targetY);
        Vector2 startPos = new Vector2(rd.Next(XMin, XMAX), startY);
        Vector2 direction = (targetPos - startPos).normalized;

        transform.position = startPos;
        float rotationZ = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ * Mathf.Rad2Deg);

        trailRenderer.enabled = true;
        collider2D.enabled = true;
        speed = 10;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (transform.position.y < targetY) ReturnPool();
    }

    public void Boom()
    {
        collider2D.enabled = false;

        trailRenderer.enabled = false;
        flameParticle.Stop();
        coreParticle.Stop();
        coreParticle.Clear();

        boomAudio.Play();

        speed = 5;

        Invoke(nameof(ReturnPool), 3f);
    }

    public void ReturnPool()
    {
        FireballPool.Instance.ReturnPool(gameObject);
    }
}
