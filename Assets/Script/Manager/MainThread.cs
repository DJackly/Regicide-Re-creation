using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    public static MainThread Instance { get; private set; }
    public int currentStage = 1; // 用于跟踪当前阶段的变量
    private readonly int numberOfStages = 5; // 有5个阶段：商店属于击杀boss后的特殊阶段
    public bool isTurnOn = false;

    public PlayCard playCardScript;
    public SkillActivateBoard skillActivateBoardScript;
    public Boss bossScript;
    public GameObject PlayArea;
    public DropCard dropCardScript;

    private bool isGameEnd = false;
    private bool isVictory = false;
    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void OnDestroy()
    {
        MyEventSystem.Instance.ShopLoadOff -= EndShopPhase;
    }
    private void Start()
    {
        MyEventSystem.Instance.ShopLoadOff += EndShopPhase;
    }
    private void Update()
    {
        switch (currentStage)
        {
            case 1:
                PlayCardPhase();break;
            case 2:
                SkillActivationPhase();break;
            case 3:
                ResolveDamagePhase(); break;
            case 4:
                EndureDamagePhase(); break;
            case 5:
                EndPhase(); break;
        }
    }
    public void FinishPhase(int stageNum)
    {
        if(currentStage == stageNum)   //正确调用
        {
            if (currentStage < numberOfStages) currentStage++;
            else currentStage = 1;
            isTurnOn = false;
        }
    }
    public void JumpToPhase(int stageNum)   //谨慎调用该函数
    {
        currentStage = stageNum;
        isTurnOn = false;
    } 
    public void PlayCardPhase() //出牌阶段（出多张牌后点击确认以结束）
    {
        if (isGameEnd) return;
        if (isTurnOn) return;
        else isTurnOn = true;

        playCardScript.isPlayCardPhase = true;  //可以出牌
        TipsBoard.Instance.ShowTipsBoard("出牌阶段");
        //由外部调用FinishPhase()结束
    }
    public void SkillActivationPhase() //卡牌技能激活阶段：结尾自动进入结算伤害阶段
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        skillActivateBoardScript.StartActivation();
        //由外部调用FinishPhase()结束
    }
    public void ResolveDamagePhase()  //结算伤害阶段：未击杀boss则进入承受伤害阶段，击杀则进入结束阶段
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        SEManager.Instance.Attack();
        bossScript.SetHp(bossScript.GetHp() - skillActivateBoardScript.damage);
        
        //将出的牌归入弃牌区
        List<GameObject> pokerList = new List<GameObject>();
        for(int i=0;i<PlayArea.transform.childCount;i++)
        {
            pokerList.Add(PlayArea.transform.GetChild(i).gameObject);
        }
        dropCardScript.StartDropCards(pokerList);

        if (skillActivateBoardScript.damage >= 17 && GameObject.Find("ClubSword(Clone)") != null)
        {
            FinishPhase(3);
            FinishPhase(4); //跳过承受伤害阶段
        }
        else if (bossScript.GetHp() >0) FinishPhase(3);
        else
        {
            FinishPhase(3);
            FinishPhase(4); //跳过承受伤害阶段
        }
    }
    public void EndureDamagePhase()  //承受伤害阶段:若hp归零则死亡，抗住了伤害则进入结束阶段
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        if (bossScript.GetDamage() != 0)    //boss有伤害才需要弃牌
        {
            playCardScript.isDropCardPhase = true;
            DiscardSumCount.Instance.Activate();    //弃牌计数面板
            bossScript.ReadyState();
            TipsBoard.Instance.ShowTipsBoard("请弃置点数总和≥" + playCardScript.ShouldDiscard() + "的手牌");
        }
        else FinishPhase(4);

        //由外部出牌脚本（出牌脚本控制的是按钮，也负责弃牌）调用FinishPhase()结束
    }
    public void EndPhase()  //结束阶段：可跳转胜利/死亡结局，或者出牌阶段
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        //如果击杀了Boss，请将其加入弃牌堆，或者抽牌堆顶（荣誉消灭）,并更新BOSS，若无BOSS则结束游戏
        if (bossScript.GetHp() <= 0)
        {
            isVictory = bossScript.CheckIfVictory();
            bossScript.StartSwitchBoss();
            if(!isVictory)ShopPhase();  //未胜利则进入商店
        }
        else FinishPhase(5);
    }
    public void ShopPhase()     //购物阶段:仅在击败boss后进入,同属结束阶段EndPhase
    {
        ControlCenter.Instance.GotoShop();
        //EndShopPhase调用FinishPhase()结束阶段5
    }
    private void EndShopPhase()
    {
        FinishPhase(5);
    }
    IEnumerator DefeatAnima()
    {
        isGameEnd = true;
        SEManager.Instance.Defeat();
        TipsBoard.Instance.ShowTipsBoard("死亡……", true);
        //yield return new WaitForSeconds(3f);
        yield return null;
    }
    IEnumerator VictoryAnima()
    {
        isGameEnd = true;
        SEManager.Instance.Victory();
        TipsBoard.Instance.ShowTipsBoard("胜利",true);
        //yield return new WaitForSeconds(3f);
        yield return null;
    }
    public void Defeat()    //死亡结局
    {
        StartCoroutine(DefeatAnima());
    }
    public void Victory()   //win
    {
        StartCoroutine(VictoryAnima());
    }
}
