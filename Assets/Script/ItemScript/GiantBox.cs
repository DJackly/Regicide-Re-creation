using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiantBox : Item
{
    private GameObject GameCenter;
    private void Awake()
    {
        isConsumerable = false;  //�⻷Ч��
        //effectivePhase = new int[] { };    // ��
        itemName = "��ϻ��";
        itemDescr = "�����������+1";
        itemStory = "\"���������Ʒ���ܶ�\"";
        price = 36;     PriceFloating();
        treasureIndex = 0;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����

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
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(1); //���޼�һ
        yield return null;
    }
    public override void DisabledFunction()  //��������
    {
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(-1);
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
