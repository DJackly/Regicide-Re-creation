using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Ankh : Item
{
    private void Awake()
    {
        isConsumerable = false;  //光环效果物品
        //effectivePhase = new int[] { 1, 4 };    // 略
        itemName = "安可护符";
        itemDescr = "遇到Boss时有40%的几率使得该Boss花色无效";
        itemStory = "生命的脉动在金色的护符下涌流。";
        price = 36; PriceFloating();
        treasureIndex = 4;  //仅限珍宝，用于更改珍宝仓库对应的计数器
        MyEventSystem.Instance.NewBossInit += WhenBossInit;
    }
    public override void ActivateFunction() //添加到道具栏时调用一次
    {
        StartCoroutine(Function());
    }
    public void WhenBossInit()
    {
        //且每次Boss刷新 由Boss脚本调用
        StartCoroutine(Function());
    }
    IEnumerator Function()
    {
        int n = Random.Range(1, 11);
        if (n <= 4)//40%的概率发动
        {
            Boss bossScipt = GameObject.Find("Boss").GetComponent<Boss>();
            if (bossScipt == null) UnityEngine.Debug.LogWarning("未找到Boss物体");
            bossScipt.ForbidSuit();

        }
        yield return null;
    }
    public override void DisabledFunction()  //重置功能
    {
        MyEventSystem.Instance.NewBossInit -= WhenBossInit;
    }
    public override void TriggerDrawOutFunction()
    {

    }
}
