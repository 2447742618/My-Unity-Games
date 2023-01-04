using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Inventory/ItenDataList")]
public class ItemDataListSO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
}
