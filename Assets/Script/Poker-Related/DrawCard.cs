using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DrawCard : MonoBehaviour
{
    //������GameCenter
    //������Ϸ����ķ���������Ĳ������
    public GameObject CardDeck;
    public GameObject Hand;
    public Vector3 destin;
    public int MaxCard;     //��������
    public readonly int OriginMax = 8;
    private int drawCardBuff = 0;   //������buff��������Ӱ���������
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

    IEnumerator DrawCards(int num)   //���������
    {
        int e;  //�������޷�Ƭʱ���Ƶı���
        if (num >= (MaxCard - Hand.transform.childCount)) num = MaxCard - Hand.transform.childCount;  //����鳬������
        if (num >= 2) e = num - 2;  //�����ڶ��γ��Ƶ�iӦ�õ���e����Ϊi���һ����num-1
        else e = -1;    //�����ڵ����ڶ��γ��ƣ���ִ��DiamondMagic()
        for (int i = 0; i < num; i++)
        {
            //if (Hand.transform.childCount == MaxCard) yield break;  //������
            if (CardDeck.GetComponent<CardDeck>().pokerList.Count == 0) yield break;    //�ƿ��


            if (i == e)   //�����ڶ��γ���ʱ(��Ϊ������һ�ſɿ��������������һ�γ���ʱ͵������)
            {
                bool flag = false;  
                for(int x = 0; x < Hand.transform.childCount; x++)
                {
                    if(Hand.transform.GetChild(x).GetComponent<Poker>().suit == cardSuit.Diamonds)flag = true;
                }
                if(! flag)//��� ��⵽�����޷�Ƭ
                {
                    CardDeck.GetComponent<CardDeck>().DiamondMagic();
                    yield return new WaitForSeconds(0.2f);
                }
            }

            GameObject poker = CardDeck.GetComponent<CardDeck>().ReturnFirstPoker();
            if (poker.GetComponent<Item>()) //������item���ƽ��д���
            {
                SpecialCard(poker);
                continue;
            }

            Vector3 start = poker.transform.position;
            Vector3 dest = destin;

            // �ƶ����Ƶ�Ŀ��λ��
            float duration = 0.2f; // ���趯������ʱ��Ϊ 0.5 ��
            float elapsed = 0f;
            SEManager.Instance.MovingCard();
            while (elapsed < duration)
            {
                poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // ȷ������λ������ΪĿ��λ��
            poker.transform.position = dest;
            poker.transform.SetParent(Hand.transform);
            poker.transform.GetChild(0).gameObject.SetActive(true);
            Hand.GetComponent<HandManager>().SortYourHand();

            // �ȴ�ָ����ʱ��
            yield return new WaitForSeconds(0.1f);
            
        }
        MyEventSystem.Instance.TriggerMoveCardEnd();
    }
    public void ChangeDrawCardBuff(int delta)
    {
        drawCardBuff += delta;
    }
    public void SpecialCard(GameObject itemCard)    //���ڳ鵽�������item���ƽ��д���
    {
        itemCard.GetComponent<Item>().TriggerDrawOutFunction();
    }
    public void ChangeMaxCard(int delta)    //���������޽����޸�
    {
        MaxCard += delta;
        StatementManager.Instance.SetStatementCount(3, MaxCard);
    }
}
