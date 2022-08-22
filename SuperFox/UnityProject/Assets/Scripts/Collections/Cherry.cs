using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Collections
{
    public override void Collected()
    {
        base.Collected();
        Player.cherry++;

        if (Player.cherry + Player.gem + SaveController.Instance.save.totalCherryCollectedCount + SaveController.Instance.save.totalGemCollected == SaveController.Instance.save.totalCollectionsCount)
        {
            AchievementController.Instance.UnlockAchievement(Achievements.list.收集者);//收集所有的收集品
        }
    }
}
