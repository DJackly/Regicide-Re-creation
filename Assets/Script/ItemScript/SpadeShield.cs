using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpadeShield : Item
{
    public int shield;
    public int spadeCount;
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "���Ҷ���";
        itemDescr = "ÿ���2�ź����ƣ���������Ϊ�˶�������һ�㻤��ֵ������Boss����ʱ�ֵ���Ӧ�������˺�";
        itemStory = "���Խ�࣬��Ķ��ƾ�Խ��";
        price = 25; PriceFloating();
        treasureIndex = 8;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����

        shield = 0;
        spadeCount = 0;
    }
    public override void ActivateFunction() //��ӵ�������ʱ����һ��
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        StatementManager.Instance.SetStatement(2, true);
        StatementManager.Instance.SetStatementCount(2, shield);
        yield return null;
    }
    public override void DisabledFunction()  //���ù���
    {
        StatementManager.Instance.SetStatement(2, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
    public int GetShield()
    {
        return shield;
    }
    public void SpadePlus()
    {
        spadeCount++;
        if(spadeCount >= 2)
        {
            spadeCount = 0;
            shield++;
        }
        StatementManager.Instance.SetStatementCount(2, shield);
    }
}
