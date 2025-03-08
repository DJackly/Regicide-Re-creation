using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbidCard : Item
{
    private void Awake()
    {
        isConsumerable = true;  //һ��������Ʒ
        effectivePhase = new int[] { 1 };    // ���ڳ��ƽ׶ο�ʹ��
        itemName = "��ֹ��";
        itemDescr = "����Ʒ���ڳ��ƽ׶�ʹ�ã�ʹ��Boss������ɫ������Ч��ʧЧ";
        itemStory = "�ⲻ������UNO�������";
        price = 17; PriceFloating();

    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  //ʹ��ʱ�϶�����Ϸ����
    {
        Boss bossScipt = GameObject.Find("Boss").GetComponent<Boss>();
        if (bossScipt == null) Debug.LogWarning("δ�ҵ�Boss����");
        bossScipt.ForbidSuit();

        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject); 
    }
    public override void DisabledFunction()
    {

    }
    public override void TriggerDrawOutFunction()
    {

    }
}
