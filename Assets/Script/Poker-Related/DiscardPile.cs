using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public List<GameObject> pokerList;
    public GameObject DiscardPileText;
    public GameObject CardDeck;
    public int cardCount = 0;
    private int restoreCardBuff = 0;   //������buff��������Ӱ��ָ����Ƶ�����
    private void Awake()
    {
        pokerList = new List<GameObject>();
    }
    private void Update()
    {
        if (cardCount != transform.childCount)  //������������֮ǰ��ͬ
        {
            cardCount = transform.childCount;
            DiscardPileText.GetComponent<TextMesh>().text = "���ƶѣ��ۼ�" + cardCount;
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
    public void SortCard()  //�������ƺ������һ��
    {
        float deltaZ = 0f;
        for (int i = 0; i < transform.childCount; i++)    //index=0Ϊ�ƶѵײ�����һ���ƿ�ʼZ��ݼ�
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
    IEnumerator RestoreCards(int num)   //������ָ�����
    {
        for (int i = 0; i < num; i++)
        {
            GameObject poker = this.GetComponent<DiscardPile>().ReturnLastPoker();
            if(poker == null) continue;
            Vector3 start = poker.transform.position;
            Vector3 dest = CardDeck.transform.position;

            // �ƶ����Ƶ�Ŀ��λ��
            float duration = 0.15f; // ���趯������ʱ��Ϊ 0.5 ��
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
            //poker.transform.SetParent(CardDeck.transform);
            //poker.transform.GetChild(0).gameObject.SetActive(false);
            CardDeck.GetComponent<CardDeck>().AddToList(poker);

            // �ȴ�ָ����ʱ��
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
