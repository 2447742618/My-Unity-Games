using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StardewValley.Inventory
{
    public class ItemSlot : MonoBehaviour
    {
        [Header("组件获取")]
        [SerializeField] protected Image slotIconImage;
        [SerializeField] protected Image slotFrameImage;
        [SerializeField] protected TextMeshProUGUI amountText;
        [SerializeField] protected Button button;

        public bool isSelected;

        public ItemDetails itemDetails;
        public int itemAmount;

        public int slotIndex;

        protected InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0) ClearSlot();
        }

        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotIconImage.sprite = item.itemIcon;
            slotIconImage.enabled = true;
            itemAmount = amount;
            amountText.text = itemAmount.ToString();
            button.interactable = true;
        }

        public void ClearSlot()
        {
            if (isSelected) isSelected = false;

            slotIconImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }
    }
}

