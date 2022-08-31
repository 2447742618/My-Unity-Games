using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private float upPoint, downPoint;
    private bool upMoving;
    private float upSpeed = 0.1f;
    private float downSpeed = 0.025f;

    private LayerMask player;
    private bool on = false;


    private new AudioSource audio;
    public GameObject crank;
    private new Collider2D collider2D;//与一个拉杆的碰撞器配对

    public Sprite crankOn, crankOff;

    private void Awake()
    {
        upPoint = transform.Find("TopPoint").gameObject.transform.position.y;
        downPoint = transform.Find("BottomPoint").gameObject.transform.position.y;
        player = LayerMask.GetMask("Player");
        audio = GetComponent<AudioSource>();

        if (crank != null)
        {
            collider2D = crank.GetComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        upMoving = true;

        if (collider2D == null) on = true;//如果没有拉杆与之配对，则其一直处于打开状态    
        else on = false;
    }

    private void FixedUpdate()
    {
        if (on) Move();
    }

    private void Update()
    {
        Switch();
    }

    private void Move()
    {
        if (upMoving) transform.position = new Vector3(transform.position.x, transform.position.y + upSpeed, 0);
        else transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, 0);

        if (transform.position.y > upPoint) upMoving = false;
        if (transform.position.y < downPoint) upMoving = true;
    }

    private void Switch()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (collider2D != null && collider2D.IsTouchingLayers(player))
            {
                audio.Play();
                on = !on;

                if (on) crank.GetComponent<SpriteRenderer>().sprite = crankOn;
                else crank.GetComponent<SpriteRenderer>().sprite = crankOff;
            }
        }
    }
}
