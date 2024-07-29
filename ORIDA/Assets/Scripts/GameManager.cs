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

    public GameObject myItemUI;
    public GameObject drawItemUI;

    public GameObject page1UI;
    public GameObject page2UI;


    public GameObject page1Btn;
    public GameObject page2Btn;

    public GameObject CLOSET;
    public GameObject HOME;

    public GameObject player;

    /*
    private void Awake()
    {
        coins = 0;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }
    */
    void Update()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = string.Format("{0:n0}", coins);
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
    public void SelectingItem()
    {
        myItemUI.SetActive(true);
    }

    public void CancleSelectingItem()
    {
        myItemUI.SetActive(false);
    }

    public void PageBtnON()
    {
        page1Btn.SetActive(true);
        page2Btn.SetActive(true);
    }
    public void PageBtnOFF()
    {
        page1Btn.SetActive(false);
        page2Btn.SetActive(false);
    }


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
