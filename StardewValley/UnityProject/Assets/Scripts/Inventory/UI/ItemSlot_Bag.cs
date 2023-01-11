using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StardewValley.Inventory
{
    public class ItemSlot_Bag : ItemSlot, IPointerEnterHandler, IPointerExitHandler
    {
        //settings
        public Vector2 slotBagIconSelectedScale = new Vector2(1.0f, 1.0f);
        public Vector2 slotBagIconUnselectedScale = new Vector2(0.9f, 0.9f);

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
            slotIconImage.transform.localScale = slotBagIconSelectedScale;
        }

        public void OnUnfocused()
        {
            slotIconImage.transform.localScale = slotBagIconUnselectedScale;
        }
    }
}


