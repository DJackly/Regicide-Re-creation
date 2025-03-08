using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class BombCard : Item
{
    public Boss bossScript;
    private void Awake()
    {
        isConsumerable = true;  //һ��������Ʒ
        effectivePhase = new int[] { 1 };    // ���ڳ��ƽ׶ο�ʹ��
        itemName = "ը����";
        itemDescr = "����Ʒ���ڳ��ƽ׶�ʹ�ã������Ƽ����ƶ����λ�ã��鵽ʱ�Զ�ʹ�ã���ɱ����Boss";
        itemStory = "\"����A�㲻��ը����\"";
        price = 33; PriceFloating();

        
    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  //�����ƶ���
    {
        CardDeck.Instance.AddItemCardRandom(this.gameObject);
        CardDeck.Instance.SortCard();
        this.GetComponent<Outline>().enabled = false;
        yield return new WaitForSeconds(1.7f);
    }
    public override void TriggerDrawOutFunction()
    {
        StartCoroutine(BeenDrawed());
    }
    IEnumerator BeenDrawed()    //�鵽ʱ����
    {
        bossScript = GameObject.Find("Boss").GetComponent<Boss>();
        bossScript.SetHp(-1);
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<Animator>().SetBool("startAnima", true);  //��ʼ���Ŷ���
        yield return new WaitForSeconds(2f);
        if(MainThread.Instance.currentStage<=3)MainThread.Instance.JumpToPhase(5);
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()
    {

    }
}
