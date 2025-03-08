using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Plus4Card : Item
{
    public DrawCard DrawCardScript;
    public OptionalBox OptionalBoxScript;
    private void Awake()
    {
        isConsumerable = true;  //һ��������Ʒ
        effectivePhase = new int[] { 1, 4 };    // ���ڳ���,���ƽ׶ο�ʹ��
        itemName = "+4��";
        itemDescr = "����Ʒ���ڳ��ƽ׶Ρ����ƽ׶�ʹ�ã���ȡ���ſ��ƣ����������ޣ�";
        itemStory = "���Ƕ��Լ�ʹ�õ�+4���ơ�";
        price = 13; PriceFloating();

        Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in gameScene.GetRootGameObjects())
        {
            if (rootObject.tag == "GameControlCenter") DrawCardScript = rootObject.GetComponent<DrawCard>();
            if (rootObject.name == "OptionalBox") OptionalBoxScript = rootObject.GetComponent<OptionalBox>();
        }

    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()
    {
        OptionalBoxScript.CancelAll();  //ȡ�������Ƶ�ѡ�񣬷�ֹ����
        DrawCardScript.StartDrawCards(4);  
        yield return new WaitForSeconds(1.7f);
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()
    {

    }
    public override void TriggerDrawOutFunction()
    {

    }

}
