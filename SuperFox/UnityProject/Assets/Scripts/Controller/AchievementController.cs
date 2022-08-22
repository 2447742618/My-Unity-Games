using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    public static AchievementController Instance;

    Text text;
    AudioSource unlockAchievementAudio;

    Animator animator;

    private bool doing;
    public Queue<int> toUnlock = new Queue<int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        text = GameObject.Find("LevelCanvas").transform.Find("UnlockDialog").transform.Find("Text").GetComponent<Text>();

        unlockAchievementAudio = GetComponent<AudioSource>();
        animator = GameObject.Find("LevelCanvas").transform.Find("UnlockDialog").GetComponent<Animator>();
    }

    private void Update()
    {
        if (toUnlock.Count != 0 && !doing)
        {
            StartCoroutine(IEUnlockAchievement());
        }
    }

    public void UnlockAchievement(Achievements.list _list)
    {
        if (!Player.isDead)
        {
            int index = (int)System.Enum.Parse(typeof(Achievements.list), _list.ToString(), true);
            if (SaveController.Instance.save.achievements.unlock[index]) return;
            toUnlock.Enqueue(index);

            SaveController.Instance.save.totalUnlockAchievementCount++;

            if (SaveController.Instance.save.totalUnlockAchievementCount == System.Enum.GetNames(typeof(Achievements.list)).Length - 1)
            {
                SaveController.Instance.save.totalUnlockAchievementCount++;
                toUnlock.Enqueue((int)System.Enum.Parse(typeof(Achievements.list), Achievements.list.全剧终.ToString(), true));
                //全剧终
            }
        }
    }

    IEnumerator IEUnlockAchievement()
    {
        doing = true;
        unlockAchievementAudio.Play();

        //解锁成就并写回文件
        int index = toUnlock.Dequeue();
        SaveController.Instance.save.achievements.unlock[index] = true;
        //SaveController.Instance.WriteBackToFile();关卡结算时统一写回文件

        //修改对应文本
        text.text = "新成就：" + Achievements.name[index];

        animator.SetBool("Show", true);
        yield return new WaitForSeconds(0.1f);

        float animationLength = animator.runtimeAnimatorController.animationClips[0].length;//获得动画时长
        yield return new WaitForSeconds(animationLength);//等待动画结束

        animator.SetBool("Show", false);

        doing = false;
        yield break;
    }
}
