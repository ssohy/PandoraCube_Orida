using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> items; // ������ ����Ʈ
    public Transform character; // ĳ������ �θ� ������Ʈ
    public Transform home;
    private Dictionary<string, Transform> equipSlots; // ���� ���� ��ųʸ�
    private Dictionary<string, GameObject> equippedItems; // ������ ������ ��ųʸ�

    void Start()
    {
        equipSlots = new Dictionary<string, Transform>();
        equippedItems = new Dictionary<string, GameObject>();

        // ĳ������ �ڽ� ������Ʈ�� �� ������ ã�� ��ųʸ��� �߰�
        foreach (Transform child in character)
        {
            if (child.name.EndsWith("Slot"))
            {
                equipSlots[child.name] = child;
            }
        }

        // ĳ���� �ٹ̱� ������
        AddSlotToDictionary("check", character);
        AddSlotToDictionary("neck", character);
        AddSlotToDictionary("eyes", character);
        AddSlotToDictionary("head", character);
        AddSlotToDictionary("hand", character);

        // �� �ٹ̱� ������
        AddSlotToDictionary("wall", home);
        AddSlotToDictionary("window", home);
        AddSlotToDictionary("pot", home);
        AddSlotToDictionary("furniture", home);
        AddSlotToDictionary("doll", home);
        AddSlotToDictionary("pet", home);

        LoadEquippedItems(); // ������ ������ �ε�
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
            // ���� ���Կ� �̹� ������ �������� �ִ��� Ȯ��
            if (equippedItems.ContainsKey(item.equipSlot))
            {
                GameObject currentEquippedItem = equippedItems[item.equipSlot];

                // �̹� ������ �������� �ٽ� Ŭ���� ���, �ش� ������ ����
                if (currentEquippedItem.name == item.itemName)
                {
                    Destroy(currentEquippedItem);
                    equippedItems.Remove(item.equipSlot);
                    PlayerPrefs.DeleteKey(item.equipSlot);
                    PlayerPrefs.Save();
                    Debug.Log("������ ����: " + item.itemName);
                    return;
                }
                else
                {
                    // �ٸ� �������� ������ ��� ���� ������ ����
                    Destroy(currentEquippedItem);
                    equippedItems.Remove(item.equipSlot);
                }
            }

            // �������� ����
            GameObject newEquippedItem = Instantiate(item.itemPrefab, equipSlots[item.equipSlot]);
            newEquippedItem.name = item.itemName;

            // ��ġ �� ���� ����
            newEquippedItem.transform.localPosition = Vector3.zero; // ������ �߽����� ��ġ ����
            newEquippedItem.transform.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
            newEquippedItem.transform.localScale = Vector3.one; // ũ�� �ʱ�ȭ

            // �ʿ��� ��� Z�� �Ǵ� ���̾� ����
            newEquippedItem.transform.localPosition = new Vector3(
                newEquippedItem.transform.localPosition.x,
                newEquippedItem.transform.localPosition.y,
                0); // 2D ������ ��� Z���� 0���� ����

            // ���̾� ���� (�ɼ�)
            SpriteRenderer spriteRenderer = newEquippedItem.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Foreground"; // ���̾� �̸� ����
                spriteRenderer.sortingOrder = 5; // ���� ���� ����
            }

            Debug.Log("������ ����: " + item.itemName);
            equippedItems[item.equipSlot] = newEquippedItem; // ������ ������ ��ųʸ��� �߰�

            SaveEquippedItem(item); // ������ ������ ����
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
        ClearSavedEquippedItems(); // ����� ���� ���� �ʱ�ȭ
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
                    Put_On(items.IndexOf(item)); // ������ ����
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
