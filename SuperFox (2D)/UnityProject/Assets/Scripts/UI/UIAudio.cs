using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static AudioSource clickAudio;
    private void Awake()
    {
        clickAudio = GameObject.Find("LevelCanvas").GetComponent<AudioSource>();
    }

    static public void PlayClickAudio()
    {
        clickAudio.Play();
    }
}
