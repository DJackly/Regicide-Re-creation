using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CopyCard : Item
{
    public GameObject hand;
    private void Awake()
    {
        isConsumerable = true;  //一次性消耗品
        effectivePhase = new int[] { 1 };    // 仅在出牌,阶段可使用
        itemName = "复制牌";
        itemDescr = "消耗品。在出牌阶段使用，随机复制一张手牌加入牌堆底部";
        itemStory = "感觉应该会有奇效。";
        price = 9; PriceFloating();

    }
    public override void ActivateFunction()
    {
        StartCoroutine(Function());
    }

    IEnumerator Function()  
    {
        hand = GameObject.Find("Grid/Hand");
        int n =Random.Range(0, hand.transform.childCount);  //随机选一张手牌
        GameObject p = hand.transform.GetChild(n).gameObject;   //选定的手牌
        GameObject poker = Instantiate(p,p.transform.position,Quaternion.identity,p.transform.parent);   //复制该手牌

        Vector3 start = poker.transform.position;
        Vector3 dest = CardDeck.Instance.gameObject.transform.position; //目的地为牌堆
        float duration = 0.4f; // 动画持续时间为
        float elapsed = 0f;
        SEManager.Instance.MovingCard();
        while (elapsed < duration)
        {
            poker.transform.position = Vector3.Lerp(start, dest, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);
        CardDeck.Instance.AddItemCardRandom(poker,false);
        CardDeck.Instance.SortCard();
        Destroy(this.gameObject);
    }
    public override void DisabledFunction()
    {

    }
    public override void TriggerDrawOutFunction()
    {

    }
}
