using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Unity.IO;
using System;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    public Save save;

    private void Awake()
    {
        save.Init();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //首先获得存档信息
        if (File.Exists(Application.persistentDataPath + "/save.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Open);
            if (file.Length != 0) save = (Save)bf.Deserialize(file);
            file.Close();
        }
        else File.Create(Application.persistentDataPath + "/save.data");
    }

    private void ResetSave()
    {
        if (File.Exists(Application.persistentDataPath + "/save.data")) File.Delete(Application.persistentDataPath + "/save.data");
    }

    public void LoadLevelInformation(int level)//加载关卡信息
    {
        foreach (string name in save.levelInformation[level])
        {
            GameObject gameObject = GameObject.Find("Collections").transform.Find(name).gameObject;
            if (gameObject != null) Destroy(gameObject);//销毁已经收集过的物品
        }
    }

    public void UpdateLevelInformation(int level, List<string> collected)//更新关卡信息
    {
        save.levelPass[level] = true;
        if (level + 1 <= Save.level) SaveController.Instance.save.levelUnlock[level + 1] = true;

        save.levelInformation[level].AddRange(collected);//保证不会有重复项

        WriteBackToFile();
    }

    public void UpdateCollectionInformation(int cherry, int gem)//更新信息
    {
        save.cherry += cherry;
        save.gem += gem;

        save.totalGemCollected += gem;
        save.totalCherryCollectedCount += cherry;

        WriteBackToFile();
    }

    public void WriteBackToFile()//写回文件
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Open);
        bf.Serialize(file, save);
        file.Close();
    }

    public double LevelRateCalculate(int index)
    {
        double rate = 0;
        if (save.levelPass[index]) rate += 50;

        if (save.levelCollectionNumber[index] == 0) rate += 50;
        else rate += 50f * save.levelInformation[index].Count / save.levelCollectionNumber[index];

        rate = Math.Round(rate, 1);
        return rate;
    }
}
