using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mini1gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }

    public void mini2gameStart()
    {
        SceneManager.LoadScene("MiniGame1");
    }
    public void gameUIstart()
    {
        gameUI.SetActive(true);
    }
    public void gameUIcancle()
    {
        gameUI.SetActive(false);
    }
}
