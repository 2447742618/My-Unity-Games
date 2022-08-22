using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BloodBar : MonoBehaviour
{
    Image image;

    public GameObject prefab;

    int maxHP = 100;
    int curHP = 100;
    static public int totalDamage = 0;

    bool doing = false;

    private void Awake()
    {
        image = GetComponent<Image>();

        totalDamage = 0;
        curHP = maxHP = 100;
        doing = false;
    }

    private void Update()
    {
        if (!doing && totalDamage > 0) StartCoroutine(BloodDeduction());
    }

    IEnumerator BloodDeduction()
    {
        doing = true;
        while (totalDamage > 0)
        {
            if (curHP <= 0) break;
            totalDamage--;

            float preFillAmount = (float)curHP / (float)maxHP;
            float curFillAmount = (float)(--curHP) / (float)maxHP;
            image.fillAmount = curFillAmount;

            GameObject obj = Instantiate(prefab, transform);
            obj.GetComponent<BloodBarEffect>().Init(preFillAmount, curFillAmount);
            obj.transform.SetAsFirstSibling();

            yield return new WaitForSeconds(0.2f);
        }

        if (curHP <= 0)
        {
            transform.parent.GetComponent<Animator>().enabled = true;
            transform.parent.GetComponent<Animator>().SetTrigger("Disappear");
        }

        doing = false;
        yield break;
    }
}
