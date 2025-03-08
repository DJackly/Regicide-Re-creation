using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWareHouse : MonoBehaviour
{
    //用于记录所有宝物方便商店调用,挂载于Game Center物体
    public static ItemWareHouse Instance;
    public List<GameObject> CardItemList = new List<GameObject>();        //消耗品卡牌
    public List<GameObject> TreasureList = new List<GameObject>();    //宝物列表
    public int[] TreasureCount = new int[20];  //用于记录同索引的TreasureList里的 *宝物* 在商店卖出的次数，若宝物卖出了，则为1，且不会在商店再次出现
    private void Awake()
    {
        Instance = this;
    }
    public void SoldTreasure(int treasureIndex, bool buyFromShop) //商店卖出一样 *宝物* 时记录卖出次数，非消耗品卖出一次后不会再刷新
    {
        if(!buyFromShop) TreasureCount[treasureIndex]--;    //如果不是从商店买，则是卖给商店，计数器减1
        else TreasureCount[treasureIndex]++;
    }
    public bool CheckTreasureAvailable(int index)   //卖出过的宝物不可再出现于商店
    {
        if (TreasureCount[index] == 0)return true;
        else return false;
    }
    public int GetTreasureCount(GameObject itemObject) //用于让商店检查该  *宝物* 是否卖出过，从而判断是否可以上架
    {
        if (!TreasureList.Contains(itemObject)) return -1;
        else return TreasureCount[FindItemIndex(itemObject)];   
    }
    public int FindItemIndex(GameObject itemObject) //找出宝物在图鉴表的索引
    {
        if (!TreasureList.Contains(itemObject) && !CardItemList.Contains(itemObject) ) return -1;

        if (TreasureList.Contains(itemObject))
        {
            for (int i = 0; i < TreasureList.Count; i++)
            {
                if (TreasureList[i] == itemObject) return i;
            }
        }
        else
        {
            for(int i = 0;i< CardItemList.Count;i++)
            {
                if (CardItemList[i] == itemObject) return i;
            }
        }
        return - 1; //无用
    }
    public GameObject FindCardItem(int index)
    {
        if (index < CardItemList.Count)  //索引未越界
        {
            return CardItemList[index];
        }
        else
        {
            Debug.LogWarning("获取消耗卡越界");
            return null;
        }
    }
    public GameObject FindTreasure(int index)
    {
        if (index < TreasureList.Count)  //索引未越界
        {
            return TreasureList[index];
        }
        else
        {
            Debug.LogWarning("获取珍宝越界");
            return null;
        }
       
    }
}
