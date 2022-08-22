using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.UI;

public class Level3Controller : MonoBehaviour
{
    public static Level3Controller Instance;

    static public Crank[] crank = new Crank[4];

    public AudioClip bossAliveBGM, bossDeadBGM;

    public GameObject skullPrefab;
    public GameObject treePrefab, bushPrefab;
    GameObject environment, boss, bloodBarBackground;

    GameObject rebornDialog;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < 4; i++)
        {
            string name = "Crank_" + i.ToString();
            crank[i] = GameObject.Find("Cranks").transform.Find(name).gameObject.GetComponent<Crank>();
        }

        environment = GameObject.Find("Environment").gameObject;
        boss = GameObject.Find("Boss").gameObject;
        bloodBarBackground = GameObject.Find("LevelCanvas").transform.Find("BloodBarBackground").gameObject;
        rebornDialog = GameObject.Find("LevelCanvas").transform.Find("RebornDialog").gameObject;
    }

    private void Start()
    {
        rebornDialog.transform.Find("NO").GetComponent<Button>().onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); rebornDialog.GetComponent<Animator>().SetBool("Show", false); GameObject.Find("Player").GetComponent<Player>().enabled = true; });
        rebornDialog.transform.Find("YES").GetComponent<Button>().onClick.AddListener(delegate () { UIAudio.PlayClickAudio(); rebornDialog.GetComponent<Animator>().SetBool("Show", false); BossReborn(); GameObject.Find("Player").GetComponent<Player>().enabled = true; });

        if (!SaveController.Instance.save.bossAlive)
        {
            GameObject.Find("Boss").SetActive(false);
            GameObject.Find("Player").GetComponents<AudioSource>()[0].clip = bossDeadBGM;
            GameObject.Find("Player").GetComponents<AudioSource>()[0].Play();

            GameObject.Find("Environment").transform.Find("Background").GetComponent<Animator>().enabled = false;
            GameObject.Find("Environment").transform.Find("Background").GetComponent<SpriteRenderer>().color = Color.white;


            //关闭血条UI
            bloodBarBackground.GetComponent<Animator>().enabled = false;
            Color color = bloodBarBackground.GetComponent<Image>().color;
            color.a = 0;
            bloodBarBackground.GetComponent<Image>().color = color;
            color = bloodBarBackground.transform.Find("BloodBar").GetComponent<Image>().color;
            color.a = 0;
            bloodBarBackground.transform.Find("BloodBar").GetComponent<Image>().color = color;

            //生成骸骨
            GameObject skull = Instantiate(skullPrefab, environment.transform);
            skull.name = "Skull";
            skull.transform.position = new Vector3(SaveController.Instance.save.skullPositionX, skull.transform.position.y, 0);
        }
        else
        {
            Destroy(environment.transform.Find("Plants").gameObject);
            RandomCrankOn();
        }
    }

    static public void RandomCrankOn()
    {
        System.Random rd = new System.Random();
        int i = rd.Next() % 4;
        crank[i].SwitchToOn();
    }

    static public IEnumerator SummonFireball()//用于被Crank调用，生成随机火球雨
    {
        FireballPool.Instance.burningAudio.volume = 1;
        FireballPool.Instance.burningAudio.Play();
        for (int i = 1; i <= FireballPool.Instance.fireballCount; i++)
        {
            GameObject fireball = FireballPool.Instance.GetFromPool();
            yield return new WaitForSeconds(1f);
        }

        yield break;
    }

    static public IEnumerator CrankOnCountDown()
    {
        float LeftTime = 13f;
        while (LeftTime > 0)
        {
            LeftTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (Boss.HP > 0) RandomCrankOn();
        yield break;
    }

    public void BossDie()
    {
        GameObject.Find("Environment").transform.Find("Background").GetComponent<Animator>().enabled = true;
        GameObject.Find("Environment").transform.Find("Background").GetComponent<Animator>().SetTrigger("Brighten");
        StartCoroutine(BGMTransform(bossDeadBGM));
    }

    public void BossReborn()
    {
        Destroy(GameObject.Find("Skull").gameObject);
        boss.transform.position = GameObject.Find("RebornPoint").transform.position;

        Boss.specificJumpCount = 2;
        boss.SetActive(true);

        RandomCrankOn();

        bloodBarBackground.GetComponent<Animator>().enabled = true;
        bloodBarBackground.GetComponent<Animator>().SetTrigger("Show");

        GameObject.Find("Environment").transform.Find("Background").GetComponent<Animator>().enabled = true;
        GameObject.Find("Environment").transform.Find("Background").GetComponent<Animator>().SetTrigger("Darken");
        StartCoroutine(BGMTransform(bossAliveBGM));

        //保存进度
        SaveController.Instance.save.bossAlive = true;
        SaveController.Instance.WriteBackToFile();
    }

    IEnumerator BGMTransform(AudioClip targetClip)
    {
        AudioSource bgm = GameObject.Find("Player").GetComponents<AudioSource>()[0];

        float percent = 1;

        while (percent > 0)
        {
            percent -= 0.01f;
            bgm.volume = percent;
            yield return new WaitForFixedUpdate();
        }

        bgm.clip = targetClip;
        bgm.Play();

        while (percent < 1)
        {
            percent += 0.01f;
            bgm.volume = percent;
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }
}
