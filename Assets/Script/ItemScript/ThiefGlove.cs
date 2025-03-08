using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThiefGlove : Item
{
    private GameObject GameCenter;
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч����Ʒ
        //effectivePhase = new int[] { 1, 4 };    // ��
        itemName = "С͵����";
        itemDescr = "������ƬЧ������ʱ���ɶ��������(��������������);��������Ч���ָ�����ʱ���ɶ�ָ�������";
        itemStory = "��˵̦˿�������������ھƹ���ʹ�ø�����͵���˻ƽ��ںϾ޹֡�";
        price = 21; PriceFloating();
        treasureIndex = 1;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����

        Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in gameScene.GetRootGameObjects())
        {
            if (rootObject.tag == "GameControlCenter") GameCenter = rootObject;
        }
    }

    public override void ActivateFunction() //��ӵ�������ʱ����
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().ChangeRestoreCardBuff(2);  //�ָ�����+2��
        GameCenter.GetComponent<DrawCard>().ChangeDrawCardBuff(2);  //������+2��
        yield return null;
    }
    public override void DisabledFunction()
    {
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().ChangeRestoreCardBuff(-2);  //�ָ�����-2��
        GameCenter.GetComponent<DrawCard>().ChangeDrawCardBuff(-2);  //������-2��
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }

    public override void TriggerDrawOutFunction()
    {
        
    }
}
