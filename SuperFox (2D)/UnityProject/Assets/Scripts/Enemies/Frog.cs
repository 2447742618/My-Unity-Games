using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    LayerMask ground;
    float leftPoint, rightPoint;

    private bool leftMoving = true;
    private float speed = 3f, jumpForce = 5f;

    protected override void Awake()
    {
        base.Awake();

        ground = LayerMask.GetMask("Ground");
        leftPoint = transform.Find("LeftPoint").position.x;
        rightPoint = transform.Find("RightPoint").position.x;
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("Die")) SwitchAnimation();
    }

    private void Move()
    {
        if (collider2D.IsTouchingLayers(ground))
        {
            if (transform.position.x < leftPoint)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                leftMoving = false;
            }
            else if (transform.position.x > rightPoint)
            {
                transform.localScale = new Vector3(1, 1, 0);
                leftMoving = true;
            }
        }


        if (leftMoving) rigidbody2D.velocity = new Vector2(-speed, jumpForce);
        else rigidbody2D.velocity = new Vector2(speed, jumpForce);

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
}
