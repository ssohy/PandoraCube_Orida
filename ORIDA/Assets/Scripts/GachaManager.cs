using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public int coins;
    public int coinsPerDraw = 5; // 뽑기 1회당 필요한 코인 수
    public List<GameObject> objects; // 0부터 9까지의 오브젝트 리스트
    public List<GameObject> item; // 뽑힌 아이템

    public GameObject DrawUI;
    public GameObject noCoinTxt;
    public GameObject allItemTxt;

    private HashSet<int> drawnIndices; // 이미 뽑힌 번호를 저장할 HashSet

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        drawnIndices = new HashSet<int>(); // HashSet 초기화

        LoadDrawnIndices(); // 뽑힌 번호 로드
        LoadObjectStates(); // 오브젝트 상태 로드
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 'G' 키를 눌러 뽑기 수행
        {
            if (DrawUI.activeSelf) // DrawUI가 활성화된 경우에만
            {
                Draw();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) // 'R' 키를 눌러 초기화 수행
        {
            ResetPlayerPrefs();
        }
    }

    void Draw()
    {
        if (coins < coinsPerDraw)
        {
            noCoinTxt.SetActive(true);
            StartCoroutine(Nocoin(2f));
            Debug.Log("Not enough coins to draw.");
            return;
        }

        coins -= coinsPerDraw; // 뽑기 시 코인 차감
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        int randomIndex = GetUniqueRandomIndex(); // 중복되지 않는 번호를 랜덤으로 뽑기
        if (randomIndex == -1) return;

        Debug.Log("You drew: " + randomIndex);

        if (objects[randomIndex] != null)
        {
            objects[randomIndex].SetActive(false); // 뽑힌 번호에 해당되는 오브젝트 비활성화
            PlayerPrefs.SetInt("Object" + randomIndex, 0); // 상태 저장
            PlayerPrefs.Save();

            item[randomIndex].SetActive(true);
            Debug.Log("활성화");
            StartCoroutine(DrawedItem(randomIndex, 2f)); // 2초 후에 아이템 비활성화

            SaveDrawnIndices(); // 뽑힌 번호 저장
        }
        else
        {
            Debug.LogError("Object at index " + randomIndex + " is not assigned.");
        }
    }

    int GetUniqueRandomIndex()
    {
        if (drawnIndices.Count >= 20) // 모든 번호가 이미 뽑혔는지 확인
        {
            allItemTxt.SetActive(true);
            StartCoroutine(Done(2f));
            Debug.LogError("All items have been drawn.");
            return -1; // 예외 상황 처리
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, 20); // 0부터 19까지의 번호를 랜덤으로 뽑기
        } while (drawnIndices.Contains(randomIndex));

        drawnIndices.Add(randomIndex); // 뽑힌 번호를 HashSet에 추가
        return randomIndex;
    }

    void LoadObjectStates()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            int state = PlayerPrefs.GetInt("Object" + i, 1); // 기본값은 활성화 상태 (1)
            objects[i].SetActive(state == 1);
        }
    }

    void SaveDrawnIndices()
    {
        PlayerPrefs.SetInt("DrawnIndicesCount", drawnIndices.Count);
        int index = 0;
        foreach (int drawnIndex in drawnIndices)
        {
            PlayerPrefs.SetInt("DrawnIndex_" + index, drawnIndex);
            index++;
        }
        PlayerPrefs.Save();
    }

    void LoadDrawnIndices()
    {
        int count = PlayerPrefs.GetInt("DrawnIndicesCount", 0);
        for (int i = 0; i < count; i++)
        {
            int drawnIndex = PlayerPrefs.GetInt("DrawnIndex_" + i);
            drawnIndices.Add(drawnIndex);
        }
    }

    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        drawnIndices.Clear();
        foreach (var obj in objects)
        {
            obj.SetActive(true);
        }
        coins = 0;
        Debug.Log("PlayerPrefs has been reset.");
    }

    IEnumerator DrawedItem(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        item[index].SetActive(false);
    }

    IEnumerator Done(float delay)
    {
        yield return new WaitForSeconds(delay);
        allItemTxt.SetActive(false);
    }

    IEnumerator Nocoin(float delay)
    {
        yield return new WaitForSeconds(delay);
        noCoinTxt.SetActive(false);
    }
}
