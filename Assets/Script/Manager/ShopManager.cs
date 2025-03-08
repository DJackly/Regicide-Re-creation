using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    //此脚本挂载于Shop场景的Shop物体上
    public static ShopManager Instance;

    public List<GameObject>ContainerList = new List<GameObject>();
    public List<GameObject>PriceLabelList = new List<GameObject>();
    private ItemWareHouse WareHouseInstance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable() 
    {
        Scene shopScene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //从商店场景获取游戏场景的仓库脚本
        {
            if (rootObject.CompareTag("GameControlCenter"))
            {
                
                WareHouseInstance = rootObject.GetComponent<ItemWareHouse>();
                break;
            }
        }
        //商店开张：四个消耗品格子，第一个固定为小丑卡，第二个格子必开，第三个格子70%机率开，若第三开启则第四有50%几率开
        SetItem(1,WareHouseInstance.FindCardItem(0));       //图鉴0号位为小丑牌，放入一号格子

        int index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //随机获取一个消耗品的索引
        SetItem(2, WareHouseInstance.FindCardItem(index));  //随机获取一个消耗品放入二号格子

        int p = Random.Range(0, 100);
        if (p < 70) //70%概率开启第三个格子
        {
            index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //随机获取一个消耗品的索引
            SetItem(3, WareHouseInstance.FindCardItem(index));  //随机获取一个消耗品放入三号格子

            p = Random.Range(0, 100);   //第三开了才能开第四
            if (p < 50) //50%概率开启第四个格子
            {
                index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //随机获取一个消耗品的索引
                SetItem(4, WareHouseInstance.FindCardItem(index));  //随机获取一个消耗品放入四号格子
            }
        }

        List<int> AvailableIndexList = new List<int>();
        for(int i=0; i<WareHouseInstance.TreasureList.Count; i++) //检查珍宝表的该索引处是否可用
        {
            if (WareHouseInstance.CheckTreasureAvailable(i))  AvailableIndexList.Add(i);    //储存可用的索引
        }
        if(AvailableIndexList.Count >= 2)   //有两个以上可用，两个格子都开张
        {
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(5, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //随机获取一个消耗品放入5号格子
            AvailableIndexList.RemoveAt(index);
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(6, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //随机获取一个消耗品放入6号格子
        }
        else if(AvailableIndexList.Count ==1)
        {
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(5, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //随机获取一个消耗品放入5号格子
        }
        //没有可用的珍宝则不卖了

    }
    private void SetItem(int containerNo, GameObject itemPrefab)
    {
        GameObject itemObject = Instantiate(itemPrefab, ContainerList[containerNo-1].transform);

        itemObject.transform.position = itemObject.transform.parent.position + new Vector3(0, 0, -0.5f);

        SetPrice(containerNo, itemObject.GetComponent<Item>().price);
    }
    private void SetPrice(int priceLabelNo, int price)
    {
        GameObject priceLabel = PriceLabelList[priceLabelNo - 1];
        priceLabel.SetActive(true);
        for(int i=0; i<priceLabel.transform.childCount; i++)
        {
            if (priceLabel.transform.GetChild(i).name == "Price")
                priceLabel.transform.GetChild(i).GetComponent<TextMesh>().text = price.ToString();
        }
    }
    private void HidePrice(int priceLabelNo)
    {
        GameObject priceLabel = PriceLabelList[priceLabelNo - 1];
        priceLabel.SetActive(false);
    }
    public void ItemSold(GameObject itemsParentObject, bool isTreasure)//从店里卖出，即购买商品
    {
        if(isTreasure) { WareHouseInstance.SoldTreasure(itemsParentObject.transform.GetChild(0).gameObject.GetComponent<Item>().treasureIndex, true); }
        for(int i = 0; i < ContainerList.Count; i++)
        {
            if (ContainerList[i] == itemsParentObject)
            {
                HidePrice(i+1);
            }
        }
    }
    private void OnDisable()    //结束时隐藏所有价格
    {
        for(int i = 1; i <= PriceLabelList.Count; i++)
        {
            HidePrice(i);
        }
    }
}

