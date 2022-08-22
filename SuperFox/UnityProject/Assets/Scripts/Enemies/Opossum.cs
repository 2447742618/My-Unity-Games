using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Enemy
{
    float leftPoint, rightPoint;

    public bool leftMoving;
    private float speed = 0.15f;

    protected override void Awake()
    {
        base.Awake();

        leftPoint = transform.Find("LeftPoint").position.x;
        rightPoint = transform.Find("RightPoint").position.x;
    }

    private void Start()
    {
        if (leftMoving == false) transform.localScale = new Vector3(-1, 1, 0);
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Die")) Move();
    }
    private void Move()
    {
        if (leftMoving) transform.position = new Vector3(transform.position.x - speed, transform.position.y, 0);
        else transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0);

        if (transform.position.x < leftPoint)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            leftMoving = false;
        }

        if (transform.position.x > rightPoint)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            leftMoving = true;
        }
    }
}
