using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    /// <summary>
    /// 注册事件
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
}
