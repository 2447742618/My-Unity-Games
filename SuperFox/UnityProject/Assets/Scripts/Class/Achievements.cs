using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Achievements
{
    public enum list { 残影杀手, 新世界, 收集者, 另辟蹊径, 屠龙者, 全剧终 };

    public List<bool> unlock = new List<bool>();
    static public List<string> name = new List<string>();
    static public List<string> description = new List<string>();

    public void Init()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(list)).Length; i++)
        {
            unlock.Add(false);
            name.Add("");
            description.Add("");
        }

        foreach (int index in System.Enum.GetValues(typeof(list)))
        {
            name[index] = System.Enum.GetName(typeof(list), index);
        }

        description[(int)list.新世界] = "解锁关卡一中的隐藏地图";
        description[(int)list.收集者] = "收集所有的樱桃与钻石";
        description[(int)list.另辟蹊径] = "通过使BOSS掉落的方式对其造成伤害";
        description[(int)list.残影杀手] = "在冲刺过程中击杀敌人";
        description[(int)list.屠龙者] = "打败最终BOSS";
        description[(int)list.全剧终] = "解锁所有游戏内容";
    }
}
