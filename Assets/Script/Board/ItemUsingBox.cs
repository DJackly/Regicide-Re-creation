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
    private bool itemActivated; //�������Ʒδ���δ�����򵯳������
    public int BoxNo = 0;   //�����жϿ���Ƿ���������call��
    float timer = 0f;   //��ʱ��û��һ���Զ��ر�
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
        if (SelectItem == null) return;     //δѡ����Ʒ
        else    //��ѡ��ĳһ��Ʒ
        {
            timer += Time.deltaTime;
            if(timer > 1.5f)
            {
                ResetBox();
                timer = 0;
                return;
            }
        }
        if (!itemActivated) //�����Ʒδ�����˵�������̵������
        {
            UseButton.SetActive(false);
            DisposeButton.SetActive(false);
            BuyButton.SetActive(true);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)    //���
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                    if (hit.transform == BuyButton.transform)    //�������
                    {
                        if (GemBoard.Instance.GetCurrentGem() < SelectItem.GetComponent<Item>().price)   //Ǯ����
                        {
                            TipsBoard.Instance.ShowTipsBoard("��ʯ����");
                        }
                        else
                        {
                            ItemAreaManager ShopItemAreaScript = null;
                            Scene shopScene = SceneManager.GetSceneByName("Shop");
                            foreach (GameObject rootObject in shopScene.GetRootGameObjects())   //��ȡ�̵곡����ItemAreaManager�ű�
                            {
                                if (rootObject.name == "AllObject")
                                {
                                    for (int i = 0; i < rootObject.transform.childCount; i++)  //��ȡ���Ӷ���
                                    {
                                        GameObject obj = rootObject.transform.GetChild(i).gameObject;
                                        if (obj.name == "ItemArea") ShopItemAreaScript = obj.GetComponent<ItemAreaManager>();
                                    }
                                }
                            }

                            if (ShopItemAreaScript.ItemCount < ShopItemAreaScript.MaxItem)  //�̵곡������Ʒ��δ��
                            {
                                float x = SelectItem.transform.localScale.x;
                                float y = SelectItem.transform.localScale.y;
                                float z = SelectItem.transform.localScale.z;
                                SelectItem.transform.localScale = new Vector3(0.73f * x, 0.73f * y, z);

                                SEManager.Instance.UseItem();
                                GemBoard.Instance.LoseGem(SelectItem.GetComponent<Item>().price);
                                ShopManager.Instance.ItemSold(SelectItem.transform.parent.gameObject, !SelectItem.GetComponent<Item>().isConsumerable);
                                ShopItemAreaScript.AddItem(SelectItem); //�ѹ������Ʒ������Ʒ��
                                SelectItem.GetComponent<Item>().SetActivate(true);
                                ResetBox();
                            }
                            else
                            {
                                TipsBoard.Instance.ShowTipsBoard("��Ʒ������,��������Ʒ��");
                            }


                        }
                    }
                }

                return; //δ������ ����Ĵ��벻ִ�У��ӿ�Ч��
            }
        }
        else
        {
            UseButton.SetActive(true);
            DisposeButton.SetActive(true);
            BuyButton.SetActive(false);
        }

        if (! SelectItem.GetComponent<Item>().isConsumerable) canUse = false;    //����������Ʒ����һֱ������
        else {      //������Ʒ���ж�
            if(isInShop) { canUse = false; }    //�����̵��򲻸���
            else   //�����ڶ�Ӧ�׶β�����
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

        if (Input.touchCount > 0)    //�˴�������Ʒ����Ʒ��ʱ�ĵ��Ч��
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                if (hit.transform == UseButton.transform && canUse)  //��� ʹ�ð�ť �� ����
                {
                    SEManager.Instance.ClickButton();
                    SEManager.Instance.UseItem();
                    SelectItem.GetComponent<Item>().ActivateFunction();
                }
                if (hit.transform == DisposeButton.transform && isInShop)   //���������ť�������̵곡��
                {
                    SEManager.Instance.ClickButton();
                    GemBoard.Instance.AddGem(SelectItem.GetComponent<Item>().price / 2);
                    //��Ϊ�˽ű�������Ϸ�����ĵ���������Ҫ��ȡ�̵곡���Ľű�����
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
