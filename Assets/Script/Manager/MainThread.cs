using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    public static MainThread Instance { get; private set; }
    public int currentStage = 1; // ���ڸ��ٵ�ǰ�׶εı���
    private readonly int numberOfStages = 5; // ��5���׶Σ��̵����ڻ�ɱboss�������׶�
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
        if(currentStage == stageNum)   //��ȷ����
        {
            if (currentStage < numberOfStages) currentStage++;
            else currentStage = 1;
            isTurnOn = false;
        }
    }
    public void JumpToPhase(int stageNum)   //�������øú���
    {
        currentStage = stageNum;
        isTurnOn = false;
    } 
    public void PlayCardPhase() //���ƽ׶Σ��������ƺ���ȷ���Խ�����
    {
        if (isGameEnd) return;
        if (isTurnOn) return;
        else isTurnOn = true;

        playCardScript.isPlayCardPhase = true;  //���Գ���
        TipsBoard.Instance.ShowTipsBoard("���ƽ׶�");
        //���ⲿ����FinishPhase()����
    }
    public void SkillActivationPhase() //���Ƽ��ܼ���׶Σ���β�Զ���������˺��׶�
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        skillActivateBoardScript.StartActivation();
        //���ⲿ����FinishPhase()����
    }
    public void ResolveDamagePhase()  //�����˺��׶Σ�δ��ɱboss���������˺��׶Σ���ɱ���������׶�
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        SEManager.Instance.Attack();
        bossScript.SetHp(bossScript.GetHp() - skillActivateBoardScript.damage);
        
        //�������ƹ���������
        List<GameObject> pokerList = new List<GameObject>();
        for(int i=0;i<PlayArea.transform.childCount;i++)
        {
            pokerList.Add(PlayArea.transform.GetChild(i).gameObject);
        }
        dropCardScript.StartDropCards(pokerList);

        if (skillActivateBoardScript.damage >= 17 && GameObject.Find("ClubSword(Clone)") != null)
        {
            FinishPhase(3);
            FinishPhase(4); //���������˺��׶�
        }
        else if (bossScript.GetHp() >0) FinishPhase(3);
        else
        {
            FinishPhase(3);
            FinishPhase(4); //���������˺��׶�
        }
    }
    public void EndureDamagePhase()  //�����˺��׶�:��hp��������������ס���˺����������׶�
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        if (bossScript.GetDamage() != 0)    //boss���˺�����Ҫ����
        {
            playCardScript.isDropCardPhase = true;
            DiscardSumCount.Instance.Activate();    //���Ƽ������
            bossScript.ReadyState();
            TipsBoard.Instance.ShowTipsBoard("�����õ����ܺ͡�" + playCardScript.ShouldDiscard() + "������");
        }
        else FinishPhase(4);

        //���ⲿ���ƽű������ƽű����Ƶ��ǰ�ť��Ҳ�������ƣ�����FinishPhase()����
    }
    public void EndPhase()  //�����׶Σ�����תʤ��/������֣����߳��ƽ׶�
    {
        if (isTurnOn) return;
        else isTurnOn = true;

        //�����ɱ��Boss���뽫��������ƶѣ����߳��ƶѶ�����������,������BOSS������BOSS�������Ϸ
        if (bossScript.GetHp() <= 0)
        {
            isVictory = bossScript.CheckIfVictory();
            bossScript.StartSwitchBoss();
            if(!isVictory)ShopPhase();  //δʤ��������̵�
        }
        else FinishPhase(5);
    }
    public void ShopPhase()     //����׶�:���ڻ���boss�����,ͬ�������׶�EndPhase
    {
        ControlCenter.Instance.GotoShop();
        //EndShopPhase����FinishPhase()�����׶�5
    }
    private void EndShopPhase()
    {
        FinishPhase(5);
    }
    IEnumerator DefeatAnima()
    {
        isGameEnd = true;
        SEManager.Instance.Defeat();
        TipsBoard.Instance.ShowTipsBoard("��������", true);
        //yield return new WaitForSeconds(3f);
        yield return null;
    }
    IEnumerator VictoryAnima()
    {
        isGameEnd = true;
        SEManager.Instance.Victory();
        TipsBoard.Instance.ShowTipsBoard("ʤ��",true);
        //yield return new WaitForSeconds(3f);
        yield return null;
    }
    public void Defeat()    //�������
    {
        StartCoroutine(DefeatAnima());
    }
    public void Victory()   //win
    {
        StartCoroutine(VictoryAnima());
    }
}
