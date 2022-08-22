using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    new BoxCollider2D collider2D;

    LayerMask player;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        player = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        if (collider2D.IsTouchingLayers(player))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameObject.Find("LevelCanvas").transform.Find("RebornDialog").GetComponent<Animator>().SetBool("Show", true);
                GameObject.Find("LevelCanvas").transform.Find("EndDialog").gameObject.SetActive(false);

                GameObject.Find("Player").GetComponent<Player>().enabled = false;
            }
        }
    }
}
