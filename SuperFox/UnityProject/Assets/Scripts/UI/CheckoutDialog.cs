using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CheckoutDialog : MonoBehaviour
{
    Button nextLevelButton, exitButton;
    static Text cherryNumber, gemNumber, rateNumber;
    static Animator animator;

    static double rateTarget, rateSource;

    new AudioSource audio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        nextLevelButton = transform.Find("NextLevelButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        cherryNumber = transform.Find("CherryNumber").GetComponent<Text>();
        gemNumber = transform.Find("GemNumber").GetComponent<Text>();
        rateNumber = transform.Find("Rate").GetComponent<Text>();
    }

    private void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevelButtonDown);
        exitButton.onClick.AddListener(ExitButtonDown);
    }

    static public void DialogShow(int index)
    {
        GameObject.Find("Player").GetComponent<Player>().enabled = false;//停止控制人物

        cherryNumber.text = Player.cherry.ToString();
        gemNumber.text = Player.gem.ToString();

        rateSource = SaveController.Instance.LevelRateCalculate(index);
        rateNumber.text = rateSource.ToString() + "%";

        animator.SetBool("DialogShow", true);

        //保存存档
        SaveController.Instance.UpdateCollectionInformation(Player.cherry, Player.gem);
        SaveController.Instance.UpdateLevelInformation(SceneManager.GetActiveScene().buildIndex, Player.collected);

        rateTarget = SaveController.Instance.LevelRateCalculate(index);

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SaveController.Instance.save.bossAlive = false;
            SaveController.Instance.save.skullPositionX = GameObject.Find("Environment").transform.Find("Skull").transform.position.x;
            SaveController.Instance.WriteBackToFile();
        }
    }

    public IEnumerator UpdateRate()
    {
        double rate = rateSource, gap = rateTarget - rateSource;
        while (rate < rateTarget)
        {
            rate += Mathf.Min(1f, (float)(rateTarget - rate));
            rate = Math.Round(rate, 1);
            rateNumber.text = rate.ToString() + "%";
            yield return new WaitForSeconds(1f / (float)gap);
        }

        animator.SetBool("ButtonShow", true);
        yield break;
    }

    public void ButtonShow()
    {
        animator.SetBool("ButtonShow", true);
    }

    public void NextLevelButtonDown()
    {
        UIAudio.PlayClickAudio();
        StartCoroutine(SceneController.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        //加载下一关卡
    }

    public void ExitButtonDown()
    {
        UIAudio.PlayClickAudio();
        SceneManager.LoadScene(0);//返回主菜单
    }

    public void CheckDisableButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)//最后一个关卡
        {
            nextLevelButton.interactable = false;
            Color cur = transform.Find("NextLevelButton").transform.Find("Text").GetComponent<Text>().color;
            transform.Find("NextLevelButton").transform.Find("Text").GetComponent<Text>().color = new Color(cur.r, cur.b, cur.g, 100 / 255f);
        }
    }

    public void PlayAudio()
    {
        audio.Play();
    }
}
