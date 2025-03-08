using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CopyCard : Item
{
    public GameObject hand;
    private void Awake()
    {
        isConsumerable = true;  //һ��������Ʒ
        effectivePhase = new int[] { 1 };    // ���ڳ���,�׶ο�ʹ��
        itemName = "������";
        itemDescr = "����Ʒ���ڳ��ƽ׶�ʹ�ã��������һ�����Ƽ����ƶѵײ�";
        itemStory = "�о�Ӧ�û�����Ч��";
        price = 9; PriceFloating();

    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  
    {
        hand = GameObject.Find("Grid/Hand");
        int n =Random.Range(0, hand.transform.childCount);  //���ѡһ������
        GameObject p = hand.transform.GetChild(n).gameObject;   //ѡ��������
        GameObject poker = Instantiate(p,p.transform.position,Quaternion.identity,p.transform.parent);   //���Ƹ�����

        Vector3 start = poker.transform.position;
        Vector3 dest = CardDeck.Instance.gameObject.transform.position; //Ŀ�ĵ�Ϊ�ƶ�
        float duration = 0.4f; // ��������ʱ��Ϊ
        float elapsed = 0f;
        SEManager.Instance.MovingCard();
        while (elapsed < duration)
        {
            poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);
        CardDeck.Instance.AddItemCardRandom(poker,false);
        CardDeck.Instance.SortCard();
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()
    {

    }
    public override void TriggerDrawOutFunction()
    {

    }
}
