using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    public float speed;

    private float inputX, inputY;
    private Vector2 movementInput;

    private Animator[] animators;

    private bool isMoving;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        PlayerInput();
        SwitchAnimation();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 && inputY != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }

        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;
    }

    private void Movement()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movementInput * speed * Time.fixedDeltaTime);
    }

    private void SwitchAnimation()
    {
        if (inputX < 0) transform.localScale = new Vector2(-1, 1);
        if (inputX > 0) transform.localScale = new Vector2(1, 1);

        foreach (var anim in animators)
        {
            anim.SetBool("isMoving", isMoving);

            if (isMoving)
            {
                anim.SetFloat("inputX", inputX);
                anim.SetFloat("inputY", inputY);
            }
        }
    }
}
