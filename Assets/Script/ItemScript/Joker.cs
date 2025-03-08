using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Joker : Item
{
    public GameObject hand;
    public DrawCard DrawCardScript;
    public GameObject discardPile;
    public OptionalBox OptionalBoxScript;
    private void Awake()
    {
        isConsumerable = true;  //һ��������Ʒ
        effectivePhase = new int[] { 1, 4 };    // ���ڳ���,���ƽ׶ο�ʹ��
        itemName = "С����";
        itemDescr = "����Ʒ���ڳ��ƽ׶Ρ����ƽ׶�ʹ�ã������������Ʋ���ȡ����ֱ�����ޡ�";
        itemStory = "�����С�󲻷ִ�С��";
        price = 20; PriceFloating();

        Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in gameScene.GetRootGameObjects())
        {
            if (rootObject.name == "Grid") 
            { 
                for(int i=0;i<rootObject.transform.childCount;i++)  //��ȡ���Ӷ���
                {
                    GameObject obj = rootObject.transform.GetChild(i).gameObject;
                    if (obj.tag == "Hand") hand = obj;
                    if (obj.name == "DiscardPile") discardPile = obj;
                }
            }   
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

        List<GameObject> cardList = new List<GameObject>();
        for (int i = 0; i < hand.transform.childCount; i++)
        {
            cardList.Add(hand.transform.GetChild(i).gameObject);
        }
        discardPile.GetComponent<DropCard>().StartDropCards(cardList);  //�߳�A
        yield return new WaitForSeconds(1.7f);
        DrawCardScript.StartDrawCards(DrawCardScript.MaxCard);  //�߳�B
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
