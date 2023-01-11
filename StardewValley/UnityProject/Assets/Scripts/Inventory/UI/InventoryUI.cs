using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StardewValley.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private ItemSlot_Bar[] itemSlot_Bars;
        [SerializeField] private ItemSlot_Bag[] itemSlot_Bags;

        private void Start()
        {
            for (int i = 0; i < Settings.bagCapacity; i++) itemSlot_Bars[i].slotIndex = itemSlot_Bags[i].slotIndex = i;

        }

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        }

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
    }
}


