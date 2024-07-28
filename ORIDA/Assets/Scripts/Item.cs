using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public GameObject itemPrefab; // 아이템에 해당하는 프리팹
    public string equipSlot; // 장착할 슬롯 이름 (예: "EyeSlot")
}