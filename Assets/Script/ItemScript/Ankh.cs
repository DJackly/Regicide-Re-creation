using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Ankh : Item
{
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "���ɻ���";
        itemDescr = "����Bossʱ��40%�ļ���ʹ�ø�Boss��ɫ��Ч";
        itemStory = "�����������ڽ�ɫ�Ļ�����ӿ����";
        price = 36; PriceFloating();
        treasureIndex = 4;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
        MyEventSystem.Instance.NewBossInit += WhenBossInit;
    }
    public override void ActivateFunction() //��ӵ�������ʱ����һ��
    {
        StartCoroutine(Function());
    }
    public void WhenBossInit()
    {
        //��ÿ��Bossˢ�� ��Boss�ű�����
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        int n = Random.Range(1, 11);
        if (n <= 4)//40%�ĸ��ʷ���
        {
            Boss bossScipt = GameObject.Find("Boss").GetComponent<Boss>();
            if (bossScipt == null) UnityEngine.Debug.LogWarning("δ�ҵ�Boss����");
            bossScipt.ForbidSuit();

        }
        yield return null;
    }
    public override void DisabledFunction()  //���ù���
    {
        MyEventSystem.Instance.NewBossInit -= WhenBossInit;
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
