using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GachaItem
{
    public string itemName;
    public float probability; // 뽑힐 확률
}

public class GachaManager : MonoBehaviour
{
    public List<GachaItem> gachaItems; // 뽑기 아이템 리스트
    public int coins; // 현재 코인 수
    public int coinsPerDraw = 5; // 뽑기 1회당 필요한 코인 수

    void Awake()
    {
        // 아이템과 확률 초기화 (예시)
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

        coins -= coinsPerDraw; // 뽑기 시 코인 차감

        float totalProbability = 0f;

        // 전체 확률 합산
        foreach (GachaItem item in gachaItems)
        {
            totalProbability += item.probability;
        }

        // 랜덤 확률 생성
        float randomPoint = Random.value * totalProbability;

        // 랜덤 포인트가 속하는 아이템 찾기
        foreach (GachaItem item in gachaItems)
        {
            if (randomPoint < item.probability)
            {
                return item; // 뽑힌 아이템 반환
            }
            else
            {
                randomPoint -= item.probability;
            }
        }

        return null; // 예외 상황 (전체 확률 합이 100%가 아닐 경우)
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 'G' 키를 눌러 뽑기 수행
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
