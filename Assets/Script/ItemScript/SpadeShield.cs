using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpadeShield : Item
{
    public int shield;
    public int spadeCount;
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "黑桃盾牌";
        itemDescr = "每打出2张黑桃牌，即可永久为此盾牌增加一点护甲值，可在Boss攻击时抵挡相应点数的伤害";
        itemStory = "打的越多，你的盾牌就越厚。";
        price = 25; PriceFloating();
        treasureIndex = 8;  //仅限珍宝，用于更改珍宝仓库对应的计数器

        shield = 0;
        spadeCount = 0;
    }
    public override void ActivateFunction() //添加到道具栏时调用一次
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        StatementManager.Instance.SetStatement(2, true);
        StatementManager.Instance.SetStatementCount(2, shield);
        yield return null;
    }
    public override void DisabledFunction()  //重置功能
    {
        StatementManager.Instance.SetStatement(2, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
    public int GetShield()
    {
        return shield;
    }
    public void SpadePlus()
    {
        spadeCount++;
        if(spadeCount >= 2)
        {
            spadeCount = 0;
            shield++;
        }
        StatementManager.Instance.SetStatementCount(2, shield);
    }
}
