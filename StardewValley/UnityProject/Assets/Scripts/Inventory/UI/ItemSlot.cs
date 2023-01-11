using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StardewValley.Inventory
{
    public class ItemSlot : MonoBehaviour
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Button button;

        public bool isSelected;

        public ItemDetails itemDetails;
        public int itemAmount;

        public int slotIndex;

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0) ClearSlot();
        }

        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            slotImage.enabled = true;
            itemAmount = amount;
            amountText.text = itemAmount.ToString();
            button.interactable = true;
        }

        public void ClearSlot()
        {
            if (isSelected) isSelected = false;

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }
    }
}

