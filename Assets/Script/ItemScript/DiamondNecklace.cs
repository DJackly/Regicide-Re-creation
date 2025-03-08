using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiamondNecklace : Item
{
    private CardDeck CardDeckScript;
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "��Ƭ����";
        itemDescr = "��Ƭ���Ƶĵ���+1���ƶ�ʣ�����С�ڻ����15ʱ�����ƽ���������˺�+5��";//��������ʱ����CardDeck�ű���CardDeckChange()��ͬ����������
        itemStory = "��е����ϵķ�Ƭ�Ʒ·���ħ��";
        price = 32; PriceFloating();
        treasureIndex = 2;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
    }
    public int ActivateFunction2()
    {
        CardDeckScript = GameObject.Find("Grid/CardDeck").GetComponent<CardDeck>();
        if (CardDeckScript.pokerList.Count <= 15) return 5;
        else return 0;
    }
    public override void ActivateFunction() //��ӵ�������ʱ����
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        //���䱦�����ã��������ܼ���ű�������Ƿ���� Ȼ������Ƿ�������
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
