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
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "小偷手套";
        itemDescr = "触发方片效果抽牌时，可多抽两张牌(不超过手牌上限);触发红桃效果恢复卡牌时，可多恢复两张牌";
        itemStory = "据说苔丝·格雷迈恩曾在酒馆中使用该手套偷到了黄金融合巨怪。";
        price = 21; PriceFloating();
        treasureIndex = 1;  //仅限珍宝，用于更改珍宝仓库对应的计数器

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
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().ChangeRestoreCardBuff(2);  //恢复牌数+2；
        GameCenter.GetComponent<DrawCard>().ChangeDrawCardBuff(2);  //摸牌数+2；
        yield return null;
    }
    public override void DisabledFunction()
    {
        GameObject.Find("DiscardPile").GetComponent<DiscardPile>().ChangeRestoreCardBuff(-2);  //恢复牌数-2；
        GameCenter.GetComponent<DrawCard>().ChangeDrawCardBuff(-2);  //摸牌数-2；
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }

    public override void TriggerDrawOutFunction()
    {
        
    }
}
