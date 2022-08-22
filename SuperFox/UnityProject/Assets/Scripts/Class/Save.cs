using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save//用于保存所有游戏信息
{
    static public int level = 3;

    public List<int> levelCollectionNumber = new List<int>(level);
    public List<List<string>> levelInformation = new List<List<string>>(level);//保存已经收集过的收集品，用其size计算关卡进度
    public List<bool> levelUnlock = new List<bool>(level);
    public List<bool> levelPass = new List<bool>(level);

    public int cherry, gem, life;//游戏数值

    public bool doubleJump, dash;//技能
    public bool bossAlive;

    public float skullPositionX;

    public int totalGemCollected, totalCherryCollectedCount, totalCollectionsCount;
    public Achievements achievements = new Achievements();//成就
    public int totalUnlockAchievementCount;

    public bool firstEnterLevel3;

    public void Init()
    {
        cherry = gem = totalCherryCollectedCount = totalGemCollected = totalUnlockAchievementCount = 0;
        life = 3;
        doubleJump = dash = false;
        bossAlive = firstEnterLevel3 = true;

        //初始化List
        for (int i = 0; i <= level; i++)
        {
            levelInformation.Add(new List<string>());
            levelUnlock.Add(false);
            levelPass.Add(false);
            levelCollectionNumber.Add(-1);
        }

        levelUnlock[1] = true;//第一关默认解锁

        //初始化关卡收集品总数量
        levelCollectionNumber[1] = 15;
        levelCollectionNumber[2] = 5;
        levelCollectionNumber[3] = 0;

        totalCollectionsCount = 0;
        for (int i = 1; i < level; i++) totalCollectionsCount += levelCollectionNumber[i];

        achievements.Init();//初始化成就
    }
}
