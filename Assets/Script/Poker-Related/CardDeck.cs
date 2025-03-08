using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour   //����Ϸ��ʼʱ����SO�������п��Ƶ�ʵ��
{
    public static CardDeck Instance { get; private set; }

    public GameObject cardPrefab;
    public List<GameObject> pokerList;
    public GameObject CardDeckText;
    public GameObject BossObject;
    public int cardCount = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        pokerList = new List<GameObject>();
        GenerateCardDeck();
        SortCard(true);
    }
    private void Update()
    {
        if (cardCount != transform.childCount)  //������������֮ǰ��ͬ
        {
            cardCount = transform.childCount;
            CardDeckText.GetComponent<TextMesh>().text = "�ƶѣ�ʣ��" + cardCount;
        }
    }
    private void GenerateCardDeck()
    {    
        var allCardSO = Resources.LoadAll("PokerSO", typeof(PokerSO));
        float deltaZ = 0f;
        foreach (var cardSO in allCardSO)
        {
            PokerSO card = cardSO as PokerSO;
            GameObject poker = GameObject.Instantiate(cardPrefab,transform);  //�����Ŀ�
            Poker p = poker.GetComponent<Poker>();  //�����Ŀ��ϵ�poker�ű�

            p.cardNumber = card.cardNumber;
            p.picNo = card.picNo;
            p.cardNo = card.cardNo;
            p.suit = card.suit;
            p.isInHand = card.isInHand;
            p.pokerSO = card;
            p.setName();
            p.GetComponent<InstantiateCard>().InstantiatePoker();
            poker.name = card.suit.ToString()+card.cardNumber;
            poker.transform.GetChild(0).gameObject.SetActive(false);
            if (p.isInHand)
            {
                pokerList.Add(poker);
                poker.transform.position = transform.position + new Vector3(0, 0, -deltaZ);
                deltaZ += 0.05f;
            }
            else   //Boss�����Ƶ���
            {
                poker.transform.SetParent(BossObject.transform);
                poker.transform.position = BossObject.transform.position;
                BossObject.GetComponentInChildren<Boss>().BossList.Add(poker);
                poker.transform.position += new Vector3(0, 0, -deltaZ);
                deltaZ += 0.05f;
            }
        }
    }
    public void AddToList(GameObject poker) //��ʱ���ڵ��ö�κ󣬵���һ��sortCard()
    {
        poker.transform.parent = transform;
        poker.transform.GetChild(0).gameObject.SetActive(false);
        pokerList.Add(poker);
        CardDeckChange();
        //SortCard();
    }
    public GameObject ReturnFirstPoker()    //����Ϊ��һ��
    {
        if (pokerList.Count == 0) return null;
        GameObject poker = pokerList[0];
        pokerList.RemoveAt(0);
        CardDeckChange();
        return poker;
    }
    public void SortCard(bool needShuffle = false)  //�������ƺ������һ�Σ�����ϴ��
    {
        if(needShuffle)Shuffle();     //�Ƿ���Ҫϴ��
        CardDeckChange();
        float deltaZ = 0f;
        for (int i = 0; i < transform.childCount; i++)    //index=0Ϊ�ƶѶ�����һ���ƿ�ʼZ�����
        {
            GameObject poker = pokerList[i];
            poker.transform.position = transform.position + new Vector3(0, 0, deltaZ);
            deltaZ += 0.05f;
        }
    }
    public void Shuffle() //����List
    {
        System.Random rad = new();
        int n = pokerList.Count;
        while (n > 1)
        {
            // ���ѡ��ǰ������֮���Ԫ�ؽ��н���
            int k = rad.Next(n--); // ���� [0, n) ��Χ�ڵ������
            GameObject temp = pokerList[n];
            pokerList[n] = pokerList[k];
            pokerList[k] = temp;
        }
    }
    public void DiamondMagic()  //�˺�����Ϊ�˱�֤�޷�Ƭʱ�س鵽��Ƭ�����ڣ��ڳ鵹���ڶ���ʱ�����һ�ţ�
    {
        //���ƶ��ǰ��һ�ŷ�Ƭ�붥�ϵڶ����ƻ���λ��
        int i;
        for(i = 0; i < pokerList.Count; i++)
        {
            if (pokerList[i].GetComponent<Poker>()?.suit == cardSuit.Diamonds) break;    //�ҵ���һ�ŷ�Ƭ
        }
        GameObject temp = pokerList[1]; //���ϵڶ�����
        pokerList[1] = pokerList[i];
        pokerList[i] = temp;
        //SortCard();
        Vector3 tempPos = pokerList[1].transform.position;
        pokerList[1].transform.position = pokerList[i].transform.position;
        pokerList[i].transform.position = tempPos;
    }
    public void AddItemCardRandom(GameObject poker,bool changeScale=true)
    {
        poker.transform.parent = transform;
        poker.transform.position = transform.position;
        if(changeScale)poker.transform.localScale = new Vector3(0.25f, 0.26f, 1);
        int index = Random.Range(0, pokerList.Count);
        pokerList.Insert(index, poker);
        CardDeckChange();
    }
    public void CardDeckChange()
    {
        if (pokerList.Count <= 15 && GameObject.Find("DiamondNecklace(Clone)") != null)   //��Ƭ����
        {
            StatementManager.Instance.SetStatement(0, true);
        }
        else StatementManager.Instance.SetStatement(0, false);
        if(pokerList.Count >= 28 && GameObject.Find("HeartNecklace(Clone)") != null)  //��������
        {
            StatementManager.Instance.SetStatement(1, true);
        }
        else StatementManager.Instance.SetStatement(1, false);
    }
}
