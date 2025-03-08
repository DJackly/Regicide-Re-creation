using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDeck : MonoBehaviour   //在游戏开始时根据SO生成所有卡牌的实体
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
        if (cardCount != transform.childCount)  //子物体数量与之前不同
        {
            cardCount = transform.childCount;
            CardDeckText.GetComponent<TextMesh>().text = "牌堆：剩余" + cardCount;
        }
    }
    private void GenerateCardDeck()
    {    
        var allCardSO = Resources.LoadAll("PokerSO", typeof(PokerSO));
        float deltaZ = 0f;
        foreach (var cardSO in allCardSO)
        {
            PokerSO card = cardSO as PokerSO;
            GameObject poker = GameObject.Instantiate(cardPrefab,transform);  //创建的卡
            Poker p = poker.GetComponent<Poker>();  //创建的卡上的poker脚本

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
            else   //Boss卡则移到别处
            {
                poker.transform.SetParent(BossObject.transform);
                poker.transform.position = BossObject.transform.position;
                BossObject.GetComponentInChildren<Boss>().BossList.Add(poker);
                poker.transform.position += new Vector3(0, 0, -deltaZ);
                deltaZ += 0.05f;
            }
        }
    }
    public void AddToList(GameObject poker) //短时间内调用多次后，调用一次sortCard()
    {
        poker.transform.parent = transform;
        poker.transform.GetChild(0).gameObject.SetActive(false);
        pokerList.Add(poker);
        CardDeckChange();
        //SortCard();
    }
    public GameObject ReturnFirstPoker()    //顶上为第一张
    {
        if (pokerList.Count == 0) return null;
        GameObject poker = pokerList[0];
        pokerList.RemoveAt(0);
        CardDeckChange();
        return poker;
    }
    public void SortCard(bool needShuffle = false)  //新增卡牌后请调用一次，但别洗牌
    {
        if(needShuffle)Shuffle();     //是否需要洗牌
        CardDeckChange();
        float deltaZ = 0f;
        for (int i = 0; i < transform.childCount; i++)    //index=0为牌堆顶，第一张牌开始Z轴递增
        {
            GameObject poker = pokerList[i];
            poker.transform.position = transform.position + new Vector3(0, 0, deltaZ);
            deltaZ += 0.05f;
        }
    }
    public void Shuffle() //打乱List
    {
        System.Random rad = new();
        int n = pokerList.Count;
        while (n > 1)
        {
            // 随机选择当前索引或之后的元素进行交换
            int k = rad.Next(n--); // 生成 [0, n) 范围内的随机数
            GameObject temp = pokerList[n];
            pokerList[n] = pokerList[k];
            pokerList[k] = temp;
        }
    }
    public void DiamondMagic()  //此函数是为了保证无方片时必抽到方片而存在（在抽倒数第二张时换最后一张）
    {
        //将牌堆最靠前的一张方片与顶上第二的牌互换位置
        int i;
        for(i = 0; i < pokerList.Count; i++)
        {
            if (pokerList[i].GetComponent<Poker>()?.suit == cardSuit.Diamonds) break;    //找到第一张方片
        }
        GameObject temp = pokerList[1]; //顶上第二的牌
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
        if (pokerList.Count <= 15 && GameObject.Find("DiamondNecklace(Clone)") != null)   //方片项链
        {
            StatementManager.Instance.SetStatement(0, true);
        }
        else StatementManager.Instance.SetStatement(0, false);
        if(pokerList.Count >= 28 && GameObject.Find("HeartNecklace(Clone)") != null)  //红桃项链
        {
            StatementManager.Instance.SetStatement(1, true);
        }
        else StatementManager.Instance.SetStatement(1, false);
    }
}
