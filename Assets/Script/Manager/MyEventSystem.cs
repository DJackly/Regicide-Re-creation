using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyEventSystem : MonoBehaviour
{
    public static MyEventSystem Instance;
    private void Awake()
    {
        Instance = this;
    }

    public event Action ShopLoadOff;
    public void TriggerShopLoadOff()
    {
        if(ShopLoadOff != null) ShopLoadOff();
    }
    //===========================
    public event Action MoveCardEnd;
    public void TriggerMoveCardEnd()
    {
        if(MoveCardEnd != null) MoveCardEnd();
    }
    //===========================
    public event Action MoveCardStart;
    public void TriggerMoveCardStart()
    {
        if (MoveCardEnd != null) MoveCardStart();
    }
    //===========================
    public event Action EnterShopScene;
    public void TriggerEnterShopScene()
    {
        if(EnterShopScene != null) EnterShopScene();
    }
    //===========================
    public event Action NewBossInit;
    public void TriggerNewBossInit()
    {
        if(NewBossInit != null) NewBossInit();
    }
    //===========================
    /*public event Action HeartNecklaceActivate;
    public event Action DiamondNecklaceActivate;
    public event Action ShieldActivate;
    public event Action ShieldPlus;
    public void TriggerHNActi(){ if(HeartNecklaceActivate != null) HeartNecklaceActivate(); }
    public void TriggerDNActi() { if (DiamondNecklaceActivate != null) DiamondNecklaceActivate(); }
    public void TriggerShieldActi() { if(ShieldActivate != null) ShieldActivate(); }
    public void TriggerShieldPlus() {  if(ShieldPlus!=null) ShieldPlus(); }*/
}
