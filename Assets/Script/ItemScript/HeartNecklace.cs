using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartNecklace : Item
{
    private CardDeck CardDeckScript;
    public readonly float ReduceRate = 0.3f;    //���˱���
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "��������";
        itemDescr = "���ҿ��Ƶĵ���+1���ƶ�ʣ����ƴ��ڻ����28ʱ��Boss��ʵʱ����������30%��";//��������ʱ����CardDeck�ű���CardDeckChange()��ͬ����������
        itemStory = "�����Ʊ�ø�ǿ��һЩ...";
        price = 35; PriceFloating();
        treasureIndex = 3;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
    }
    public bool CheckIfTrue()
    {
        CardDeckScript = GameObject.Find("Grid/CardDeck").GetComponent<CardDeck>();
        if (CardDeckScript.pokerList.Count >=28) return true;
        else return false;
    }
    public override void ActivateFunction() //��ӵ�������ʱ����
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        //���䱦�����ã�����[playcard�ű�]������Ƿ���� Ȼ������Ƿ�������
        GameObject.Find("Grid/Boss").GetComponent<Boss>().UpdateBossInfo(); //��������̶�boss�Ĺ��������и���
        CardDeck.Instance.CardDeckChange(); //�ֶ�ˢ��״̬�������Ƿ�����������������Ȼ����ʾ״̬ͼ��
        yield return null;
    }
    public override void DisabledFunction()
    {
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
