using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TipsBoard : MonoBehaviour
{
    public static TipsBoard Instance { get;private set; }
    public bool waiting = false;
    public GameObject ContinueText;
    private int tipsNo = 0; //编号，用于在短时间内多次调用tips板时，后面的能正确覆盖前面的而不被Invoke提前关闭 
    private int lastNo;     //供关闭函数读取的编号，和现在编号对比一样则关闭
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        HideTipsBoard();
    }
    private void Update()
    {/*
        if (waiting)
        {
            ContinueText.GetComponent<Animator>().Play("Sparkle");
        }*/
    }
    public void ShowTipsBoard(string s,bool isWaiting = false)
    {
        tipsNo++;
        this.gameObject.SetActive(true);
        GetComponent<CanvasGroup>().alpha = 1;
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = s;

        ContinueText.SetActive(isWaiting);
        if (isWaiting)  //该变量为真时不主动关闭
        {
            waiting = true;
        }
        else
        {
            waiting = false;
            lastNo = tipsNo;
            Invoke("HideTipsBoard", 2.0f);
        }
    }
    public void HideTipsBoard() //该函数用于定时关闭
    {
        if (lastNo != tipsNo) return;   //若不相等，说明两秒内提示板被再次调用了，别关闭
        waiting = false;
        this.gameObject.SetActive(false);
    }
    public void ClickToHide()   //该函数用于点击关闭,被button组件引用
    {
        if(waiting)
        {
            waiting = false;
            this.gameObject.SetActive(false);
            ContinueText.SetActive(false);
            GameObject.FindWithTag("GameControlCenter").GetComponent<LoadScene>().LoadToScene(0);  //返回主页面
        }
    }
}
