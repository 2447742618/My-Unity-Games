using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StardewValley.Inventory
{
    public class InventoryUI : MonoBehaviour, IScrollHandler
    {
        [Header("物品栏")]
        [SerializeField] private ItemSlot_Bar[] itemSlot_Bars;
        [SerializeField] private GameObject barUI;


        [Header("玩家背包")]
        [SerializeField] private ItemSlot_Bag[] itemSlot_Bags;
        [SerializeField] private GameObject bagUI;

        [Header("父物体")]
        [SerializeField] private Image inventoryImage;

        [Header("背包当前所选物品")]
        [SerializeField] private GameObject holdItem;
        [SerializeField] private Image holdItemIcon;
        [SerializeField] private TextMeshProUGUI holdItemAmount;

        private bool bagOpened;

        private int slotSelectedIndex;

        private void Start()
        {
            for (int i = 0; i < Settings.bagCapacity; i++) itemSlot_Bars[i].slotIndex = itemSlot_Bags[i].slotIndex = i;
            SelectSlotBar(0);
        }

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.UpdateBagHoldItem += OnUpdateBagHoldItem;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.UpdateBagHoldItem -= OnUpdateBagHoldItem;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) SwitchBagOpen();
            KeySelectSlotBar();
        }

        /// <summary>
        /// 更新物品栏与背包UI
        /// </summary>
        /// <param name="location"></param>
        /// <param name="list"></param>
        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < Settings.bagCapacity; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            itemSlot_Bars[i].UpdateSlot(item, list[i].itemAmount);
                            itemSlot_Bags[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            itemSlot_Bars[i].ClearSlot();
                            itemSlot_Bags[i].ClearSlot();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 控制背包UI开关
        /// </summary>
        public void SwitchBagOpen()
        {
            if (bagOpened && holdItem.activeInHierarchy) return;//选中物品的情况下无法关闭背包窗口

            bagOpened = !bagOpened;

            if (bagOpened) inventoryImage.color = new Color(0, 0, 0, Settings.darkALpha);
            else inventoryImage.color = new Color(0, 0, 0, 0);

            //TODO:修改为关闭其他所有窗口（函数）
            bagUI.SetActive(bagOpened);
            barUI.SetActive(!bagOpened);
        }

        /// <summary>
        /// 用于选择指定物品框
        /// </summary>
        /// <param name="index"></param>
        public void SelectSlotBar(int index)
        {
            slotSelectedIndex = index;

            foreach (var slot in itemSlot_Bars)
            {
                if (slot.slotIndex == index) slot.OnSelected();
                else slot.OnUnselected();
            }
        }

        /// <summary>
        /// 用于处理键盘选择物品框
        /// </summary>
        public void KeySelectSlotBar()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlotBar(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlotBar(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlotBar(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlotBar(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlotBar(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlotBar(5);
            if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlotBar(6);
            if (Input.GetKeyDown(KeyCode.Alpha8)) SelectSlotBar(7);
            if (Input.GetKeyDown(KeyCode.Alpha9)) SelectSlotBar(8);
            if (Input.GetKeyDown(KeyCode.Alpha0)) SelectSlotBar(9);
            if (Input.GetKeyDown(KeyCode.Minus)) SelectSlotBar(10);
            if (Input.GetKeyDown(KeyCode.Equals)) SelectSlotBar(11);
        }

        /// <summary>
        /// 用于处理滚轮切换所选物品框
        /// </summary>
        /// <param name="pointerEventData"></param>
        public void OnScroll(PointerEventData pointerEventData)
        {
            if (barUI.activeInHierarchy == false || CheckBarEmpty()) return;

            int dir = (int)-pointerEventData.scrollDelta.y;
            for (int nextIndex = slotSelectedIndex + dir; ; nextIndex += dir)
            {
                nextIndex = (nextIndex + Settings.bagCapacity) % Settings.bagCapacity;
                if (itemSlot_Bars[nextIndex].itemAmount != 0)
                {
                    slotSelectedIndex = nextIndex;
                    SelectSlotBar(slotSelectedIndex);
                    break;
                }
            }
        }

        /// <summary>
        /// 用于检查物品栏是否全空，用于忽略滚轮事件
        /// </summary>
        /// <returns></returns>
        private bool CheckBarEmpty()
        {
            foreach (var slot in itemSlot_Bars)
            {
                if (slot.itemAmount != 0) return false;
            }
            return true;
        }

        /// <summary>
        /// 更新背包中所选物品的图标
        /// </summary>
        public void OnUpdateBagHoldItem(InventoryItem item)
        {
            if (item.itemAmount == 0)
            {
                holdItem.SetActive(false);
            }
            else
            {
                holdItem.SetActive(true);

                var icon = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
                var amount = item.itemAmount;

                holdItemIcon.sprite = icon;
                holdItemIcon.SetNativeSize();
                holdItemAmount.text = amount.ToString();
            }
        }
    }
}


