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
    private int tipsNo = 0; //��ţ������ڶ�ʱ���ڶ�ε���tips��ʱ�����������ȷ����ǰ��Ķ�����Invoke��ǰ�ر� 
    private int lastNo;     //���رպ�����ȡ�ı�ţ������ڱ�ŶԱ�һ����ر�
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
        if (isWaiting)  //�ñ���Ϊ��ʱ�������ر�
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
    public void HideTipsBoard() //�ú������ڶ�ʱ�ر�
    {
        if (lastNo != tipsNo) return;   //������ȣ�˵����������ʾ�屻�ٴε����ˣ���ر�
        waiting = false;
        this.gameObject.SetActive(false);
    }
    public void ClickToHide()   //�ú������ڵ���ر�,��button�������
    {
        if(waiting)
        {
            waiting = false;
            this.gameObject.SetActive(false);
            ContinueText.SetActive(false);
            GameObject.FindWithTag("GameControlCenter").GetComponent<LoadScene>().LoadToScene(0);  //������ҳ��
        }
    }
}
