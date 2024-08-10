using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameUI;
    public int coins;
    public TMP_Text coinsText;

    public GameObject Draw;
    public GameObject drawItemUI;
    private CanvasGroup DrawCanvasGroup;

    public GameObject Mine;
    private CanvasGroup MineCanvasGroup;

    public GameObject page1UI;
    public GameObject page2UI;

    public GameObject CLOSET;
    public GameObject HOME;

    public GameObject player;

    void Awake()
    {
        // PlayerPrefs에서 코인 값 가져오기
        coins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("현재 코인 : " + coins);

        // CanvasGroup 초기화
        DrawCanvasGroup = Draw.GetComponent<CanvasGroup>();
        if (DrawCanvasGroup == null)
        {
            DrawCanvasGroup = Draw.AddComponent<CanvasGroup>();
        }

        MineCanvasGroup = Mine.GetComponent<CanvasGroup>();
        if (MineCanvasGroup == null)
        {
            MineCanvasGroup = Mine.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = string.Format("{0:n0}", coins);

        // page1UI 또는 page2UI가 활성화되어 있으면 Draw 비활성화
        if (page1UI.activeSelf || page2UI.activeSelf)
        {
            DrawCanvasGroup.interactable = false;
            DrawCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            DrawCanvasGroup.interactable = true;
            DrawCanvasGroup.blocksRaycasts = true;
        }

        // drawItemUI가 활성화되어 있으면 Mine 비활성화
        if (drawItemUI.activeSelf)
        {
            MineCanvasGroup.interactable = false;
            MineCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            MineCanvasGroup.interactable = true;
            MineCanvasGroup.blocksRaycasts = true;
        }
    }

    // #.Main Scene
    public void Mini1gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void Mini2gameStart()
    {
        SceneManager.LoadScene("MiniGame2");
    }

    public void ClosetLoad()
    {
        CLOSET.SetActive(true);
        HOME.SetActive(false);
        player.transform.position = new Vector3(5, 0, 0);
    }

    public void homeButton()
    {
        HOME.SetActive(true);
        CLOSET.SetActive(false);
        player.transform.position = new Vector3(0, -1, 0);
    }
    public void GameUIstart()
    {
        gameUI.SetActive(true);
    }
    public void GameUIcancle()
    {
        gameUI.SetActive(false);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit();
        #endif
    }

    // #.closet Scene
    public void DrawItem()
    {
        drawItemUI.SetActive(true);
    }
    public void CancleDrawItem()
    {
        drawItemUI.SetActive(false);
    }

    public void Page1()
    {
        page1UI.SetActive(true);
    }
    public void CanclePage1()
    {
        page1UI.SetActive(false);
    }
    public void Page2()
    {
        page2UI.SetActive(true);
    }
    public void CanclePage2()
    {
        page2UI.SetActive(false);
    }
}
