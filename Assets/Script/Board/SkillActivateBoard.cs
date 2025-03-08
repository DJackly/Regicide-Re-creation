using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class SkillActivateBoard : MonoBehaviour
{
    public GameObject DamageBoard;
    public GameObject DecreaseBoard;
    public GameObject RestoreBoard;
    public GameObject DrawBoard;
    public GameObject PlayArea;
    public GameObject Hand;
    public Boss bossScript;
    public DrawCard drawCardScript;
    public int decrease;
    public int restore;
    public int damage;
    public int draw;
    public GameObject CardDeck;
    public GameObject DiscardPile;

    private bool hasDiamondLace = false;

    public void StartActivation()
    {
        StartCoroutine(ActivateSkill());
    }
    void Start()
    {
        HideAll();
    }
    IEnumerator ActivateSkill()
    {
        decrease = 0;
        restore = 0;
        draw = 0;
        damage = 0;
        int cardNumSum = 0;
        HashSet<cardSuit> suitSet = new HashSet<cardSuit>();
        bool hasAverageDice = false;
        if(GameObject.Find("AverageDice(Clone)")!=null)hasAverageDice = true;   //检测是否拥有平均骰子
        for(int i=0;i<PlayArea.transform.childCount;i++)
        {
            Poker pokerScript = PlayArea.transform.GetChild(i).gameObject.GetComponent<Poker>();
            if(hasAverageDice && pokerScript.cardNumber <= 6) cardNumSum += (pokerScript.cardNumber+1);
            else cardNumSum += pokerScript.cardNumber;       //出的牌的点数和
            suitSet.Add(pokerScript.suit);              //出的牌的所有花色

            if (GameObject.Find("SpadeShield(Clone)") != null && pokerScript.suit == cardSuit.Spades)
                GameObject.Find("SpadeShield(Clone)").GetComponent<SpadeShield>().SpadePlus();
        }
        cardNumSum = DiamondAndHeart(suitSet, cardNumSum);   //检测是否有被项链珍宝影响

        // 若BOSS脚本未禁止花色，则    使得出牌中含有的 与boss相同的花色 无效化
        if (!bossScript.isSuitForbid && suitSet.Contains(bossScript.BossPoker.GetComponent<Poker>().suit))
        {
            if(bossScript.BossPoker.GetComponent<Poker>().suit == cardSuit.Clubs && GameObject.Find("ClubSword(Clone)") != null)
            {   //Boss是梅花且有梅花长剑
                //不禁用花色
            }
            else suitSet.Remove(bossScript.BossPoker.GetComponent<Poker>().suit);
        }

        if (suitSet.Contains(cardSuit.Clubs)) damage = 2 * cardNumSum;  //梅花：双倍伤害
        else damage = cardNumSum;   //其他：一倍伤害
        
        if (suitSet.Contains(cardSuit.Spades))
        {
            decrease = cardNumSum;  //黑桃：减少Boss攻击 
        }
           
        if (suitSet.Contains(cardSuit.Diamonds))   draw = cardNumSum;  //方片：抽牌 
        if (suitSet.Contains(cardSuit.Hearts)) restore = cardNumSum;   //红桃：恢复卡牌 

        if (hasDiamondLace) 
            damage += GameObject.Find("DiamondNecklace(Clone)").GetComponent<DiamondNecklace>().ActivateFunction2();
        
        ShowBoard(1, damage);  SEManager.Instance.ActivateSuit(); yield return new WaitForSeconds(0.5f);
        ShowBoard(2, decrease);SEManager.Instance.ActivateSuit(); yield return new WaitForSeconds(0.5f);
        ShowBoard(3, restore); SEManager.Instance.ActivateSuit(); yield return new WaitForSeconds(0.5f);
        ShowBoard(4, draw);    SEManager.Instance.ActivateSuit(); yield return new WaitForSeconds(0.5f);

        // 伤害在下一阶段结算
        bossScript.SetDamage(bossScript.GetDamage() - decrease);
        if (draw != 0)
        {
            drawCardScript.StartDrawCards(draw);
            if(draw < (8 - Hand.transform.childCount)) yield return new WaitForSeconds(draw * 0.2f);
            else yield return new WaitForSeconds( (8 - Hand.transform.childCount) *0.2f);
        }
        if (restore != 0) DiscardPile.GetComponent<DiscardPile>().StartRestoreCard(restore);

        hasDiamondLace = false;
        yield return new WaitForSeconds(0.4f);
        MainThread.Instance.FinishPhase(2);    //结束卡牌激活阶段
        HideAll();
    }
    public void ShowBoard(int No, int num)
    {
        if (No == 1)
        {
            DamageBoard.GetComponent<TextMesh>().text = "     "+num;
            DamageBoard.SetActive(true);
        }
        else if (No == 2)
        {
            DecreaseBoard.GetComponent<TextMesh>().text = "     " + num;
            DecreaseBoard.SetActive(true);
        }
        else if (No == 3)
        {
            RestoreBoard.GetComponent<TextMesh>().text = "     " + num;
            RestoreBoard.SetActive(true);
        }
        else if (No == 4)
        {
            DrawBoard.GetComponent<TextMesh>().text = "     " + num;
            DrawBoard.SetActive(true);
        }
    }
    public void HideAll()
    {
        DamageBoard.SetActive(false);
        DecreaseBoard.SetActive(false);
        RestoreBoard.SetActive(false);
        DrawBoard.SetActive(false);
    }
    
    private int DiamondAndHeart(HashSet<cardSuit> suitSet, int cardNumTotal)  //检测是否拥有方片&红桃项链，有的话对方片&红桃牌点数＋1
    {
        int delta = 0;
        if(GameObject.Find("HeartNecklace(Clone)") !=null)    //出牌包括红桃且有红桃项链
        {
            if (suitSet.Contains(cardSuit.Hearts))
            {
                delta += 1;
            }
        }
        if (GameObject.Find("DiamondNecklace(Clone)") != null)    //出牌包括方片且有方片项链
        {
            hasDiamondLace = true;
            if (suitSet.Contains(cardSuit.Diamonds))
            {
                delta += 1;
            }
        }
        return cardNumTotal + delta;    //改变点数和
    }
}