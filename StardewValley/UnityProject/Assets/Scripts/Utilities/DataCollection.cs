using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string itemName;

    public ItemType itemType;

    public Sprite itemIcon;

    public Sprite itemOnWorldSprite;

    public string itemDescription;

    public int itemUseRadius;

    public bool pickedupable, carriedable, droppedable;

    public int itemPrice;

    [Range(0, 1)]
    public float soldPercentage;
}


[System.Serializable]
public struct InventoryItem
{
    public int itemID;

    public int itemAmount;
}
