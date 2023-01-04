using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string name;

    public ItemType itemType;

    public Sprite itemIcon;

    public Sprite itemOnWorldSprite;

    public string itemDescription;

    public int itemUseRadius;

    public bool pickable, carriable,dropable;

    public int itemPrice;

    [Range(0,1)]
    public float solePercentage;
}
