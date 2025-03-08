using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiantBox : Item
{
    private GameObject GameCenter;
    private void Awake()
    {
        isConsumerable = false;  //光环效果
        //effectivePhase = new int[] { };    // 略
        itemName = "大匣子";
        itemDescr = "你的手牌上限+1";
        itemStory = "\"比用手拿牌方便很多\"";
        price = 36;     PriceFloating();
        treasureIndex = 0;  //仅限珍宝，用于更改珍宝仓库对应的计数器

        Scene gameScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in gameScene.GetRootGameObjects())
        {
            if (rootObject.tag == "GameControlCenter") GameCenter = rootObject;
        }
    }
    public override void ActivateFunction() //添加到道具栏时调用
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(1); //上限加一
        yield return null;
    }
    public override void DisabledFunction()  //重置上限
    {
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(-1);
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
