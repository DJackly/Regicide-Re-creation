using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AverageDice : Item
{
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "ƽ������";
        itemDescr = "����ĵ���<=6�Ŀ����ڽ���ʱ����+1";
        itemStory = "��������ƺ�ֻ��3���4�㣬���ž���ƽ���ɣ�";
        price = 22; PriceFloating();
        treasureIndex = 6;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
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
