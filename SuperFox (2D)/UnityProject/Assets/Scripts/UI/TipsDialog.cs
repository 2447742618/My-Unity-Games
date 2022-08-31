using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsDialog : MonoBehaviour
{
    GameObject tipsDialog;
    Button tipsButton;
    Animator animator;

    Button backButton, nextButton, previousButton;
    GameObject[] dialogs = new GameObject[3];

    EventSystem eventSystem;

    private void Awake()
    {
        tipsButton = GameObject.Find("LevelCanvas").transform.Find("TipsButton").GetComponent<Button>();
        tipsDialog = GameObject.Find("LevelCanvas").transform.Find("TipsDialog").gameObject;
        animator = tipsDialog.GetComponent<Animator>();

        backButton = tipsDialog.transform.Find("BackButton").GetComponent<Button>();
        nextButton = tipsDialog.transform.Find("NextPageButton").GetComponent<Button>();
        previousButton = tipsDialog.transform.Find("PreviousPageButton").GetComponent<Button>();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        for (int i = 0; i < 3; i++)
        {
            string name = "Dialog_" + i.ToString();
            dialogs[i] = tipsDialog.transform.Find(name).gameObject;
        }
    }

    private void Start()
    {
        tipsButton.onClick.AddListener(TipsButtonDown);
        backButton.onClick.AddListener(BackButtonDown);
        nextButton.onClick.AddListener(NextPageButtonDown);
        previousButton.onClick.AddListener(PreviousPageButtonDown);

        if (SaveController.Instance.save.firstEnterLevel3)
        {
            TipsButtonDown();
            SaveController.Instance.save.firstEnterLevel3 = false;
            SaveController.Instance.WriteBackToFile();
        }
    }

    public void BackButtonDown()
    {
        UIAudio.PlayClickAudio();

        //游戏时间恢复
        Time.timeScale = 1;
        animator.SetBool("Show", false);
    }

    public void NextPageButtonDown()
    {
        eventSystem.SetSelectedGameObject(null);
        UIAudio.PlayClickAudio();

        previousButton.gameObject.SetActive(true);

        if (dialogs[0].activeInHierarchy)//当前是第一页
        {
            dialogs[0].SetActive(false);
            dialogs[1].SetActive(true);
        }
        else if (dialogs[1].activeInHierarchy)
        {
            dialogs[1].SetActive(false);
            dialogs[2].SetActive(true);
        }

        if (dialogs[2].activeInHierarchy) nextButton.gameObject.SetActive(false);
        if (!SaveController.Instance.save.levelPass[3] && dialogs[1].activeInHierarchy) nextButton.gameObject.SetActive(false);
        //若未打败boss则不显示第三页
    }

    public void PreviousPageButtonDown()
    {
        eventSystem.SetSelectedGameObject(null);
        UIAudio.PlayClickAudio();

        nextButton.gameObject.SetActive(true);

        if (dialogs[1].activeInHierarchy)//当前是第一页
        {
            dialogs[1].SetActive(false);
            dialogs[0].SetActive(true);
        }
        else if (dialogs[2].activeInHierarchy)
        {
            dialogs[2].SetActive(false);
            dialogs[1].SetActive(true);
        }

        if (dialogs[0].activeInHierarchy) previousButton.gameObject.SetActive(false);
    }

    public void TipsButtonDown()
    {
        UIAudio.PlayClickAudio();

        dialogs[0].SetActive(true);
        dialogs[1].SetActive(false);
        dialogs[2].SetActive(false);

        animator.SetBool("Show", true);

        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
    }

    public void TimeFreeze()
    {
        //暂停时间，在窗口弹出后调用
        Time.timeScale = 0;
    }
}
