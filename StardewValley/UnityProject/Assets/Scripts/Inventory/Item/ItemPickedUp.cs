using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StardewValley.Inventory
{
    public class ItemPickedUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemDetails.pickedupable)
                {
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }
    }
}

