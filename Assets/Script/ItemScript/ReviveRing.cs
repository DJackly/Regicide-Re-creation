using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveRing : Item
{
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "���ս�ָ";
        itemDescr = "�ֵ�һ�����������飬�������ƣ���ʹ����������-1";
        itemStory = "�������ڰ���³��������г����Ե��ģ���֪���ǲ�����ġ�";
        price = 33; PriceFloating();
        treasureIndex = 5;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
    }
    public override void ActivateFunction() //��ӵ�������ʱ����һ��
    {
        //�����ô˺���������ʱ����startFuntion
    }
    public void StartFunction() //����ʱ����
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        yield return new WaitForSeconds(0.6f);
        SEManager.Instance.Break();

        GameObject GameCenter = GameObject.FindWithTag("GameControlCenter");
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(-1); //���޼�һ
        GameCenter.GetComponent<DrawCard>().StartDrawCards(3);

        DisabledFunction();
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()  //���ù���
    {
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
