using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSystem : MonoBehaviour
{
    //同时按GEM可获得宝石(GemBoard脚本)，同时按SH可进入商店（ConrolCenter脚本）
    private float timer = 0;
    private bool startCount = false;
    private bool isConsumerableConfirm = false;
    private bool isIndex1Confirm = false;

    private bool isConcumerable;
    private int itemIndex1; //十位
    private int itemIndex2; //个位

    private ItemAreaManager itemAreaManager;
    public GameObject testBox;  //用Item Area的一个格子来实例化，这样出来的大小才正常
    void Update()
    {
        if(Input.GetKey(KeyCode.K))     //击杀当前boss
        {
            if(Input.GetKeyDown(KeyCode.I)) 
            {
                Boss bossScript = GameObject.Find("Boss").GetComponent<Boss>();
                bossScript.SetHp(-1);
                if (MainThread.Instance.currentStage <= 3) MainThread.Instance.JumpToPhase(5);
            }
        }
        if (Input.GetKey(KeyCode.C))    //cheat
        {
            if(Input.GetKeyDown(KeyCode.H)) startCount = true;
        }
        if (startCount)
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                ResetThis();
                return;
            }
            //输入是否是消耗品
            if (Input.GetKeyDown(KeyCode.T))
            {
                isConsumerableConfirm = true;
                isConcumerable = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                isConsumerableConfirm = true;
                isConcumerable = false; 
            }
            //输入两位数
            if (isConsumerableConfirm)
            {
                if(! isIndex1Confirm)   //个位数未输入
                {
                    for(int i = 0; i <= 9; i++)
                    {
                        if(Input.GetKeyDown(KeyCode.Alpha0 + i))
                        {
                            itemIndex1 = i;
                            isIndex1Confirm = true;
                        }
                    }
                }
                else     //个位数输入了
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                        {
                            itemIndex2 = i;
                            AddItem(isConcumerable, 10*itemIndex1 + itemIndex2);
                            ResetThis();
                        }
                    }
                }
            }
            
        }
    }
    private void AddItem(bool consumerable, int index)
    {
        itemAreaManager = GameObject.Find("Grid/ItemArea").GetComponent<ItemAreaManager>();

        GameObject prefab = null;
        if (consumerable)
        {
           if(ItemWareHouse.Instance.FindCardItem(index) != null)prefab = ItemWareHouse.Instance.FindCardItem(index);
        }
        else
        {
            if (ItemWareHouse.Instance.FindTreasure(index) != null)prefab = ItemWareHouse.Instance.FindTreasure(index);
        }
        if (prefab == null) return;
        GameObject item = Instantiate(prefab, testBox.transform);
        item.transform.parent = null;
        item.GetComponent<Item>().SetActivate(true);
        itemAreaManager.AddItem(item);
    }
    private void ResetThis()
    {
        timer = 0;
        startCount = false;
        isConsumerableConfirm = false;
        isIndex1Confirm = false;
    }
}
