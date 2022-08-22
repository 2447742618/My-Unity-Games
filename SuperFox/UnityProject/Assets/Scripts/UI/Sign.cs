using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    new Collider2D collider2D;
    GameObject message;
    Animator animator;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        message = transform.Find("Message").gameObject;
        animator = message.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("Show", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("Show", false);
        }
    }
}
