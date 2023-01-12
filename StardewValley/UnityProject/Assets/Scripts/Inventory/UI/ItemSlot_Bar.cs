using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StardewValley.Inventory
{
    public class ItemSlot_Bar : ItemSlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Sprite slotSelectedFrameSprite;
        public Sprite slotUnselectedFrameSprite;

        //settings
        public Vector2 slotBarIconSelectedScale = new Vector2(0.9f, 0.9f);
        public Vector2 slotBarIconUnselectedScale = new Vector2(0.8f, 0.8f);

        public void OnPointerClick(PointerEventData eventData)
        {
            inventoryUI.SelectSlotBar(slotIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnFocused();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnUnfocused();
        }

        public void OnFocused()
        {
            slotIconImage.transform.localScale = slotBarIconSelectedScale;
        }

        public void OnUnfocused()
        {
            slotIconImage.transform.localScale = slotBarIconUnselectedScale;
        }

        public void OnSelected()
        {
            isSelected = true;
            slotFrameImage.sprite = slotSelectedFrameSprite;
            slotIconImage.transform.localScale = slotBarIconSelectedScale;
        }

        public void OnUnselected()
        {
            isSelected = false;
            slotFrameImage.sprite = slotUnselectedFrameSprite;
            slotIconImage.transform.localScale = slotBarIconUnselectedScale;
        }
    }
}


