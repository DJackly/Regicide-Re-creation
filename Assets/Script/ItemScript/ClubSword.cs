using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ClubSword : Item
{
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "÷������";
        itemDescr = "���÷���Ʋ��ᱻBoss���ߣ�����һ�������>=17���˺�ʱ��Boss�ڱ��غ����޷�������";
        itemStory = "\"����������õķ��ء�\"";
        price = 46; PriceFloating();
        treasureIndex = 7;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
    }
    public override void ActivateFunction() //��ӵ�������ʱ����һ��
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {

        yield return null;
    }
    public override void DisabledFunction()  //���ù���
    {
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
