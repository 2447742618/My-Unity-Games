using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    Button settingButton, restartButton;
    Button continueButton, exitButton;
    Button bgmButton, audioButton;
    Slider bgmSlider, audioSlider;

    Animator animator;

    public Sprite audioOn, audioOff;
    GameObject bgmOff;

    public AudioMixer bgmMixer, audioMixer;

    GameObject restartDialog;

    private void Awake()
    {
        settingButton = GameObject.Find("LevelCanvas").gameObject.transform.Find("Setting").gameObject.GetComponent<Button>();
        restartButton = GameObject.Find("LevelCanvas").gameObject.transform.Find("RestartButton").gameObject.GetComponent<Button>();

        continueButton = transform.Find("ContinueButton").gameObject.GetComponent<Button>();
        exitButton = transform.Find("ExitButton").gameObject.GetComponent<Button>();
        bgmButton = transform.Find("BGMButton").gameObject.GetComponent<Button>();
        audioButton = transform.Find("AudioButton").gameObject.GetComponent<Button>();

        bgmSlider = transform.Find("BGMSlider").gameObject.GetComponent<Slider>();
        audioSlider = transform.Find("AudioSlider").gameObject.GetComponent<Slider>();

        bgmOff = transform.Find("BGMButton").gameObject.transform.Find("Off").gameObject;

        animator = GetComponent<Animator>();
        restartDialog = GameObject.Find("LevelCanvas").gameObject.transform.Find("RestartDialog").gameObject;
    }

    private void Start()
    {
        settingButton.onClick.AddListener(SettingButtonDown);
        continueButton.onClick.AddListener(ContinueButtonDown);
        exitButton.onClick.AddListener(ExitButtonDown);

        bgmButton.onClick.AddListener(BGMButtonDown);
        audioButton.onClick.AddListener(AudioButtonDown);

        audioSlider.onValueChanged.AddListener(SetAudioVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        bgmOff.SetActive(false);

        restartButton.onClick.AddListener(RestartButtonDown);
        restartDialog.transform.Find("YES").GetComponent<Button>().onClick.AddListener(YESButtonDown);
        restartDialog.transform.Find("NO").GetComponent<Button>().onClick.AddListener(NOButtonDown);

        Load();
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("BGMVolume"))
        {
            PlayerPrefs.SetFloat("BGMVolume", bgmSlider.maxValue);
        }
        else
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
            if (bgmSlider.value == bgmSlider.minValue)
            {
                bgmOff.SetActive(true);
                bgmButton.enabled = false;
            }
        }



        if (!PlayerPrefs.HasKey("AudioVolume"))
        {
            PlayerPrefs.SetFloat("AudioVolume", audioSlider.maxValue);
        }
        else
        {
            audioSlider.value = PlayerPrefs.GetFloat("AudioVolume");
            if (audioSlider.value == audioSlider.minValue)
            {
                audioButton.GetComponent<Button>().image.sprite = audioOff;
                audioButton.enabled = false;
            }
        }
    }

    private void SettingButtonDown()
    {
        UIAudio.PlayClickAudio();
        animator.SetBool("Show", true);
        //窗口展示完成后再暂停
    }

    private void ContinueButtonDown()
    {
        UIAudio.PlayClickAudio();
        Time.timeScale = 1;//必须先将时间恢复
        animator.SetBool("Show", false);
    }

    private void ExitButtonDown()
    {
        UIAudio.PlayClickAudio();
        Time.timeScale = 1;//将时间恢复
        SceneManager.LoadScene(0);
    }

    public void TimeFreeze()
    {
        Time.timeScale = 0f;
    }

    private void SetBGMVolume(float value)
    {
        if (bgmOff.activeInHierarchy)
        {
            bgmOff.SetActive(false);
            bgmButton.enabled = true;
        }
        bgmMixer.SetFloat("Volume", value);

        if (bgmSlider.value == bgmSlider.minValue)
        {
            bgmOff.SetActive(true);
            bgmButton.enabled = false;
        }

        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
    }

    private void SetAudioVolume(float value)
    {
        if (audioButton.GetComponent<Button>().image.sprite == audioOff)
        {
            audioButton.GetComponent<Button>().image.sprite = audioOn;
            audioButton.enabled = true;
        }
        audioMixer.SetFloat("Volume", value);

        if (audioSlider.value == audioSlider.minValue)
        {
            audioButton.GetComponent<Button>().image.sprite = audioOff;
            audioButton.enabled = false;
        }

        PlayerPrefs.SetFloat("AudioVolume", audioSlider.value);
    }

    private void BGMButtonDown()
    {
        UIAudio.PlayClickAudio();
        bgmSlider.value = bgmSlider.minValue;
        bgmOff.SetActive(true);
        bgmButton.enabled = false;
    }

    private void AudioButtonDown()
    {
        UIAudio.PlayClickAudio();
        audioSlider.value = audioSlider.minValue;
        audioButton.GetComponent<Button>().image.sprite = audioOff;
        audioButton.enabled = false;
    }

    private void RestartButtonDown()
    {
        UIAudio.PlayClickAudio();
        GameObject.Find("Player").GetComponent<Player>().enabled = false;//不可再控制人物
        restartDialog.GetComponent<Animator>().SetBool("Show", true);
    }

    private void YESButtonDown()
    {
        UIAudio.PlayClickAudio();
        GameObject.Find("Player").GetComponent<Player>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void NOButtonDown()
    {
        UIAudio.PlayClickAudio();
        GameObject.Find("Player").GetComponent<Player>().enabled = true;
        restartDialog.GetComponent<Animator>().SetBool("Show", false);
    }

}
