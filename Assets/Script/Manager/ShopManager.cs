using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    //�˽ű�������Shop������Shop������
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
        foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //���̵곡����ȡ��Ϸ�����Ĳֿ�ű�
        {
            if (rootObject.CompareTag("GameControlCenter"))
            {
                
                WareHouseInstance = rootObject.GetComponent<ItemWareHouse>();
                break;
            }
        }
        //�̵꿪�ţ��ĸ�����Ʒ���ӣ���һ���̶�ΪС�󿨣��ڶ������ӱؿ�������������70%���ʿ��������������������50%���ʿ�
        SetItem(1,WareHouseInstance.FindCardItem(0));       //ͼ��0��λΪС���ƣ�����һ�Ÿ���

        int index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //�����ȡһ������Ʒ������
        SetItem(2, WareHouseInstance.FindCardItem(index));  //�����ȡһ������Ʒ������Ÿ���

        int p = Random.Range(0, 100);
        if (p < 70) //70%���ʿ�������������
        {
            index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //�����ȡһ������Ʒ������
            SetItem(3, WareHouseInstance.FindCardItem(index));  //�����ȡһ������Ʒ�������Ÿ���

            p = Random.Range(0, 100);   //�������˲��ܿ�����
            if (p < 50) //50%���ʿ������ĸ�����
            {
                index = Random.Range(0, WareHouseInstance.CardItemList.Count);   //�����ȡһ������Ʒ������
                SetItem(4, WareHouseInstance.FindCardItem(index));  //�����ȡһ������Ʒ�����ĺŸ���
            }
        }

        List<int> AvailableIndexList = new List<int>();
        for(int i=0; i<WareHouseInstance.TreasureList.Count; i++) //����䱦��ĸ��������Ƿ����
        {
            if (WareHouseInstance.CheckTreasureAvailable(i))  AvailableIndexList.Add(i);    //������õ�����
        }
        if(AvailableIndexList.Count >= 2)   //���������Ͽ��ã��������Ӷ�����
        {
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(5, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //�����ȡһ������Ʒ����5�Ÿ���
            AvailableIndexList.RemoveAt(index);
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(6, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //�����ȡһ������Ʒ����6�Ÿ���
        }
        else if(AvailableIndexList.Count ==1)
        {
            index = Random.Range(0, AvailableIndexList.Count);
            SetItem(5, WareHouseInstance.FindTreasure( AvailableIndexList[index] ));  //�����ȡһ������Ʒ����5�Ÿ���
        }
        //û�п��õ��䱦������

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
    public void ItemSold(GameObject itemsParentObject, bool isTreasure)//�ӵ�����������������Ʒ
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
    private void OnDisable()    //����ʱ�������м۸�
    {
        for(int i = 1; i <= PriceLabelList.Count; i++)
        {
            HidePrice(i);
        }
    }
}

