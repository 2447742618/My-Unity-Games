using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    Animator animator;
    LayerMask fireball;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fireball = LayerMask.GetMask("Fireball");
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(this.transform.position, 1f, fireball)) animator.SetBool("Disappear", true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
