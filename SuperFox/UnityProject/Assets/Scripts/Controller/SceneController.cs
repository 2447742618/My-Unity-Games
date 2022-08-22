using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    Text rate, text;
    GameObject handle, hint;
    Slider slider;
    EventSystem eventSystem;

    public Sprite[] sprites = new Sprite[6];

    private int textNumber = 5;
    string[] _text = new string[10];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rate = transform.Find("Rate").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();
        handle = transform.Find("Slider").transform.Find("Handle Slide Area").transform.Find("Handle").gameObject;
        hint = transform.Find("Hint").gameObject;
        slider = transform.Find("Slider").GetComponent<Slider>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Start()
    {
        _text[0] = "解锁二段跳是打败BOSS的必要条件，但冲刺不是";
        _text[1] = "事实上，解锁二段跳是解锁冲刺的前提";
        _text[2] = "收集品可以用于在商店解锁技能或购买生命值上限";
        _text[3] = "有些收集品并不容易被发现";
        _text[4] = "关卡结算前，游戏进度不会被保存";
    }

    public IEnumerator LoadScene(int sceneIndex)//要加载的场景编号
    {
        eventSystem.SetSelectedGameObject(null);//将选取的物体置空，避免按钮被重复按下

        //停止BGM
        if (SceneManager.GetActiveScene().buildIndex == 0) GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
        else GameObject.Find("Player").GetComponents<AudioSource>()[0].Stop();

        System.Random rd = new System.Random();
        text.text = "Tips:" + _text[rd.Next() % textNumber];

        GetComponent<Animator>().SetBool("Show", true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float percent = 0;
        int spriteIndex = 0;
        while (percent < 100)
        {
            percent += 1;
            slider.value = percent;
            rate.text = percent.ToString() + "%";

            if (percent % 2 == 0)
            {
                handle.GetComponent<Image>().sprite = sprites[spriteIndex];
                spriteIndex = (spriteIndex + 1) % 6;
            }

            yield return new WaitForSeconds(0.03f);
        }

        hint.SetActive(true);

        while (true)
        {
            if (Input.anyKeyDown)
            {
                operation.allowSceneActivation = true;
            }
            yield return 0;
        }

    }

}
