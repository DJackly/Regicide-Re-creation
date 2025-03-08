using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionalBox : MonoBehaviour
{
    public static OptionalBox Instance { get; private set; }
    private int OptionalBoxNum = 0;
    public GameObject PokerObject;
    public GameObject SelectButton;
    public GameObject CancelButton;
    public List<GameObject> SelectedPokerList;
    private bool buttonCondition = false;
    public PlayCard playCardScript;

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
        this.transform.position = new Vector3(0, 0, 5);
        SelectButton = GameObject.Find("OptionalBox/SelectButton");
        CancelButton = GameObject.Find("OptionalBox/CancelButton");
    }
    public bool SelectPoker(GameObject poker)   //����ֵ�����Ƿ�ѡ��ɹ�
    {
        //���ڳ��ƽ׶Σ�  �˴����ѡ����ſ�ʱ�Ƿ���Ϲ���
        if (playCardScript.isPlayCardPhase && SelectedPokerList.Count == 1)    //����ѡ��ťǰ��ѡ��һ�ţ�˵��Ҫѡ�ڶ�����
        {
            if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != 1)   //��һ��δѡ����￨
            {       //�� δѡ��ͬ���ֿ���������
                if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != poker.GetComponent<Poker>().cardNumber)
                {
                    if (poker.GetComponent<Poker>().cardNumber != 1)   //���ֲ�ͬ �� �ڶ���ѡ��Ҳ���ǳ��￨
                    {
                        TipsBoard.Instance.ShowTipsBoard("����ͨ�����￨ ���� ͬ���ֵĿ���������");
                        return false;
                    }
                    //else;   //���ֲ�ͬ �� �ڶ���ѡ���￨ �����
                }
                else if (poker.GetComponent<Poker>().cardNumber * 2 > 10) // ͬ���ֿ����е�������10 ����
                {
                    TipsBoard.Instance.ShowTipsBoard("ͬ���ֵĿ���������ʱ����֮�Ͳ��ܴ���10");
                    return false;
                }
                //else;   //��һ��δѡ����￨ ����������ͬ�������� ����ִ��ѡ�����
            }
            //else;// ��һ�ž��ǳ��￨���ڶ���ѡʲô����
        }
        else if (playCardScript.isPlayCardPhase && SelectedPokerList.Count > 1)    //�Ѿ�ѡ�������ţ��������ѡ������
        {
            bool flag = false;
            for (int i = SelectedPokerList.Count - 1; i >= 0; i--)
            {
                if (SelectedPokerList[i].GetComponent<Poker>().cardNumber == 1) //ȷ����ѡ��������д�
                    flag = true;
            }
            if (flag)   //��������
            {
                TipsBoard.Instance.ShowTipsBoard("ʹ�ó��￨��������ʱֻ�ܳ�������");
                return false;
            }
            else  //ͬ��������
            {
                if (poker.GetComponent<Poker>().cardNumber != SelectedPokerList[0].GetComponent<Poker>().cardNumber)
                {
                    TipsBoard.Instance.ShowTipsBoard("����ͨ�����￨ ���� ͬ���ֵĿ���������");
                    return false;
                }
                else if (poker.GetComponent<Poker>().cardNumber * (SelectedPokerList.Count + 1) > 10)
                {
                    TipsBoard.Instance.ShowTipsBoard("ͬ���ֵĿ���������ʱ����֮�Ͳ��ܴ���10");
                    return false;
                }
            }
        }
        //������

        SelectedPokerList.Add(poker); //��ѡ��Ŀ������б�
        return true;
    }
    public void RemovePoker(GameObject poker)
    {
        if(poker != null && SelectedPokerList.Contains(poker))
        {
            SelectedPokerList.Remove(poker);
        }
    }

    private void Update()
    {
        if (buttonCondition)    //true����Ĭ�ϵġ�ѡ��
        {
            SelectButton.SetActive(true);
            CancelButton.SetActive(false);
        }
        else
        {
            SelectButton.SetActive(false);
            CancelButton.SetActive(true);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.transform == SelectButton.transform) //����ѡ��ť
                {
                    if (SelectedPokerList.Contains(PokerObject)) return;    //��ѡ��Ŀ��޷���ѡ��ť

                    //���ڳ��ƽ׶Σ�  �˴����ѡ����ſ�ʱ�Ƿ���Ϲ���
                    if (playCardScript.isPlayCardPhase && SelectedPokerList.Count == 1)    //����ѡ��ťǰ��ѡ��һ�ţ�˵��Ҫѡ�ڶ�����
                    {
                        if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != 1)   //��һ��δѡ����￨
                        {       //�� δѡ��ͬ���ֿ���������
                            if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != PokerObject.GetComponent<Poker>().cardNumber)
                            {
                                if (PokerObject.GetComponent<Poker>().cardNumber != 1)   //���ֲ�ͬ �� �ڶ���ѡ��Ҳ���ǳ��￨
                                {
                                    TipsBoard.Instance.ShowTipsBoard("����ͨ�����￨ ���� ͬ���ֵĿ���������");
                                    return;
                                }
                                //else;   //���ֲ�ͬ �� �ڶ���ѡ���￨ �����
                            }
                            else if (PokerObject.GetComponent<Poker>().cardNumber * 2 > 10) // ͬ���ֿ����е�������10 ����
                            {
                                TipsBoard.Instance.ShowTipsBoard("ͬ���ֵĿ���������ʱ����֮�Ͳ��ܴ���10");
                                return;
                            }
                            //else;   //��һ��δѡ����￨ ����������ͬ�������� ����ִ��ѡ�����
                        }
                        //else;// ��һ�ž��ǳ��￨���ڶ���ѡʲô����
                    }
                    else if (playCardScript.isPlayCardPhase && SelectedPokerList.Count > 1)    //�Ѿ�ѡ�������ţ��������ѡ������
                    {
                        bool flag = false;
                        for (int i = SelectedPokerList.Count - 1; i >= 0; i--)
                        {
                            if (SelectedPokerList[i].GetComponent<Poker>().cardNumber == 1) //ȷ����ѡ��������д�
                                flag = true;
                        }
                        if (flag)   //��������
                        {
                            TipsBoard.Instance.ShowTipsBoard("ʹ�ó��￨��������ʱֻ�ܳ�������");
                            return;
                        }
                        else  //ͬ��������
                        {
                            if (PokerObject.GetComponent<Poker>().cardNumber != SelectedPokerList[0].GetComponent<Poker>().cardNumber)
                            {
                                TipsBoard.Instance.ShowTipsBoard("����ͨ�����￨ ���� ͬ���ֵĿ���������");
                                return;
                            }
                            else if (PokerObject.GetComponent<Poker>().cardNumber * (SelectedPokerList.Count + 1) > 10)
                            {
                                TipsBoard.Instance.ShowTipsBoard("ͬ���ֵĿ���������ʱ����֮�Ͳ��ܴ���10");
                                return;
                            }
                        }
                    }
                    //������

                    SelectedPokerList.Add(PokerObject); //��ѡ��Ŀ������б�
                    PokerObject.GetComponent<ClickPoker>().isSelected = true;
                    PokerObject = null;                 //��ǰ���ڵĿ��ÿ�
                    this.gameObject.SetActive(false);
                    OptionalBoxNum++;
                }
                else if (hit.transform == CancelButton.transform)   //����ȡ����ť
                {
                    if (SelectedPokerList.Contains(PokerObject)) SelectedPokerList.Remove(PokerObject);

                    PokerObject.GetComponent<ClickPoker>().isSelected = false;
                    PokerObject.GetComponent<ClickPoker>().SwitchPressed(); //�л���ǰ����״̬��ʹ֮δ�����
                    PokerObject = null;                 //��ǰ���ڵĿ��ÿ�
                    this.gameObject.SetActive(false);
                    OptionalBoxNum++;
                }
            }
        }
    }
    public int GetOptionalBoxNum()  //��ȡ���
    {
        return OptionalBoxNum;
    }
    public void SetOptionalBoxOn(GameObject go)  //�л�ѡ����Ӧ�Ŀ���
    {
        OptionalBoxNum++;
        PokerObject = go;
        if (PokerObject.GetComponent<ClickPoker>().isSelected) buttonCondition = false;
        else buttonCondition = true;
    }
    
    public void CancelPoker(GameObject poker)   //ģ��Ϊpoker����ȡ����ť 
    {
        if (SelectedPokerList.Contains(poker)) SelectedPokerList.Remove(poker);

        //poker.GetComponent<ClickPoker>().isSelected = false;
        poker.GetComponent<ClickPoker>().SwitchPressed();     //�л���ǰ����״̬��ʹ֮δ�����
        PokerObject = null;                 //��ǰ���ڵĿ��ÿ�
        this.gameObject.SetActive(false);
        OptionalBoxNum++;
    }
    public void CancelAll()
    {
        for(int i= SelectedPokerList.Count-1; i>=0;i--)
        {
            CancelPoker(SelectedPokerList[i]);
        }
    }
}
