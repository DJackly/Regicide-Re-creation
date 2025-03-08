using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AverageDice : Item
{
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "平均骰子";
        itemDescr = "打出的点数<=6的卡牌在结算时点数+1";
        itemStory = "这个骰子似乎只有3点和4点，这大概就是平均吧？";
        price = 22; PriceFloating();
        treasureIndex = 6;  //仅限珍宝，用于更改珍宝仓库对应的计数器
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
