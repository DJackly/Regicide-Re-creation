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
        isConsumerable = true;  //一次性消耗品
        effectivePhase = new int[] { 1, 4 };    // 仅在出牌,弃牌阶段可使用
        itemName = "小丑牌";
        itemDescr = "消耗品。在出牌阶段、弃牌阶段使用，丢弃所有手牌并抽取手牌直到上限。";
        itemStory = "这里的小丑不分大小王";
        price = 20; PriceFloating();

        Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in gameScene.GetRootGameObjects())
        {
            if (rootObject.name == "Grid") 
            { 
                for(int i=0;i<rootObject.transform.childCount;i++)  //获取其子对象
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
        OptionalBoxScript.CancelAll();  //取消对手牌的选择，防止出错

        List<GameObject> cardList = new List<GameObject>();
        for (int i = 0; i < hand.transform.childCount; i++)
        {
            cardList.Add(hand.transform.GetChild(i).gameObject);
        }
        discardPile.GetComponent<DropCard>().StartDropCards(cardList);  //线程A
        yield return new WaitForSeconds(1.7f);
        DrawCardScript.StartDrawCards(DrawCardScript.MaxCard);  //线程B
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
