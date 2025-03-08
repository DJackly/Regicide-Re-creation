using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartNecklace : Item
{
    private CardDeck CardDeckScript;
    public readonly float ReduceRate = 0.3f;    //减伤比例
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "红桃项链";
        itemDescr = "红桃卡牌的点数+1；牌堆剩余的牌大于或等于28时，Boss的实时攻击力降低30%。";//更改条件时请在CardDeck脚本的CardDeckChange()中同步更改条件
        itemStory = "红桃牌变得更强了一些...";
        price = 35; PriceFloating();
        treasureIndex = 3;  //仅限珍宝，用于更改珍宝仓库对应的计数器
    }
    public bool CheckIfTrue()
    {
        CardDeckScript = GameObject.Find("Grid/CardDeck").GetComponent<CardDeck>();
        if (CardDeckScript.pokerList.Count >=28) return true;
        else return false;
    }
    public override void ActivateFunction() //添加到道具栏时调用
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        //此珍宝的作用，依靠[playcard脚本]检测其是否存在 然后决定是否起作用
        GameObject.Find("Grid/Boss").GetComponent<Boss>().UpdateBossInfo(); //购买后立刻对boss的攻击力进行更新
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
