using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    Transform player;
    SpriteRenderer thisSprite, playerSprite;

    Color color;

    private float alphaMultiplier = 0.8f;
    private float alpha, leftTime;

    private void OnEnable()
    {
        player = GameObject.Find("Player").gameObject.transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.gameObject.GetComponent<SpriteRenderer>();

        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        alpha = leftTime = 1f;
    }


    private void FixedUpdate()
    {
        alpha *= alphaMultiplier;
        color = new Color(0.5f, 0.5f, 1, alpha);
        thisSprite.color = color;

        if (leftTime > 0)
        {
            leftTime -= Time.fixedDeltaTime;
            return;
        }

        ShadowPool.Instance.ReturnPool(gameObject);
    }
}
