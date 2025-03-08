using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ClubSword : Item
{
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "梅花长剑";
        itemDescr = "你的梅花牌不会被Boss免疫；当你一次性造成>=17点伤害时，Boss在本回合内无法反击你";
        itemStory = "\"进攻就是最好的防守。\"";
        price = 46; PriceFloating();
        treasureIndex = 7;  //仅限珍宝，用于更改珍宝仓库对应的计数器
    }
    public override void ActivateFunction() //添加到道具栏时调用一次
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {

        yield return null;
    }
    public override void DisabledFunction()  //重置功能
    {
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
