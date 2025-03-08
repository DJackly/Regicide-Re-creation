using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item : MonoBehaviour   //此类为所有道具的父类 
{
    public bool isConsumerable; //界定消耗品 或 光环效果物品
    public int[] effectivePhase;  //道具作用阶段，1-5

    public string itemName = "默认名";
    public string itemDescr = "默认描述默认描述默认描述";
    public string itemStory = "默认默认故事";
    public int price;
    public bool isClicked = false;
    public int treasureIndex;  //仅限珍宝，用于更改珍宝仓库对应的计数器
    protected bool inforBoardBeyongScreen;
    protected bool isInShop = true; //在商店的状态下点击弹窗不同
    protected bool isActivated = false; //卖出时激活，激活后才可以点击使用，否则只能查看功能
    int boxNo;  //用于判断框框是否被其他宝物call过
    float timer = 0f;
    bool startTimer = false;    //判断是否开始计时
    float STAY_TIME = 0.7f; //长按0.7秒算作悬停
    bool beingChecked = false;   //正在被长按查看信息

    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.transform == this.transform)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) startTimer = true;
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (timer > STAY_TIME)   //悬停查看效果
                    {
                        beingChecked = true;
                        ItemInforBoard.Instance.ShowBoard(this, isActivated);
                        if (!ItemInforBoard.Instance.isBeyondScreen) ItemInforBoard.Instance.gameObject.transform.position = Input.mousePosition;
                        else ItemInforBoard.Instance.gameObject.transform.position = Input.mousePosition + new Vector3(-90, 0, 0);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (timer > STAY_TIME)   //说明此次松开是长按结束，不是单击
                    {
                        beingChecked = false;
                        ItemInforBoard.Instance.HideBoard();
                    }
                    else    //否则松开时处理单击
                    {
                        if (hit.transform == this.transform) //被点击 
                        {
                            if (!isClicked)   //没被点过
                            {
                                SEManager.Instance.ClickCard();
                                boxNo = ItemUsingBox.Instance.SetItemUsingBox(this.gameObject, isActivated); //选中
                                isClicked = true;
                            }
                            else if (ItemUsingBox.Instance.BoxNo == this.boxNo)  //被点击过 且此时框框未转移到其他地方
                            {
                                ItemUsingBox.Instance.ResetBox();   //取消选中
                                isClicked = false;
                            }
                            else   //被点击过 且此时框框  已转移到其他地方
                            {
                                SEManager.Instance.ClickCard();
                                boxNo = ItemUsingBox.Instance.SetItemUsingBox(this.gameObject, isActivated); //选中
                                isClicked = true;
                            }
                        }
                    }
                    startTimer = false;
                    timer = 0f;
                }

            }
            else if( beingChecked == true)
            {
                beingChecked = false;
                startTimer = false;
                timer = 0f;
                ItemInforBoard.Instance.HideBoard();
            }
        }
    }
    
    public void SetActivate(bool isActive)  
    {
        isActivated = isActive;
        if(isActivated)     //激活后的效果
        {
            GetComponent<Outline>().enabled = true;
        }
        if(! isConsumerable) ActivateFunction();    ////光环物品请在此调用activateFunction开启功能
    }
    protected void PriceFloating()    //供子物体调用，在初始化价格后对价格进行浮动调整
    {
        int delta = price / 10;
        if (delta >= 2) delta -= 1;  //别浮动太大了
        delta = Random.Range(-delta, delta+1);
        price += delta;
    }
    public abstract void ActivateFunction();    //消耗品和光环效果珍宝都使用该函数激活功能
    public abstract void DisabledFunction();    //光环效果珍宝在被删除时调用此函数，消耗品的该函数置空即可
    public abstract void TriggerDrawOutFunction();

}
