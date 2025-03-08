using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DrawCard : MonoBehaviour
{
    //挂载于GameCenter
    //负责游戏最初的发牌与后续的补充抽牌
    public GameObject CardDeck;
    public GameObject Hand;
    public Vector3 destin;
    public int MaxCard;     //手牌上限
    public readonly int OriginMax = 8;
    private int drawCardBuff = 0;   //用于在buff的所用下影响抽牌数量
    void Start()
    {
        destin = Hand.transform.position;
        MaxCard = OriginMax;
        StartDrawCards(MaxCard);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartDrawCards(3);
            }
        }
    }
    public void StartDrawCards(int i)
    {
        MyEventSystem.Instance.TriggerMoveCardStart();
        StartCoroutine(DrawCards(i + drawCardBuff));
    }

    IEnumerator DrawCards(int num)   //动画版抽牌
    {
        int e;  //服务于无方片时抽牌的变量
        if (num >= (MaxCard - Hand.transform.childCount)) num = MaxCard - Hand.transform.childCount;  //不会抽超过上限
        if (num >= 2) e = num - 2;  //倒数第二次抽牌的i应该等于e，因为i最后一次是num-1
        else e = -1;    //不存在倒数第二次抽牌，则不执行DiamondMagic()
        for (int i = 0; i < num; i++)
        {
            //if (Hand.transform.childCount == MaxCard) yield break;  //手牌满
            if (CardDeck.GetComponent<CardDeck>().pokerList.Count == 0) yield break;    //牌库空


            if (i == e)   //倒数第二次抽牌时(因为最上面一张可看见，不方便最后一次抽牌时偷换卡牌)
            {
                bool flag = false;  
                for(int x = 0; x < Hand.transform.childCount; x++)
                {
                    if(Hand.transform.GetChild(x).GetComponent<Poker>().suit == cardSuit.Diamonds)flag = true;
                }
                if(! flag)//如果 检测到手牌无方片
                {
                    CardDeck.GetComponent<CardDeck>().DiamondMagic();
                    yield return new WaitForSeconds(0.2f);
                }
            }

            GameObject poker = CardDeck.GetComponent<CardDeck>().ReturnFirstPoker();
            if (poker.GetComponent<Item>()) //对特殊item卡牌进行处理
            {
                SpecialCard(poker);
                continue;
            }

            Vector3 start = poker.transform.position;
            Vector3 dest = destin;

            // 移动卡牌到目标位置
            float duration = 0.2f; // 假设动画持续时间为 0.5 秒
            float elapsed = 0f;
            SEManager.Instance.MovingCard();
            while (elapsed < duration)
            {
                poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 确保卡牌位置设置为目标位置
            poker.transform.position = dest;
            poker.transform.SetParent(Hand.transform);
            poker.transform.GetChild(0).gameObject.SetActive(true);
            Hand.GetComponent<HandManager>().SortYourHand();

            // 等待指定的时间
            yield return new WaitForSeconds(0.1f);
            
        }
        MyEventSystem.Instance.TriggerMoveCardEnd();
    }
    public void ChangeDrawCardBuff(int delta)
    {
        drawCardBuff += delta;
    }
    public void SpecialCard(GameObject itemCard)    //对于抽到的特殊的item卡牌进行处理
    {
        itemCard.GetComponent<Item>().TriggerDrawOutFunction();
    }
    public void ChangeMaxCard(int delta)    //对手牌上限进行修改
    {
        MaxCard += delta;
        StatementManager.Instance.SetStatementCount(3, MaxCard);
    }
}
