using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemUsingBox : MonoBehaviour
{
    public static ItemUsingBox Instance { get; private set; }
    public GameObject UseButton;
    public GameObject DisposeButton;
    public GameObject BuyButton;
    public GameObject SelectItem;
    private bool canUse = false;
    public bool isInShop = false;
    private Vector3 normalScale;
    private Vector3 shopScale;
    private bool itemActivated; //点击的物品未激活（未购买）则弹出购买框
    public int BoxNo = 0;   //用于判断框框是否被其他宝物call过
    float timer = 0f;   //长时间没下一步自动关闭
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        normalScale = transform.localScale;
        shopScale = normalScale - new Vector3( normalScale.x*0.55f, normalScale.y*0.55f, 0  );

        transform.position = transform.position + new Vector3(0, 0, 100);
    }
    private void Start()
    {
        MyEventSystem.Instance.EnterShopScene += () => isInShop = true;
        MyEventSystem.Instance.ShopLoadOff += () => isInShop = false;
    }
    private void OnDestroy()
    {
        MyEventSystem.Instance.EnterShopScene -= () => isInShop = false;
        MyEventSystem.Instance.ShopLoadOff -= () => isInShop = true;
    }
    public int SetItemUsingBox(GameObject item, bool isActivated)
    {
        this.itemActivated = isActivated;
        //this.isInShop = isInShop;
        gameObject.SetActive(true);

        if(isInShop) transform.localScale = shopScale;
        if(!isInShop)this.transform.position = item.transform.parent.position + new Vector3(0, 1.7f, -1.2f);
        else this.transform.position = item.transform.parent.position + new Vector3(0, 0.9f, -1.2f);

        SelectItem = item;
        return ++BoxNo;
    }
    public void ResetBox()
    {
        transform.localScale = normalScale;
        SelectItem = null;
        transform.position = transform.position + new Vector3(0, 0, 100);
    }
    private void Update()
    {
        if (SelectItem == null) return;     //未选中物品
        else    //已选中某一物品
        {
            timer += Time.deltaTime;
            if(timer > 1.5f)
            {
                ResetBox();
                timer = 0;
                return;
            }
        }
        if (!itemActivated) //如果物品未被激活，说明还在商店货架上
        {
            UseButton.SetActive(false);
            DisposeButton.SetActive(false);
            BuyButton.SetActive(true);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)    //点击
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                    if (hit.transform == BuyButton.transform)    //点击购买
                    {
                        if (GemBoard.Instance.GetCurrentGem() < SelectItem.GetComponent<Item>().price)   //钱不够
                        {
                            TipsBoard.Instance.ShowTipsBoard("宝石不足");
                        }
                        else
                        {
                            ItemAreaManager ShopItemAreaScript = null;
                            Scene shopScene = SceneManager.GetSceneByName("Shop");
                            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //获取商店场景的ItemAreaManager脚本
                            {
                                if (rootObject.name == "AllObject")
                                {
                                    for (int i = 0; i < rootObject.transform.childCount; i++)  //获取其子对象
                                    {
                                        GameObject obj = rootObject.transform.GetChild(i).gameObject;
                                        if (obj.name == "ItemArea") ShopItemAreaScript = obj.GetComponent<ItemAreaManager>();
                                    }
                                }
                            }

                            if (ShopItemAreaScript.ItemCount < ShopItemAreaScript.MaxItem)  //商店场景的物品栏未满
                            {
                                float x = SelectItem.transform.localScale.x;
                                float y = SelectItem.transform.localScale.y;
                                float z = SelectItem.transform.localScale.z;
                                SelectItem.transform.localScale = new Vector3(0.73f * x, 0.73f * y, z);

                                SEManager.Instance.UseItem();
                                GemBoard.Instance.LoseGem(SelectItem.GetComponent<Item>().price);
                                ShopManager.Instance.ItemSold(SelectItem.transform.parent.gameObject, !SelectItem.GetComponent<Item>().isConsumerable);
                                ShopItemAreaScript.AddItem(SelectItem); //把购买的物品加入物品栏
                                SelectItem.GetComponent<Item>().SetActivate(true);
                                ResetBox();
                            }
                            else
                            {
                                TipsBoard.Instance.ShowTipsBoard("物品栏已满,请清理物品栏");
                            }


                        }
                    }
                }

                return; //未激活则 后面的代码不执行，加快效率
            }
        }
        else
        {
            UseButton.SetActive(true);
            DisposeButton.SetActive(true);
            BuyButton.SetActive(false);
        }

        if (! SelectItem.GetComponent<Item>().isConsumerable) canUse = false;    //若不是消耗品，则一直不可用
        else {      //是消耗品则判断
            if(isInShop) { canUse = false; }    //若在商店则不给用
            else   //若不在对应阶段不给用
            {
                canUse = false;
                for (int i = 0; i < SelectItem.GetComponent<Item>().effectivePhase.Length; i++)
                {
                    if (MainThread.Instance.currentStage == SelectItem.GetComponent<Item>().effectivePhase[i])
                    {
                        canUse = true; break;
                    }
                }
            }
           
        }
        if(canUse) UseButton.GetComponent<TextMesh>().color = Color.white;
        else UseButton.GetComponent<TextMesh>().color = Color.gray;
        if(isInShop) DisposeButton.GetComponent<TextMesh>().color = Color.white;
        else DisposeButton.GetComponent<TextMesh>().color = Color.gray;

        if (Input.touchCount > 0)    //此处处理物品在物品栏时的点击效果
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.transform == UseButton.transform && canUse)  //点击 使用按钮 且 可用
                {
                    SEManager.Instance.ClickButton();
                    SEManager.Instance.UseItem();
                    SelectItem.GetComponent<Item>().ActivateFunction();
                }
                if (hit.transform == DisposeButton.transform && isInShop)   //点击卖出按钮，且在商店场景
                {
                    SEManager.Instance.ClickButton();
                    GemBoard.Instance.AddGem(SelectItem.GetComponent<Item>().price / 2);
                    //因为此脚本是在游戏场景的单例，所以要获取商店场景的脚本才行
                    Scene gameScene = SceneManager.GetSceneByName("Shop");
                    foreach (GameObject rootObject in gameScene.GetRootGameObjects())
                    {
                        if (rootObject.name == "AllObject")
                        {
                            for (int i = 0; i < rootObject.transform.childCount; i++)
                            {
                                if (rootObject.transform.GetChild(i).name == "ItemArea")
                                    rootObject.transform.GetChild(i).GetComponent<ItemAreaManager>().DeleteItem(SelectItem, SelectItem.transform.parent);
                            }
                        }
                    }
                }

                ResetBox();
            }
        }
    }
}
