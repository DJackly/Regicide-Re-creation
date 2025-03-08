using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayCard : MonoBehaviour
{
    public OptionalBox opBox;
    public GameObject PlayCardButton;
    public GameObject PlayArea;
    public GameObject Hand;
    public DropCard dropCardScript;
    public Boss bossScript;
    public bool isPlayCardPhase = false;    //���Գ���
    public bool isDropCardPhase = false;    //��Ҫ����
    public bool hasSelectedItem = false;

    public bool hasDiamondNecklace = false;
    public bool hasHeartNecklace = false;
    public bool hasAverageDice = false;
    void Update()
    {
        if(opBox.SelectedPokerList.Count == 0)
        {
                hasSelectedItem = false;
        }
        if (isPlayCardPhase)   //��  ���ƽ׶�  
        {
            if (opBox.SelectedPokerList.Count > 0)//�� ѡ��������>0
            {
                hasSelectedItem = true;
                PlayCardButton.GetComponent<TextMesh>().color = Color.white;
            }
            else PlayCardButton.GetComponent<TextMesh>().color = Color.gray;
            if (Hand.transform.childCount == 0)  //���ƽ׶�����Ϊ0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "������";
                PlayCardButton.GetComponent<TextMesh>().color = Color.white;
                hasSelectedItem = true;
            }
            else PlayCardButton.GetComponent<TextMesh>().text = "����";
        }
        else if (isDropCardPhase)          //�� ��Ҫ����  
        {
            PlayCardButton.GetComponent<TextMesh>().color = Color.white;
            if (opBox.SelectedPokerList.Count > 0)//�� ѡ��������>0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "����";
            }
            else
            {
                PlayCardButton.GetComponent<TextMesh>().text = "������";
            }
            if (Hand.transform.childCount == 0)  //���ƽ׶�����Ϊ0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "������";
            }
        }
        else
        {
            PlayCardButton.GetComponent<TextMesh>().text = "...";
            PlayCardButton.GetComponent<TextMesh>().color = Color.gray;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //����
            if (touch.phase == TouchPhase.Ended && hasSelectedItem && isPlayCardPhase) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if(hit.transform == PlayCardButton.transform)  //������ư�ť
                {
                    SEManager.Instance.ClickButton();
                    isPlayCardPhase = false;    //���ƺ󲻿��ٳ���
                    if (Hand.transform.childCount == 0) //������
                    {
                        MainThread.Instance.FinishPhase(1); //�������ƽ׶�
                        MainThread.Instance.FinishPhase(2); //�������ܽ׶�
                        MainThread.Instance.FinishPhase(3); //���������׶�
                        return;
                    }
                    MainThread.Instance.FinishPhase(1); //�������ƽ׶�

                    List<GameObject> list = opBox.SelectedPokerList;   
                    for (int i = list.Count-1; i >=0; i--)
                    {   
                        GameObject go = list[i];
                        go.transform.SetParent(PlayArea.transform);
                        opBox.CancelPoker(go);
                        PlayArea.GetComponent<HandManager>().SortYourHand();
                    }
                }
            }
            //����
            if (touch.phase == TouchPhase.Ended && isDropCardPhase)    //��ѡ�����ƣ�����ҪhasSelectedItem
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.transform == PlayCardButton.transform)  //������ư�ť
                {
                    DiscardSumCount.Instance.Unactivate();
                    SEManager.Instance.ClickButton();
                    isDropCardPhase = false;    //���ƺ󲻿�������

                    int sum = 0;
                    int shield = 0;
                    if (GameObject.Find("SpadeShield(Clone)") != null)
                        shield = GameObject.Find("SpadeShield(Clone)").GetComponent<SpadeShield>().GetShield();
                    sum += shield;
                    hasDiamondNecklace = false;
                    hasHeartNecklace = false;
                    hasAverageDice = false;
                    if (GameObject.Find("HeartNecklace(Clone)") != null) hasHeartNecklace = true;
                    if (GameObject.Find("DiamondNecklace(Clone)") != null) hasDiamondNecklace = true;
                    if (GameObject.Find("AverageDice(Clone)") != null) hasAverageDice = true;

                    //�����ƽ���ͳ��
                    for (int i = 0; i < opBox.SelectedPokerList.Count; i++)
                    {
                        Poker pokerScript = opBox.SelectedPokerList[i].GetComponent<Poker>();
                        if (hasAverageDice == true && pokerScript.cardNumber <= 6) sum += (pokerScript.cardNumber + 1);
                        else sum += pokerScript.cardNumber;
                        if (pokerScript.suit == cardSuit.Hearts || pokerScript.suit == cardSuit.Diamonds)
                        {
                            if (pokerScript.suit == cardSuit.Hearts && hasHeartNecklace) sum += 1;
                            else if (pokerScript.suit == cardSuit.Diamonds && hasDiamondNecklace) sum += 1;
                        }
                    }

                    //�к�������
                    if (GameObject.Find("HeartNecklace(Clone)") != null && GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().CheckIfTrue())
                    {
                        if (sum < (bossScript.GetDamage() - (int)(bossScript.GetDamage() * GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().ReduceRate))) //Boss�˺���15%
                        {
                            if (GameObject.Find("ReviveRing(Clone)") != null)//���������ֵ�����
                                GameObject.Find("ReviveRing(Clone)").GetComponent<ReviveRing>().StartFunction();
                            else MainThread.Instance.Defeat();
                        }
                    }
                    else
                    {
                        if (sum < bossScript.GetDamage())
                        {
                            if (GameObject.Find("ReviveRing(Clone)") != null)//���������ֵ�����
                                GameObject.Find("ReviveRing(Clone)").GetComponent<ReviveRing>().StartFunction();
                            else MainThread.Instance.Defeat();
                        }
                    }
                    dropCardScript.StartDropCards(opBox.SelectedPokerList);
                    bossScript.AttackState();
                    MainThread.Instance.FinishPhase(4); //���������˺��׶ν׶�
                                                        //�б���ղ��������ƽű��ڽ���
                }
            }
        }
    }

    public int ShouldDiscard()  //���ظ����Ƶĵ�����
    {
        int sum = bossScript.GetDamage();
        int shield = 0; //���һ���

        if (GameObject.Find("HeartNecklace(Clone)") != null && GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().CheckIfTrue())
        {       //�к������������㼤����������Ҫ���Ƶ���������һ���ٷֱ�
            sum -= (int)(bossScript.GetDamage() * GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().ReduceRate);
        }
        if (GameObject.Find("SpadeShield(Clone)") != null)
            shield = GameObject.Find("SpadeShield(Clone)").GetComponent<SpadeShield>().GetShield();
        sum -= shield;  //Ҫ���Ƶĵ����ͼ�ȥ����ֵ
        if (sum < 0) sum = 0;
        return sum;
    }
}
