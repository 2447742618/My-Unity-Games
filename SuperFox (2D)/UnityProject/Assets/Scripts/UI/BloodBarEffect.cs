using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBarEffect : MonoBehaviour
{
    Image maskedImage, unmaskImage;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        maskedImage = transform.Find("Masked").GetComponent<Image>();
        unmaskImage = transform.Find("Unmask").GetComponent<Image>();
        StartCoroutine(MoveUp());
    }

    public void Init(float pre, float cur)
    {
        unmaskImage.fillAmount = cur;
        maskedImage.fillAmount = pre;
        animator.SetBool("Show", true);
    }

    IEnumerator MoveUp()
    {
        float speed = 300;
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            speed -= 10;
            transform.position += (Vector3.up * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
