using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiamondNecklace : Item
{
    private CardDeck CardDeckScript;
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "方片项链";
        itemDescr = "方片卡牌的点数+1；牌堆剩余的牌小于或等于15时，出牌结算的最终伤害+5。";//更改条件时请在CardDeck脚本的CardDeckChange()中同步更改条件
        itemStory = "你感到手上的方片牌仿佛附了魔。";
        price = 32; PriceFloating();
        treasureIndex = 2;  //仅限珍宝，用于更改珍宝仓库对应的计数器
    }
    public int ActivateFunction2()
    {
        CardDeckScript = GameObject.Find("Grid/CardDeck").GetComponent<CardDeck>();
        if (CardDeckScript.pokerList.Count <= 15) return 5;
        else return 0;
    }
    public override void ActivateFunction() //添加到道具栏时调用
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        //此珍宝的作用，依靠技能激活脚本检测其是否存在 然后决定是否起作用
        CardDeck.Instance.CardDeckChange(); //手动刷新状态让其检测是否满足项链的条件，然后显示状态图标
        yield return null;
    }
    public override void DisabledFunction()
    {
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
