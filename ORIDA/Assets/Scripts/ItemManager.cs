using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> items; // 아이템 리스트
    public Transform character; // 캐릭터의 부모 오브젝트
    public Transform home;
    private Dictionary<string, Transform> equipSlots; // 장착 슬롯 딕셔너리
    private Dictionary<string, GameObject> equippedItems; // 장착된 아이템 딕셔너리

    void Start()
    {
        equipSlots = new Dictionary<string, Transform>();
        equippedItems = new Dictionary<string, GameObject>();

        // 캐릭터의 자식 오브젝트들 중 슬롯을 찾아 딕셔너리에 추가
        foreach (Transform child in character)
        {
            if (child.name.EndsWith("Slot"))
            {
                equipSlots[child.name] = child;
            }
        }

        // 캐릭터 꾸미기 아이템
        AddSlotToDictionary("check", character);
        AddSlotToDictionary("neck", character);
        AddSlotToDictionary("eyes", character);
        AddSlotToDictionary("head", character);
        AddSlotToDictionary("hand", character);

        // 집 꾸미기 아이템
        AddSlotToDictionary("wall", home);
        AddSlotToDictionary("window", home);
        AddSlotToDictionary("pot", home);
        AddSlotToDictionary("furniture", home);
        AddSlotToDictionary("doll", home);
        AddSlotToDictionary("pet", home);

        LoadEquippedItems(); // 장착된 아이템 로드
    }

    void AddSlotToDictionary(string slotName, Transform parent)
    {
        Transform slot = parent.Find(slotName);
        if (slot != null)
        {
            equipSlots.Add(slotName, slot);
        }
    }

    public void Put_On(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= items.Count)
        {
            Debug.LogError("Invalid item index");
            return;
        }

        Item item = items[itemIndex];
        if (equipSlots.ContainsKey(item.equipSlot))
        {
            // 같은 슬롯에 이미 장착된 아이템이 있는지 확인
            if (equippedItems.ContainsKey(item.equipSlot))
            {
                GameObject currentEquippedItem = equippedItems[item.equipSlot];

                // 이미 장착된 아이템을 다시 클릭한 경우, 해당 아이템 해제
                if (currentEquippedItem.name == item.itemName)
                {
                    Destroy(currentEquippedItem);
                    equippedItems.Remove(item.equipSlot);
                    PlayerPrefs.DeleteKey(item.equipSlot);
                    PlayerPrefs.Save();
                    Debug.Log("아이템 해제: " + item.itemName);
                    return;
                }
                else
                {
                    // 다른 아이템이 장착된 경우 기존 아이템 해제
                    Destroy(currentEquippedItem);
                    equippedItems.Remove(item.equipSlot);
                }
            }

            // 아이템을 장착
            GameObject newEquippedItem = Instantiate(item.itemPrefab, equipSlots[item.equipSlot]);
            newEquippedItem.name = item.itemName;

            // 위치 및 정렬 설정
            newEquippedItem.transform.localPosition = Vector3.zero; // 슬롯의 중심으로 위치 설정
            newEquippedItem.transform.localRotation = Quaternion.identity; // 회전 초기화
            newEquippedItem.transform.localScale = Vector3.one; // 크기 초기화

            // 필요한 경우 Z축 또는 레이어 설정
            newEquippedItem.transform.localPosition = new Vector3(
                newEquippedItem.transform.localPosition.x,
                newEquippedItem.transform.localPosition.y,
                0); // 2D 게임의 경우 Z축을 0으로 설정

            // 레이어 설정 (옵션)
            SpriteRenderer spriteRenderer = newEquippedItem.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Foreground"; // 레이어 이름 설정
                spriteRenderer.sortingOrder = 5; // 정렬 순서 설정
            }

            Debug.Log("아이템 장착: " + item.itemName);
            equippedItems[item.equipSlot] = newEquippedItem; // 장착된 아이템 딕셔너리에 추가

            SaveEquippedItem(item); // 장착된 아이템 저장
        }
        else
        {
            Debug.LogError("Equip slot not found: " + item.equipSlot);
        }
    }

    public void Take_Off()
    {
        foreach (var equippedItem in equippedItems.Values)
        {
            Destroy(equippedItem);
        }
        equippedItems.Clear();
        ClearSavedEquippedItems(); // 저장된 장착 정보 초기화
    }

    void SaveEquippedItem(Item item)
    {
        PlayerPrefs.SetString(item.equipSlot, item.itemName);
        PlayerPrefs.Save();
    }

    void LoadEquippedItems()
    {
        foreach (var slot in equipSlots.Keys)
        {
            if (PlayerPrefs.HasKey(slot))
            {
                string itemName = PlayerPrefs.GetString(slot);
                Item item = items.Find(i => i.itemName == itemName);
                if (item != null)
                {
                    Put_On(items.IndexOf(item)); // 아이템 장착
                }
            }
        }
    }

    void ClearSavedEquippedItems()
    {
        foreach (var slot in equipSlots.Keys)
        {
            PlayerPrefs.DeleteKey(slot);
        }
        PlayerPrefs.Save();
    }
}
