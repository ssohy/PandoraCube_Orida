using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GachaItem
{
    public string itemName;
    public float probability; // ���� Ȯ��
}

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems; // �̱� ������ ����Ʈ
    public int coins; // ���� ���� ��
    public int coinsPerDraw = 5; // �̱� 1ȸ�� �ʿ��� ���� ��

    void Awake()
    {
        // �����۰� Ȯ�� �ʱ�ȭ (����)
        gachaItems = new List<GachaItem>
        {
            new GachaItem { itemName = "Common Item", probability = 90.0f },
            //new GachaItem { itemName = "Rare Item", probability = 30.0f },
            //new GachaItem { itemName = "Epic Item", probability = 15.0f },
            new GachaItem { itemName = "Legendary Item", probability = 10.0f }
        };
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    public GachaItem DrawItem()
    {
        if (coins < coinsPerDraw)
        {
            Debug.Log("Not enough coins to draw.");
            return null;
        }

        coins -= coinsPerDraw; // �̱� �� ���� ����

        float totalProbability = 0f;

        // ��ü Ȯ�� �ջ�
        foreach (GachaItem item in gachaItems)
        {
            totalProbability += item.probability;
        }

        // ���� Ȯ�� ����
        float randomPoint = Random.value * totalProbability;

        // ���� ����Ʈ�� ���ϴ� ������ ã��
        foreach (GachaItem item in gachaItems)
        {
            if (randomPoint < item.probability)
            {
                return item; // ���� ������ ��ȯ
            }
            else
            {
                randomPoint -= item.probability;
            }
        }

        return null; // ���� ��Ȳ (��ü Ȯ�� ���� 100%�� �ƴ� ���)
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 'G' Ű�� ���� �̱� ����
        {
            GachaItem drawnItem = DrawItem();
            if (drawnItem != null)
            {
                Debug.Log("You drew: " + drawnItem.itemName);
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.Save();
            }
        }
    }
}
