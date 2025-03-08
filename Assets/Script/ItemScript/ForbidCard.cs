using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbidCard : Item
{
    private void Awake()
    {
        isConsumerable = true;  //一次性消耗品
        effectivePhase = new int[] { 1 };    // 仅在出牌阶段可使用
        itemName = "禁止牌";
        itemDescr = "消耗品。在出牌阶段使用，使得Boss对自身花色的免疫效果失效";
        itemStory = "这不是我们UNO里的牌吗？";
        price = 17; PriceFloating();

    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  //使用时肯定在游戏场景
    {
        Boss bossScipt = GameObject.Find("Boss").GetComponent<Boss>();
        if (bossScipt == null) Debug.LogWarning("未找到Boss物体");
        bossScipt.ForbidSuit();

        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject); 
    }
    public override void DisabledFunction()
    {

    }
    public override void TriggerDrawOutFunction()
    {

    }
}
