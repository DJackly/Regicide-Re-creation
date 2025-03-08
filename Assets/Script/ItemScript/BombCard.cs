using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class BombCard : Item
{
    public Boss bossScript;
    private void Awake()
    {
        isConsumerable = true;  //一次性消耗品
        effectivePhase = new int[] { 1 };    // 仅在出牌阶段可使用
        itemName = "炸弹牌";
        itemDescr = "消耗品。在出牌阶段使用，将此牌加入牌堆随机位置，抽到时自动使用，击杀场上Boss";
        itemStory = "\"四张A算不算炸弹？\"";
        price = 33; PriceFloating();

        
    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  //加入牌堆中
    {
        CardDeck.Instance.AddItemCardRandom(this.gameObject);
        CardDeck.Instance.SortCard();
        this.GetComponent<Outline>().enabled = false;
        yield return new WaitForSeconds(1.7f);
    }
    public override void TriggerDrawOutFunction()
    {
        StartCoroutine(BeenDrawed());
    }
    IEnumerator BeenDrawed()    //抽到时触发
    {
        bossScript = GameObject.Find("Boss").GetComponent<Boss>();
        bossScript.SetHp(-1);
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<Animator>().SetBool("startAnima", true);  //开始播放动画
        yield return new WaitForSeconds(2f);
        if(MainThread.Instance.currentStage<=3)MainThread.Instance.JumpToPhase(5);
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()
    {

    }
}
