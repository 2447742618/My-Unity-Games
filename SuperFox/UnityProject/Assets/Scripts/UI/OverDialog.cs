using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverDialog : MonoBehaviour
{
    GameObject overDialog;
    Button ExitButton, PlayAgainButton;

    public static bool overDialogShow;

    private void Awake()
    {
        overDialog = GameObject.Find("LevelCanvas").gameObject.transform.Find("OverDialog").gameObject;

        ExitButton = overDialog.transform.Find("ExitButton").gameObject.GetComponent<Button>();
        PlayAgainButton = overDialog.transform.Find("PlayAgainButton").gameObject.GetComponent<Button>();

        ExitButton.onClick.AddListener(ExitButtonDown);
        PlayAgainButton.onClick.AddListener(PlayAgainButtonDown);//重新进入当前关卡

        //Destroy(ExitButton);

    }

    private void Start()
    {
        overDialogShow = false;

        overDialog.SetActive(false);    
    }

    private void Update()
    {
        if (overDialogShow)
        {
            overDialog.SetActive(true);
        }
    }

    private void PlayAgainButtonDown()
    {
        UIAudio.PlayClickAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitButtonDown()
    {
        UIAudio.PlayClickAudio();
        SceneManager.LoadScene(0);
    }


}
