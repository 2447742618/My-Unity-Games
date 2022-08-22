using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
    private float upPoint, downPoint;
    private bool upMoving = true;
    private float speed = 0.15f;

    protected override void Awake()
    {
        base.Awake();

        upPoint = transform.Find("TopPoint").gameObject.transform.position.y;
        downPoint = transform.Find("BottomPoint").gameObject.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Die")) Move();
    }

    private void Move()
    {
        if (upMoving) transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0);
        else transform.position = new Vector3(transform.position.x, transform.position.y - speed, 0);

        if (transform.position.y > upPoint) upMoving = false;
        if (transform.position.y < downPoint) upMoving = true;
    }
}
