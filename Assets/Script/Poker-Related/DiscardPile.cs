using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public List<GameObject> pokerList;
    public GameObject DiscardPileText;
    public GameObject CardDeck;
    public int cardCount = 0;
    private int restoreCardBuff = 0;   //用于在buff的所用下影响恢复卡牌的数量
    private void Awake()
    {
        pokerList = new List<GameObject>();
    }
    private void Update()
    {
        if (cardCount != transform.childCount)  //子物体数量与之前不同
        {
            cardCount = transform.childCount;
            DiscardPileText.GetComponent<TextMesh>().text = "弃牌堆：累计" + cardCount;
        }
    }
    public void AddToList(GameObject poker)
    {
        pokerList.Add(poker);
        //SortCard();
    }
    public GameObject ReturnLastPoker()
    {
        if (pokerList.Count == 0) return null;
        GameObject poker = pokerList[0];
        pokerList.RemoveAt(0);
        return poker;
    }
    public void SortCard()  //新增卡牌后请调用一次
    {
        float deltaZ = 0f;
        for (int i = 0; i < transform.childCount; i++)    //index=0为牌堆底部，第一张牌开始Z轴递减
        {
            GameObject poker = pokerList[i];
            poker.transform.position = transform.position + new Vector3(0, 0, deltaZ);
            deltaZ -= 0.05f;
        }
    }
    public void StartRestoreCard(int num)
    {
        StartCoroutine(RestoreCards(num + restoreCardBuff));
    }
    IEnumerator RestoreCards(int num)   //动画版恢复卡牌
    {
        for (int i = 0; i < num; i++)
        {
            GameObject poker = this.GetComponent<DiscardPile>().ReturnLastPoker();
            if(poker == null) continue;
            Vector3 start = poker.transform.position;
            Vector3 dest = CardDeck.transform.position;

            // 移动卡牌到目标位置
            float duration = 0.15f; // 假设动画持续时间为 0.5 秒
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
            //poker.transform.SetParent(CardDeck.transform);
            //poker.transform.GetChild(0).gameObject.SetActive(false);
            CardDeck.GetComponent<CardDeck>().AddToList(poker);

            // 等待指定的时间
            yield return new WaitForSeconds(duration);
        }
        yield return new WaitForSeconds(0.2f);
        CardDeck.GetComponent<CardDeck>().SortCard();
    }
    public void ChangeRestoreCardBuff(int delta)
    {
        restoreCardBuff += delta;
    }
}
