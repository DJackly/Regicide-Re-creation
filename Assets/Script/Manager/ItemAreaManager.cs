using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ItemAreaManager : MonoBehaviour
{
    //�˽ű���������Լ��ĵ�����
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
        if(GameObject.Find("Shop") !=null ) //˵���˽ű��������̵곡����Ӧ����Ϸ��������ItemArea
        {
            Scene shopScene = SceneManager.GetSceneByName("RegicideGameScene");
            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //���̵곡����ȡ��Ϸ������ItemAreaManager�ű�
            {
                if (rootObject.name == "Grid")
                {
                    for (int i = 0; i < rootObject.transform.childCount; i++)  //��ȡ���Ӷ���
                    {
                        GameObject obj = rootObject.transform.GetChild(i).gameObject;
                        if (obj.name == "ItemArea") PasteToHere( obj.GetComponent<ItemAreaManager>().CopyThisArea() );
                    }
                }
            }
        }
        //����ű�����Ϸ�����еĻ����ɹر��̵����İ�ť������ô��뽫��Ʒͬ��
    }
    public void AddItem(GameObject item, int containerNo = 0)   //���Ϊ0��˳��ţ���������Ӧ��ţ�����copyʱʹ�ã�
    {
        CheckItemCount();
        if (item == null) return;
        item.GetComponent<Item>().isClicked = false;
        if (ItemCount < 3 && containerNo == 0)  //��˳����������
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
            else Debug.LogWarning("��Ʒ����ӳ���");
        }
        else if(ItemCount < 3 && containerNo != 0)  //����copyʱ���ã�������һ���ǿյģ����ü�������Ƿ��
        {
            switch (containerNo)  //���ڶ�Ӧ�ĸ�����
            {
                case 1: item.transform.SetParent(ItemContainer1.transform, true); break;
                case 2: item.transform.SetParent(ItemContainer2.transform, true); break;
                case 3: item.transform.SetParent(ItemContainer3.transform, true); break;
            }
            item.transform.position = item.transform.parent.position + new Vector3(0, 0, -1);
            ItemCount++;
        }
        else Debug.LogWarning("��Ʒ��������");
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
        else Debug.LogWarning("δ�ҵ�Ҫɾ������Ʒ��");
    }
    public void StartCopyToGameScene()  //���̵����Ʒ�����Ƶ���Ϸ����
    {
        Scene scene = SceneManager.GetSceneByName("RegicideGameScene");
        foreach (GameObject rootObject in scene.GetRootGameObjects())   //���̵곡����ȡ��Ϸ������ItemAreaManager�ű�
        {
            if (rootObject.name == "Grid")
            {
                for (int i = 0; i < rootObject.transform.childCount; i++)  //��ȡ���Ӷ���
                {
                    GameObject obj = rootObject.transform.GetChild(i).gameObject;
                    if (obj.name == "ItemArea") obj.GetComponent<ItemAreaManager>().PasteToHere(CopyThisArea());
                }
            }
        }
    }
    public GameObject[] CopyThisArea()  //������Ʒ������Ʒ���Ʋ�����������
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
    public void PasteToHere(GameObject[] list)   //ɾ��ԭ���ģ������»�ȡ��
    {
        if(ItemContainer1.transform.childCount > 0) DeleteItem( ItemContainer1.transform.GetChild(0).gameObject, ItemContainer1.transform);
        if(ItemContainer2.transform.childCount > 0) DeleteItem( ItemContainer2.transform.GetChild(0).gameObject, ItemContainer2.transform);
        if(ItemContainer3.transform.childCount > 0) DeleteItem( ItemContainer3.transform.GetChild(0).gameObject, ItemContainer3.transform);

        if(isInShop)    //�ű����̵곡��
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
