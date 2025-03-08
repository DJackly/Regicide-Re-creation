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
        isConsumerable = true;  //一次性消耗品
        effectivePhase = new int[] { 1, 4 };    // 仅在出牌,弃牌阶段可使用
        itemName = "+4牌";
        itemDescr = "消耗品。在出牌阶段、弃牌阶段使用，抽取四张卡牌（不超过上限）";
        itemStory = "这是对自己使用的+4卡牌。";
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
        OptionalBoxScript.CancelAll();  //取消对手牌的选择，防止出错
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
