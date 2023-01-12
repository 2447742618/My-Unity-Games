using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StardewValley.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        [Header("玩家背包数据")]
        public InventoryBag_SO playerBag;
        [Header("UI控制")]
        [SerializeField] private InventoryUI inventoryUI;

        private InventoryItem holdInventoryItem = new InventoryItem();

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 通过ID获得物品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// 添加物品到背包中
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory"></param>
        public void AddItem(Item item, bool toDestory)
        {
            int index = GetItemIndexInBag(item.itemID);

            AddItemAtIndex(item.itemID, index, 1);

            if (toDestory) Destroy(item.gameObject);

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }


        /// <summary>
        /// 通过ID判断背包中是否存在该物体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID) return i;
            }
            return -1;
        }

        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        public bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemAmount == 0) return true;
            }
            return false;

        }

        /// <summary>
        /// 在指定下标处添加指定数量的物品
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else if (index != -1)
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount + playerBag.itemList[index].itemAmount };
                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// 用于处理左键选择背包物品栏操作
        /// </summary>
        /// <param name="index"></param>
        public void BagSlotLeftClicked(int index)
        {
            if (holdInventoryItem.itemID == playerBag.itemList[index].itemID && holdInventoryItem.itemAmount != 0)//叠加操作
            {
                AddItemAtIndex(holdInventoryItem.itemID, index, holdInventoryItem.itemAmount);
                holdInventoryItem.itemAmount = 0;
            }
            else
            {
                InventoryItem bagItem = playerBag.itemList[index];
                playerBag.itemList[index] = holdInventoryItem;
                holdInventoryItem = bagItem;
            }


            //更新UI
            EventHandler.CallUpdateBagHoldItem(holdInventoryItem);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 用于处理右键选择背包物品栏操作
        /// </summary>
        /// <param name="index"></param>
        public void BagSLotRIghtClicked(int index)
        {
            if (holdInventoryItem.itemAmount != 0 && holdInventoryItem.itemID != playerBag.itemList[index].itemID) return;
            if (playerBag.itemList[index].itemAmount == 0) return;

            holdInventoryItem.itemID = playerBag.itemList[index].itemID;
            holdInventoryItem.itemAmount++;
            AddItemAtIndex(playerBag.itemList[index].itemID, index, -1);

            //更新UI
            EventHandler.CallUpdateBagHoldItem(holdInventoryItem);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}

