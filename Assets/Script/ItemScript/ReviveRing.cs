using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveRing : Item
{
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "复苏戒指";
        itemDescr = "抵挡一次死亡后破碎，抽三张牌，并使得手牌上限-1";
        itemStory = "好像是在安德鲁镇的跳蚤市场上淘到的，不知道是不是真的。";
        price = 33; PriceFloating();
        treasureIndex = 5;  //仅限珍宝，用于更改珍宝仓库对应的计数器
    }
    public override void ActivateFunction() //添加到道具栏时调用一次
    {
        //不调用此函数吗，死亡时调用startFuntion
    }
    public void StartFunction() //死亡时调用
    {
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        yield return new WaitForSeconds(0.6f);
        SEManager.Instance.Break();

        GameObject GameCenter = GameObject.FindWithTag("GameControlCenter");
        GameCenter.GetComponent<DrawCard>().ChangeMaxCard(-1); //上限减一
        GameCenter.GetComponent<DrawCard>().StartDrawCards(3);

        DisabledFunction();
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()  //重置功能
    {
        ItemWareHouse.Instance.SoldTreasure(treasureIndex, false);
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
