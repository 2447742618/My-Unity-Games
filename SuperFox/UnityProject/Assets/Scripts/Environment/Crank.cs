using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    public Sprite crankOn, crankOff;
    private LayerMask player;

    bool on;
    new BoxCollider2D collider2D;
    private new AudioSource audio;


    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = crankOff;
        player = LayerMask.GetMask("Player");
        collider2D = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();

        on = false;
    }

    private void Update()
    {
        if (on && collider2D.IsTouchingLayers(player))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchToOff();
            }
        }
    }

    public void SwitchToOn()
    {
        audio.Play();
        on = true;
        GetComponent<SpriteRenderer>().sprite = crankOn;
    }

    public void SwitchToOff()
    {
        StartCoroutine(Level3Controller.SummonFireball());
        StartCoroutine(Level3Controller.CrankOnCountDown());

        on = false;
        audio.Play();
        GetComponent<SpriteRenderer>().sprite = crankOff;
    }

}
