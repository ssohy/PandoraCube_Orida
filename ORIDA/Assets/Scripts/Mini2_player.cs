using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini2_player : MonoBehaviour
{
    public Animator anim;
    public AudioSource[] sound;
    public Mini2_GameManager gameManager;
    public bool isleft = true, isDie = false;
    public int characterIndex, stairIndex;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Climb(false);
        }

        // 스페이스바를 눌러 방향전환
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Climb(true);
        }
    }
    public void Climb(bool isChange)
    {
        if (isChange) isleft = !isleft;
        gameManager.StairMove(stairIndex, isChange, isleft);
        if ((++stairIndex).Equals(20)) stairIndex = 0;
        MoveAnimation();
        gameManager.gaugeStart = true;
    }


    public void MoveAnimation()
    {
        //Change left and right when changing direction
        if (!isleft)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isDie) return;
        anim.SetBool("Move", true);
        Invoke("IdleAnimation", 0.05f);
    }

    public void IdleAnimation()
    {
        anim.SetBool("Move", false);
    }


}
