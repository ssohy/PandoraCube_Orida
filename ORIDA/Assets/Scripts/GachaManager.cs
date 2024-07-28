using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public int coins;
    public int coinsPerDraw = 5; // �̱� 1ȸ�� �ʿ��� ���� ��
    public List<GameObject> objects; // 0���� 9������ ������Ʈ ����Ʈ
    public List<GameObject> item; // ���� ������

    public GameObject DrawUI;

    public GameObject noCoinTxt;
    public GameObject allItemTxt;

    private HashSet<int> drawnIndices; // �̹� ���� ��ȣ�� ������ HashSet

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        drawnIndices = new HashSet<int>(); // HashSet �ʱ�ȭ

        LoadObjectStates(); // ������Ʈ ���� �ε�
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 'G' Ű�� ���� �̱� ����
        {
            if (DrawUI.activeSelf) // DrawUI�� Ȱ��ȭ�� ��쿡��
            {
                Draw();
            }
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

        coins -= coinsPerDraw; // �̱� �� ���� ����
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        int randomIndex = GetUniqueRandomIndex(); // �ߺ����� �ʴ� ��ȣ�� �������� �̱�
        if (randomIndex == -1) return;

        Debug.Log("You drew: " + randomIndex);

        if (objects[randomIndex] != null)
        {
            objects[randomIndex].SetActive(false); // ���� ��ȣ�� �ش�Ǵ� ������Ʈ ��Ȱ��ȭ
            PlayerPrefs.SetInt("Object" + randomIndex, 0); // ���� ����
            PlayerPrefs.Save();

            
            item[randomIndex].SetActive(true);
            item[randomIndex].transform.position = new Vector3(170, 130, 0);
            Debug.Log("Ȱ��ȭ");
            StartCoroutine(DrawedItem(randomIndex, 2f)); // 2�� �Ŀ� ������ ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("Object at index " + randomIndex + " is not assigned.");
        }
    }

    int GetUniqueRandomIndex()
    {
        if (drawnIndices.Count >= 20) // ��� ��ȣ�� �̹� �������� Ȯ��
        {
            allItemTxt.SetActive(true);
            StartCoroutine(Done(2f));
            Debug.LogError("All items have been drawn.");
            return -1; // ���� ��Ȳ ó��
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, 20); // 0���� 19������ ��ȣ�� �������� �̱�
        } while (drawnIndices.Contains(randomIndex));

        drawnIndices.Add(randomIndex); // ���� ��ȣ�� HashSet�� �߰�
        return randomIndex;
    }

    void LoadObjectStates()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            int state = PlayerPrefs.GetInt("Object" + i, 1); // �⺻���� Ȱ��ȭ ���� (1)
            objects[i].SetActive(state == 1);
        }
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
