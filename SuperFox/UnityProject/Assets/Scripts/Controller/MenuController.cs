using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MenuController : MonoBehaviour
{
    GameObject canvas;
    Button playButton, exitButton, backButton;
    Button shopButton, achievementButton;
    Button shopWindowCloseButton, achievementWindowCloseButton;//
    GameObject[] levelButtons = new GameObject[Save.level + 1];
    CinemachineVirtualCamera playVC, menuVC;
    Text cherryNumber, gemNumber;
    GameObject shopWindow, successfulPurchase, failedPurchase, achievementWindow;
    ScrollRect shopScrollRect;
    AudioSource successfulPurchaseAudio;

    private bool shopWindowInit, achievementWindowInit;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").gameObject;

        playButton = canvas.transform.Find("Menu").gameObject.transform.Find("PlayButton").gameObject.GetComponent<Button>();
        exitButton = canvas.transform.Find("Menu").gameObject.transform.Find("ExitButton").gameObject.GetComponent<Button>();
        backButton = canvas.transform.Find("Play").gameObject.transform.Find("BackButton").gameObject.GetComponent<Button>();
        playVC = GameObject.Find("Virtual Camera").gameObject.transform.Find("Play VC").gameObject.GetComponent<CinemachineVirtualCamera>();
        menuVC = GameObject.Find("Virtual Camera").gameObject.transform.Find("Menu VC").gameObject.GetComponent<CinemachineVirtualCamera>();
        cherryNumber = canvas.transform.Find("Play").gameObject.transform.Find("Cherry").gameObject.transform.Find("Value").gameObject.GetComponent<Text>();
        gemNumber = canvas.transform.Find("Play").gameObject.transform.Find("Gem").gameObject.transform.Find("Value").gameObject.GetComponent<Text>();

        shopButton = canvas.transform.Find("Play").gameObject.transform.Find("ShopButton").gameObject.GetComponent<Button>();
        achievementButton = canvas.transform.Find("Play").gameObject.transform.Find("AchievementButton").gameObject.GetComponent<Button>();

        shopWindow = canvas.transform.Find("Play").gameObject.transform.Find("ShopWindow").gameObject;
        achievementWindow = canvas.transform.Find("Play").gameObject.transform.Find("AchievementWindow").gameObject;
        successfulPurchase = canvas.transform.Find("Play").gameObject.transform.Find("SuccessfulPurchase").gameObject;
        failedPurchase = canvas.transform.Find("Play").gameObject.transform.Find("FailedPurchase").gameObject;

        shopWindowCloseButton = shopWindow.transform.Find("CloseButton").GetComponent<Button>();
        achievementWindowCloseButton = achievementWindow.transform.Find("CloseButton").GetComponent<Button>();

        shopScrollRect = shopWindow.transform.Find("ScrollRect").GetComponent<ScrollRect>();

        successfulPurchaseAudio = shopWindow.GetComponent<AudioSource>();

        for (int i = 1; i <= Save.level; i++)
        {
            levelButtons[i] = canvas.transform.Find("Play").gameObject.transform.Find("Level").gameObject.transform.Find("Level_" + i.ToString()).gameObject;
        }
    }

    private void Start()
    {
        Load();

        menuVC.gameObject.SetActive(true);
        playVC.gameObject.SetActive(false);
        playButton.onClick.AddListener(PlayButtonDown);
        exitButton.onClick.AddListener(ExitButtonDown);
        backButton.onClick.AddListener(BackButtonDown);

        shopButton.onClick.AddListener(ShopButtonDown);
        achievementButton.onClick.AddListener(AchievementButtonDown);

        shopWindowCloseButton.onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); CloseButtonDown(shopWindow); });
        achievementWindowCloseButton.onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); CloseButtonDown(achievementWindow); });

        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find("DoubleJump").GetComponent<Button>().onClick.AddListener(DoubleJumpBought);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.transform.Find("Dash").GetComponent<Button>().onClick.AddListener(DashBought);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.transform.Find("Life").GetComponent<Button>().onClick.AddListener(LifeBought);

        successfulPurchase.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); successfulPurchase.GetComponent<Animator>().SetBool("Show", false); shopScrollRect.enabled = true; shopWindowCloseButton.enabled = true; });
        failedPurchase.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); failedPurchase.GetComponent<Animator>().SetBool("Show", false); shopScrollRect.enabled = true; shopWindowCloseButton.enabled = true; });

        for (int i = 1; i <= 3; i++)//注意细节
        {
            int index = i;//必要操作
            levelButtons[index].GetComponent<Button>().onClick.AddListener(delegate () { StartCoroutine(SceneController.Instance.LoadScene(index)); });
        }


        shopWindowInit = achievementWindowInit = false;
    }


    private void Load()
    {
        //收集品信息
        cherryNumber.text = SaveController.Instance.save.cherry.ToString();
        gemNumber.text = SaveController.Instance.save.gem.ToString();

        //关卡信息
        for (int i = 1; i <= Save.level; i++)
        {
            if (SaveController.Instance.save.levelUnlock[i] == false)//关卡未解锁
            {
                levelButtons[i].GetComponent<Button>().interactable = false;

                Color titleColor = levelButtons[i].transform.Find("Title").gameObject.GetComponent<Text>().color;
                titleColor.a = 150f / 255;
                levelButtons[i].transform.Find("Title").gameObject.GetComponent<Text>().color = titleColor;

                levelButtons[i].transform.Find("Rate").gameObject.GetComponent<Text>().enabled = false;
            }

            if (SaveController.Instance.save.levelPass[i])
            {
                double rate = SaveController.Instance.LevelRateCalculate(i);
                levelButtons[i].transform.Find("Rate").gameObject.GetComponent<Text>().text = rate.ToString() + "%";
            }
        }

        //技能、成就信息
    }

    private void PlayButtonDown()
    {
        UIAudio.PlayClickAudio();
        menuVC.gameObject.SetActive(false);
        playVC.gameObject.SetActive(true);
    }

    private void BackButtonDown()
    {
        UIAudio.PlayClickAudio();
        playVC.gameObject.SetActive(false);
        menuVC.gameObject.SetActive(true);
    }

    private void ShopDefault(string name)
    {
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.transform.Find(name).transform.Find("Cost").gameObject.SetActive(true);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).transform.Find("Number").gameObject.SetActive(true);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).transform.Find("Owned").gameObject.SetActive(false);

    }

    private void ShopOwned(string name)
    {
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).transform.Find("Cost").gameObject.SetActive(false);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).transform.Find("Number").gameObject.SetActive(false);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).transform.Find("Owned").gameObject.SetActive(true);
        shopWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).transform.Find(name).GetComponent<Button>().interactable = false;
    }

    private void ShopButtonDown()
    {
        UIAudio.PlayClickAudio();
        shopWindow.GetComponent<Animator>().SetBool("Show", true);
        DisabledButtons();//禁用一些外部按钮

        if (!shopWindowInit)
        {
            if (SaveController.Instance.save.doubleJump) ShopOwned("DoubleJump");
            else ShopDefault("DoubleJump");

            if (SaveController.Instance.save.dash) ShopOwned("Dash");
            else ShopDefault("Dash");

            shopWindowInit = true;
        }
    }

    private void DoubleJumpBought()//购买二段跳
    {
        UIAudio.PlayClickAudio();
        if (SaveController.Instance.save.gem >= 5)
        {
            successfulPurchaseAudio.Play();
            successfulPurchase.GetComponent<Animator>().SetBool("Show", true);

            SaveController.Instance.save.doubleJump = true;
            SaveController.Instance.save.gem -= 5;
            ShopOwned("DoubleJump");
            gemNumber.text = SaveController.Instance.save.gem.ToString();
            SaveController.Instance.WriteBackToFile();
        }
        else failedPurchase.GetComponent<Animator>().SetBool("Show", true);

        shopScrollRect.enabled = false;
        shopWindowCloseButton.enabled = false;
    }

    private void DashBought()//购买冲刺
    {
        UIAudio.PlayClickAudio();
        if (SaveController.Instance.save.gem >= 9)
        {
            successfulPurchaseAudio.Play();
            successfulPurchase.GetComponent<Animator>().SetBool("Show", true);

            SaveController.Instance.save.dash = true;
            SaveController.Instance.save.gem -= 9;
            ShopOwned("Dash");
            gemNumber.text = SaveController.Instance.save.gem.ToString();
            SaveController.Instance.WriteBackToFile();
        }
        else failedPurchase.GetComponent<Animator>().SetBool("Show", true);

        shopScrollRect.enabled = false;
        shopWindowCloseButton.enabled = false;
    }

    private void LifeBought()
    {
        UIAudio.PlayClickAudio();
        if (SaveController.Instance.save.cherry >= 2)
        {
            successfulPurchaseAudio.Play();
            successfulPurchase.GetComponent<Animator>().SetBool("Show", true);

            SaveController.Instance.save.life++;
            SaveController.Instance.save.cherry -= 2;
            cherryNumber.text = SaveController.Instance.save.cherry.ToString();
            SaveController.Instance.WriteBackToFile();
        }
        else failedPurchase.GetComponent<Animator>().SetBool("Show", true);

        shopScrollRect.enabled = false;
        shopWindowCloseButton.enabled = false;
    }

    private void AchievementButtonDown()
    {
        UIAudio.PlayClickAudio();
        achievementWindow.GetComponent<Animator>().SetBool("Show", true);
        DisabledButtons();//禁用一些外部按钮

        if (!achievementWindowInit)
        {
            GameObject content = achievementWindow.transform.Find("ScrollRect").transform.GetChild(0).transform.GetChild(0).gameObject;

            int index = 0;
            for (int i = 0; i < System.Enum.GetNames(typeof(Achievements.list)).Length; i++)
            {
                if (SaveController.Instance.save.achievements.unlock[i])
                {
                    GameObject item = content.transform.GetChild(index).gameObject;
                    item.transform.Find("Description").GetComponent<Text>().text = Achievements.name[i] + "：" + Achievements.description[i];
                    item.transform.Find("Description").gameObject.SetActive(true);
                    item.transform.Find("Lock").gameObject.SetActive(false);
                    index++;
                }
            }

            int total = System.Enum.GetNames(typeof(Achievements.list)).Length;
            achievementWindow.transform.Find("Rate").GetComponent<Text>().text = index.ToString() + "/" + total.ToString();

            achievementWindowInit = true;
        }
    }

    private void CloseButtonDown(GameObject _gameObject)
    {
        UIAudio.PlayClickAudio();
        EnableButtons();
        _gameObject.GetComponent<Animator>().SetBool("Show", false);
    }

    private void ExitButtonDown()
    {
        UIAudio.PlayClickAudio();
#if UNITY_EDITOR
        Application.Quit();
#else
        Application.Quit();
#endif
    }

    private void DisabledButtons()
    {
        backButton.enabled = false;
        shopButton.enabled = false;
        achievementButton.enabled = false;
    }

    private void EnableButtons()
    {
        backButton.enabled = true;
        shopButton.enabled = true;
        achievementButton.enabled = true;
    }
}
