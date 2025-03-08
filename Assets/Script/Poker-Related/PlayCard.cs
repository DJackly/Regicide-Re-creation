using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayCard : MonoBehaviour
{
    public OptionalBox opBox;
    public GameObject PlayCardButton;
    public GameObject PlayArea;
    public GameObject Hand;
    public DropCard dropCardScript;
    public Boss bossScript;
    public bool isPlayCardPhase = false;    //可以出牌
    public bool isDropCardPhase = false;    //需要弃牌
    public bool hasSelectedItem = false;

    public bool hasDiamondNecklace = false;
    public bool hasHeartNecklace = false;
    public bool hasAverageDice = false;
    void Update()
    {
        if(opBox.SelectedPokerList.Count == 0)
        {
                hasSelectedItem = false;
        }
        if (isPlayCardPhase)   //若  出牌阶段  
        {
            if (opBox.SelectedPokerList.Count > 0)//若 选择卡牌数量>0
            {
                hasSelectedItem = true;
                PlayCardButton.GetComponent<TextMesh>().color = Color.white;
            }
            else PlayCardButton.GetComponent<TextMesh>().color = Color.gray;
            if (Hand.transform.childCount == 0)  //出牌阶段手牌为0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "不出牌";
                PlayCardButton.GetComponent<TextMesh>().color = Color.white;
                hasSelectedItem = true;
            }
            else PlayCardButton.GetComponent<TextMesh>().text = "出牌";
        }
        else if (isDropCardPhase)          //若 需要弃牌  
        {
            PlayCardButton.GetComponent<TextMesh>().color = Color.white;
            if (opBox.SelectedPokerList.Count > 0)//若 选择卡牌数量>0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "弃牌";
            }
            else
            {
                PlayCardButton.GetComponent<TextMesh>().text = "不弃牌";
            }
            if (Hand.transform.childCount == 0)  //弃牌阶段手牌为0
            {
                PlayCardButton.GetComponent<TextMesh>().text = "不弃牌";
            }
        }
        else
        {
            PlayCardButton.GetComponent<TextMesh>().text = "...";
            PlayCardButton.GetComponent<TextMesh>().color = Color.gray;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //出牌
            if (touch.phase == TouchPhase.Ended && hasSelectedItem && isPlayCardPhase) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if(hit.transform == PlayCardButton.transform)  //点击出牌按钮
                {
                    SEManager.Instance.ClickButton();
                    isPlayCardPhase = false;    //出牌后不可再出牌
                    if (Hand.transform.childCount == 0) //不出牌
                    {
                        MainThread.Instance.FinishPhase(1); //结束出牌阶段
                        MainThread.Instance.FinishPhase(2); //结束技能阶段
                        MainThread.Instance.FinishPhase(3); //结束攻击阶段
                        return;
                    }
                    MainThread.Instance.FinishPhase(1); //结束出牌阶段

                    List<GameObject> list = opBox.SelectedPokerList;   
                    for (int i = list.Count-1; i >=0; i--)
                    {   
                        GameObject go = list[i];
                        go.transform.SetParent(PlayArea.transform);
                        opBox.CancelPoker(go);
                        PlayArea.GetComponent<HandManager>().SortYourHand();
                    }
                }
            }
            //弃牌
            if (touch.phase == TouchPhase.Ended && isDropCardPhase)    //可选择不弃牌，不需要hasSelectedItem
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.transform == PlayCardButton.transform)  //点击弃牌按钮
                {
                    DiscardSumCount.Instance.Unactivate();
                    SEManager.Instance.ClickButton();
                    isDropCardPhase = false;    //弃牌后不可再弃牌

                    int sum = 0;
                    int shield = 0;
                    if (GameObject.Find("SpadeShield(Clone)") != null)
                        shield = GameObject.Find("SpadeShield(Clone)").GetComponent<SpadeShield>().GetShield();
                    sum += shield;
                    hasDiamondNecklace = false;
                    hasHeartNecklace = false;
                    hasAverageDice = false;
                    if (GameObject.Find("HeartNecklace(Clone)") != null) hasHeartNecklace = true;
                    if (GameObject.Find("DiamondNecklace(Clone)") != null) hasDiamondNecklace = true;
                    if (GameObject.Find("AverageDice(Clone)") != null) hasAverageDice = true;

                    //对弃牌进行统计
                    for (int i = 0; i < opBox.SelectedPokerList.Count; i++)
                    {
                        Poker pokerScript = opBox.SelectedPokerList[i].GetComponent<Poker>();
                        if (hasAverageDice == true && pokerScript.cardNumber <= 6) sum += (pokerScript.cardNumber + 1);
                        else sum += pokerScript.cardNumber;
                        if (pokerScript.suit == cardSuit.Hearts || pokerScript.suit == cardSuit.Diamonds)
                        {
                            if (pokerScript.suit == cardSuit.Hearts && hasHeartNecklace) sum += 1;
                            else if (pokerScript.suit == cardSuit.Diamonds && hasDiamondNecklace) sum += 1;
                        }
                    }

                    //有红桃项链
                    if (GameObject.Find("HeartNecklace(Clone)") != null && GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().CheckIfTrue())
                    {
                        if (sum < (bossScript.GetDamage() - (int)(bossScript.GetDamage() * GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().ReduceRate))) //Boss伤害减15%
                        {
                            if (GameObject.Find("ReviveRing(Clone)") != null)//复苏项链抵挡死亡
                                GameObject.Find("ReviveRing(Clone)").GetComponent<ReviveRing>().StartFunction();
                            else MainThread.Instance.Defeat();
                        }
                    }
                    else
                    {
                        if (sum < bossScript.GetDamage())
                        {
                            if (GameObject.Find("ReviveRing(Clone)") != null)//复苏项链抵挡死亡
                                GameObject.Find("ReviveRing(Clone)").GetComponent<ReviveRing>().StartFunction();
                            else MainThread.Instance.Defeat();
                        }
                    }
                    dropCardScript.StartDropCards(opBox.SelectedPokerList);
                    bossScript.AttackState();
                    MainThread.Instance.FinishPhase(4); //结束承受伤害阶段阶段
                                                        //列表清空操作在弃牌脚本内进行
                }
            }
        }
    }

    public int ShouldDiscard()  //返回该弃牌的点数和
    {
        int sum = bossScript.GetDamage();
        int shield = 0; //黑桃护盾

        if (GameObject.Find("HeartNecklace(Clone)") != null && GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().CheckIfTrue())
        {       //有红桃项链且满足激活条件，则要弃牌的数量减少一个百分比
            sum -= (int)(bossScript.GetDamage() * GameObject.Find("HeartNecklace(Clone)").GetComponent<HeartNecklace>().ReduceRate);
        }
        if (GameObject.Find("SpadeShield(Clone)") != null)
            shield = GameObject.Find("SpadeShield(Clone)").GetComponent<SpadeShield>().GetShield();
        sum -= shield;  //要弃牌的点数和减去护盾值
        if (sum < 0) sum = 0;
        return sum;
    }
}
