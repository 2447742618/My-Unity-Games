using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    /// <summary>
    /// 事件：更新物品栏与背包UI
    /// </summary>
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    /// <summary>
    /// 调用事件
    /// </summary>
    /// <param name="location"></param>
    /// <param name="list"></param>
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }


    /// <summary>
    /// 事件：更新背包中所选物品的图标
    /// </summary>
    public static event Action<InventoryItem> UpdateBagHoldItem;

    /// <summary>
    /// 调用事件
    /// </summary>
    /// <param name="item"></param>
    public static void CallUpdateBagHoldItem(InventoryItem item)
    {
        UpdateBagHoldItem?.Invoke(item);
    }
}
