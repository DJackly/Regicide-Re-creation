using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWareHouse : MonoBehaviour
{
    //���ڼ�¼���б��﷽���̵����,������Game Center����
    public static ItemWareHouse Instance;
    public List<GameObject> CardItemList = new List<GameObject>();        //����Ʒ����
    public List<GameObject> TreasureList = new List<GameObject>();    //�����б�
    public int[] TreasureCount = new int[20];  //���ڼ�¼ͬ������TreasureList��� *����* ���̵������Ĵ����������������ˣ���Ϊ1���Ҳ������̵��ٴγ���
    private void Awake()
    {
        Instance = this;
    }
    public void SoldTreasure(int treasureIndex, bool buyFromShop) //�̵�����һ�� *����* ʱ��¼����������������Ʒ����һ�κ󲻻���ˢ��
    {
        if(!buyFromShop) TreasureCount[treasureIndex]--;    //������Ǵ��̵������������̵꣬��������1
        else TreasureCount[treasureIndex]++;
    }
    public bool CheckTreasureAvailable(int index)   //�������ı��ﲻ���ٳ������̵�
    {
        if (TreasureCount[index] == 0)return true;
        else return false;
    }
    public int GetTreasureCount(GameObject itemObject) //�������̵����  *����* �Ƿ����������Ӷ��ж��Ƿ�����ϼ�
    {
        if (!TreasureList.Contains(itemObject)) return -1;
        else return TreasureCount[FindItemIndex(itemObject)];   
    }
    public int FindItemIndex(GameObject itemObject) //�ҳ�������ͼ���������
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
        return - 1; //����
    }
    public GameObject FindCardItem(int index)
    {
        if (index < CardItemList.Count)  //����δԽ��
        {
            return CardItemList[index];
        }
        else
        {
            Debug.LogWarning("��ȡ���Ŀ�Խ��");
            return null;
        }
    }
    public GameObject FindTreasure(int index)
    {
        if (index < TreasureList.Count)  //����δԽ��
        {
            return TreasureList[index];
        }
        else
        {
            Debug.LogWarning("��ȡ�䱦Խ��");
            return null;
        }
       
    }
}
