using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionalBox : MonoBehaviour
{
    public static OptionalBox Instance { get; private set; }
    private int OptionalBoxNum = 0;
    public GameObject PokerObject;
    public GameObject SelectButton;
    public GameObject CancelButton;
    public List<GameObject> SelectedPokerList;
    private bool buttonCondition = false;
    public PlayCard playCardScript;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        this.transform.position = new Vector3(0, 0, 5);
        SelectButton = GameObject.Find("OptionalBox/SelectButton");
        CancelButton = GameObject.Find("OptionalBox/CancelButton");
    }
    public bool SelectPoker(GameObject poker)   //返回值代表是否选择成功
    {
        //仅在出牌阶段：  此处检测选择多张卡时是否符合规则
        if (playCardScript.isPlayCardPhase && SelectedPokerList.Count == 1)    //按下选择按钮前已选了一张，说明要选第二张了
        {
            if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != 1)   //第一张未选择宠物卡
            {       //且 未选择同数字卡进行连招
                if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != poker.GetComponent<Poker>().cardNumber)
                {
                    if (poker.GetComponent<Poker>().cardNumber != 1)   //数字不同 且 第二张选的也不是宠物卡
                    {
                        TipsBoard.Instance.ShowTipsBoard("仅能通过宠物卡 或者 同数字的卡进行连招");
                        return false;
                    }
                    //else;   //数字不同 但 第二张选宠物卡 则可以
                }
                else if (poker.GetComponent<Poker>().cardNumber * 2 > 10) // 同数字卡连招点数大于10 不行
                {
                    TipsBoard.Instance.ShowTipsBoard("同数字的卡进行连招时数字之和不能大于10");
                    return false;
                }
                //else;   //第一张未选择宠物卡 但正常进行同数字连招 正常执行选择操作
            }
            //else;// 第一张就是宠物卡，第二张选什么都行
        }
        else if (playCardScript.isPlayCardPhase && SelectedPokerList.Count > 1)    //已经选了两三张，继续点击选择的情况
        {
            bool flag = false;
            for (int i = SelectedPokerList.Count - 1; i >= 0; i--)
            {
                if (SelectedPokerList[i].GetComponent<Poker>().cardNumber == 1) //确定已选择宠物连招打法
                    flag = true;
            }
            if (flag)   //宠物连招
            {
                TipsBoard.Instance.ShowTipsBoard("使用宠物卡进行连招时只能出两张牌");
                return false;
            }
            else  //同数字连招
            {
                if (poker.GetComponent<Poker>().cardNumber != SelectedPokerList[0].GetComponent<Poker>().cardNumber)
                {
                    TipsBoard.Instance.ShowTipsBoard("仅能通过宠物卡 或者 同数字的卡进行连招");
                    return false;
                }
                else if (poker.GetComponent<Poker>().cardNumber * (SelectedPokerList.Count + 1) > 10)
                {
                    TipsBoard.Instance.ShowTipsBoard("同数字的卡进行连招时数字之和不能大于10");
                    return false;
                }
            }
        }
        //检测结束

        SelectedPokerList.Add(poker); //将选择的卡加入列表
        return true;
    }
    public void RemovePoker(GameObject poker)
    {
        if(poker != null && SelectedPokerList.Contains(poker))
        {
            SelectedPokerList.Remove(poker);
        }
    }

    private void Update()
    {
        if (buttonCondition)    //true代表默认的“选择”
        {
            SelectButton.SetActive(true);
            CancelButton.SetActive(false);
        }
        else
        {
            SelectButton.SetActive(false);
            CancelButton.SetActive(true);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.transform == SelectButton.transform) //按下选择按钮
                {
                    if (SelectedPokerList.Contains(PokerObject)) return;    //已选择的卡无法按选择按钮

                    //仅在出牌阶段：  此处检测选择多张卡时是否符合规则
                    if (playCardScript.isPlayCardPhase && SelectedPokerList.Count == 1)    //按下选择按钮前已选了一张，说明要选第二张了
                    {
                        if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != 1)   //第一张未选择宠物卡
                        {       //且 未选择同数字卡进行连招
                            if (SelectedPokerList[0].GetComponent<Poker>().cardNumber != PokerObject.GetComponent<Poker>().cardNumber)
                            {
                                if (PokerObject.GetComponent<Poker>().cardNumber != 1)   //数字不同 且 第二张选的也不是宠物卡
                                {
                                    TipsBoard.Instance.ShowTipsBoard("仅能通过宠物卡 或者 同数字的卡进行连招");
                                    return;
                                }
                                //else;   //数字不同 但 第二张选宠物卡 则可以
                            }
                            else if (PokerObject.GetComponent<Poker>().cardNumber * 2 > 10) // 同数字卡连招点数大于10 不行
                            {
                                TipsBoard.Instance.ShowTipsBoard("同数字的卡进行连招时数字之和不能大于10");
                                return;
                            }
                            //else;   //第一张未选择宠物卡 但正常进行同数字连招 正常执行选择操作
                        }
                        //else;// 第一张就是宠物卡，第二张选什么都行
                    }
                    else if (playCardScript.isPlayCardPhase && SelectedPokerList.Count > 1)    //已经选了两三张，继续点击选择的情况
                    {
                        bool flag = false;
                        for (int i = SelectedPokerList.Count - 1; i >= 0; i--)
                        {
                            if (SelectedPokerList[i].GetComponent<Poker>().cardNumber == 1) //确定已选择宠物连招打法
                                flag = true;
                        }
                        if (flag)   //宠物连招
                        {
                            TipsBoard.Instance.ShowTipsBoard("使用宠物卡进行连招时只能出两张牌");
                            return;
                        }
                        else  //同数字连招
                        {
                            if (PokerObject.GetComponent<Poker>().cardNumber != SelectedPokerList[0].GetComponent<Poker>().cardNumber)
                            {
                                TipsBoard.Instance.ShowTipsBoard("仅能通过宠物卡 或者 同数字的卡进行连招");
                                return;
                            }
                            else if (PokerObject.GetComponent<Poker>().cardNumber * (SelectedPokerList.Count + 1) > 10)
                            {
                                TipsBoard.Instance.ShowTipsBoard("同数字的卡进行连招时数字之和不能大于10");
                                return;
                            }
                        }
                    }
                    //检测结束

                    SelectedPokerList.Add(PokerObject); //将选择的卡加入列表
                    PokerObject.GetComponent<ClickPoker>().isSelected = true;
                    PokerObject = null;                 //当前所在的卡置空
                    this.gameObject.SetActive(false);
                    OptionalBoxNum++;
                }
                else if (hit.transform == CancelButton.transform)   //按下取消按钮
                {
                    if (SelectedPokerList.Contains(PokerObject)) SelectedPokerList.Remove(PokerObject);

                    PokerObject.GetComponent<ClickPoker>().isSelected = false;
                    PokerObject.GetComponent<ClickPoker>().SwitchPressed(); //切换当前卡的状态，使之未被点击
                    PokerObject = null;                 //当前所在的卡置空
                    this.gameObject.SetActive(false);
                    OptionalBoxNum++;
                }
            }
        }
    }
    public int GetOptionalBoxNum()  //获取编号
    {
        return OptionalBoxNum;
    }
    public void SetOptionalBoxOn(GameObject go)  //切换选择框对应的卡牌
    {
        OptionalBoxNum++;
        PokerObject = go;
        if (PokerObject.GetComponent<ClickPoker>().isSelected) buttonCondition = false;
        else buttonCondition = true;
    }
    
    public void CancelPoker(GameObject poker)   //模拟为poker按下取消按钮 
    {
        if (SelectedPokerList.Contains(poker)) SelectedPokerList.Remove(poker);

        //poker.GetComponent<ClickPoker>().isSelected = false;
        poker.GetComponent<ClickPoker>().SwitchPressed();     //切换当前卡的状态，使之未被点击
        PokerObject = null;                 //当前所在的卡置空
        this.gameObject.SetActive(false);
        OptionalBoxNum++;
    }
    public void CancelAll()
    {
        for(int i= SelectedPokerList.Count-1; i>=0;i--)
        {
            CancelPoker(SelectedPokerList[i]);
        }
    }
}
