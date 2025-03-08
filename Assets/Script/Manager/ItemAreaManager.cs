using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ItemAreaManager : MonoBehaviour
{
    //此脚本管理的是自己的道具栏
    //public static ItemAreaManager Instance { get; private set; }
    public GameObject ItemContainer1;
    public GameObject ItemContainer2;
    public GameObject ItemContainer3;
    public int MaxItem = 3;
    public int ItemCount = 0;
    public bool isInShop;

    /*private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance );
            return;
        }
        Instance = this;
    }*/
    private void Start()
    {
        if(GameObject.Find("Shop") !=null ) //说明此脚本挂载于商店场景，应从游戏场景复制ItemArea
        {
            Scene shopScene = SceneManager.GetSceneByName("RegicideGameScene");
            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //从商店场景获取游戏场景的ItemAreaManager脚本
            {
                if (rootObject.name == "Grid")
                {
                    for (int i = 0; i < rootObject.transform.childCount; i++)  //获取其子对象
                    {
                        GameObject obj = rootObject.transform.GetChild(i).gameObject;
                        if (obj.name == "ItemArea") PasteToHere( obj.GetComponent<ItemAreaManager>().CopyThisArea() );
                    }
                }
            }
        }
        //如果脚本在游戏场景中的话，由关闭商店界面的按钮负责调用代码将物品同步
    }
    public void AddItem(GameObject item, int containerNo = 0)   //编号为0则按顺序放，否则放入对应编号（仅在copy时使用）
    {
        CheckItemCount();
        if (item == null) return;
        item.GetComponent<Item>().isClicked = false;
        if (ItemCount < 3 && containerNo == 0)  //按顺序放入空容器
        {
            if (ItemContainer1.transform.childCount == 0)
            {
                item.transform.SetParent(ItemContainer1.transform, true);
                item.transform.position = item.transform.parent.position + new Vector3(0, 0, -1);
                ItemCount++;
            }
            else if (ItemContainer2.transform.childCount == 0)
            {
                item.transform.SetParent(ItemContainer2.transform, true);
                item.transform.position = item.transform.parent.position + new Vector3(0, 0, -1);
                ItemCount++;
            }
            else if (ItemContainer3.transform.childCount == 0)
            {
                item.transform.SetParent(ItemContainer3.transform, true);
                item.transform.position = item.transform.parent.position + new Vector3(0, 0, -1);
                ItemCount++;
            }
            else Debug.LogWarning("物品栏添加出错");
        }
        else if(ItemCount < 3 && containerNo != 0)  //仅在copy时调用，格子内一定是空的，不用检查容器是否空
        {
            switch (containerNo)  //归于对应的格子内
            {
                case 1: item.transform.SetParent(ItemContainer1.transform, true); break;
                case 2: item.transform.SetParent(ItemContainer2.transform, true); break;
                case 3: item.transform.SetParent(ItemContainer3.transform, true); break;
            }
            item.transform.position = item.transform.parent.position + new Vector3(0, 0, -1);
            ItemCount++;
        }
        else Debug.LogWarning("物品栏已满！");
    }
    public void DeleteItem(GameObject item, Transform itemParent)
    {
        if (ItemContainer1.transform == itemParent)
        {
            if (ItemContainer1.transform.GetChild(0).gameObject == item)
            {
                item.GetComponent<Item>().DisabledFunction();
                Destroy(item.gameObject);
                ItemCount--;
            }
        }
        else if (ItemContainer2.transform == itemParent)
        {
            if (ItemContainer2.transform.GetChild(0).gameObject == item)
            {
                item.GetComponent<Item>().DisabledFunction();
                Destroy(item.gameObject);
                ItemCount--;
            }
        }
        else if (ItemContainer3.transform == itemParent)
        {
            if (ItemContainer3.transform.GetChild(0).gameObject == item)
            {
                item.GetComponent<Item>().DisabledFunction();
                Destroy(item.gameObject);
                ItemCount--;
            }
        }
        else Debug.LogWarning("未找到要删除的物品！");
    }
    public void StartCopyToGameScene()  //把商店的物品栏复制到游戏场景
    {
        Scene scene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in scene.GetRootGameObjects())   //从商店场景获取游戏场景的ItemAreaManager脚本
        {
            if (rootObject.name == "Grid")
            {
                for (int i = 0; i < rootObject.transform.childCount; i++)  //获取其子对象
                {
                    GameObject obj = rootObject.transform.GetChild(i).gameObject;
                    if (obj.name == "ItemArea") obj.GetComponent<ItemAreaManager>().PasteToHere(CopyThisArea());
                }
            }
        }
    }
    public GameObject[] CopyThisArea()  //将本物品栏的物品复制并发给调用者
    {
        ItemCount = 0;
        GameObject[] list = new GameObject[3];
        if (ItemContainer1.transform.childCount > 0)
            list[0] = ( ItemContainer1.transform.GetChild(0).gameObject);
        if (ItemContainer2.transform.childCount > 0)
            list[1] = (ItemContainer2.transform.GetChild(0).gameObject);
        if (ItemContainer3.transform.childCount > 0)
            list[2] = (ItemContainer3.transform.GetChild(0).gameObject);
        return list;
    }
    public void PasteToHere(GameObject[] list)   //删除原来的，加入新获取的
    {
        if(ItemContainer1.transform.childCount > 0) DeleteItem( ItemContainer1.transform.GetChild(0).gameObject, ItemContainer1.transform);
        if(ItemContainer2.transform.childCount > 0) DeleteItem( ItemContainer2.transform.GetChild(0).gameObject, ItemContainer2.transform);
        if(ItemContainer3.transform.childCount > 0) DeleteItem( ItemContainer3.transform.GetChild(0).gameObject, ItemContainer3.transform);

        if(isInShop)    //脚本在商店场景
        {
            for(int i = 0; i < 3;i++)
            {
                if (list[i] == null) continue;
                float x = list[i].transform.localScale.x / 2;
                float y = list[i].transform.localScale.y / 2;
                list[i].transform.localScale = list[i].transform.localScale - new Vector3(x,y,0);
            }
            AddItem(list[0],1);
            AddItem(list[1],2);
            AddItem(list[2],3);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (list[i] == null) continue;
                float x = list[i].transform.localScale.x;
                float y = list[i].transform.localScale.y;
                list[i].transform.localScale = list[i].transform.localScale + new Vector3(x, y, 0);
            }
            AddItem(list[0], 1);
            AddItem(list[1], 2);
            AddItem(list[2], 3);
        }
    }
    private void CheckItemCount()
    {
        ItemCount = 0;
        if (ItemContainer1.transform.childCount > 0) ItemCount++;
        if (ItemContainer2.transform.childCount > 0) ItemCount++;
        if (ItemContainer3.transform.childCount > 0) ItemCount++;
    }
}
