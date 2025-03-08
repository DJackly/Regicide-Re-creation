using OutlineFx;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Item : MonoBehaviour   //����Ϊ���е��ߵĸ��� 
{
    public bool isConsumerable; //�綨����Ʒ �� �⻷Ч����Ʒ
    public int[] effectivePhase;  //�������ý׶Σ�1-5

    public string itemName = "Ĭ����";
    public string itemDescr = "Ĭ������Ĭ������Ĭ������";
    public string itemStory = "Ĭ��Ĭ�Ϲ���";
    public int price;
    public bool isClicked = false;
    public int treasureIndex;  //�����䱦�����ڸ����䱦�ֿ��Ӧ�ļ�����
    protected bool inforBoardBeyongScreen;
    protected bool isInShop = true; //���̵��״̬�µ��������ͬ
    protected bool isActivated = false; //����ʱ��������ſ��Ե��ʹ�ã�����ֻ�ܲ鿴����
    int boxNo;  //�����жϿ���Ƿ���������call��
    float timer = 0f;
    bool startTimer = false;    //�ж��Ƿ�ʼ��ʱ
    float STAY_TIME = 0.7f; //����0.7��������ͣ
    bool beingChecked = false;   //���ڱ������鿴��Ϣ

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
                    if (timer > STAY_TIME)   //��ͣ�鿴Ч��
                    {
                        beingChecked = true;
                        ItemInforBoard.Instance.ShowBoard(this, isActivated);
                        if (!ItemInforBoard.Instance.isBeyondScreen) ItemInforBoard.Instance.gameObject.transform.position = Input.mousePosition;
                        else ItemInforBoard.Instance.gameObject.transform.position = Input.mousePosition + new Vector3(-90, 0, 0);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (timer > STAY_TIME)   //˵���˴��ɿ��ǳ������������ǵ���
                    {
                        beingChecked = false;
                        ItemInforBoard.Instance.HideBoard();
                    }
                    else    //�����ɿ�ʱ������
                    {
                        if (hit.transform == this.transform) //����� 
                        {
                            if (!isClicked)   //û�����
                            {
                                SEManager.Instance.ClickCard();
                                boxNo = ItemUsingBox.Instance.SetItemUsingBox(this.gameObject, isActivated); //ѡ��
                                isClicked = true;
                            }
                            else if (ItemUsingBox.Instance.BoxNo == this.boxNo)  //������� �Ҵ�ʱ���δת�Ƶ������ط�
                            {
                                ItemUsingBox.Instance.ResetBox();   //ȡ��ѡ��
                                isClicked = false;
                            }
                            else   //������� �Ҵ�ʱ���  ��ת�Ƶ������ط�
                            {
                                SEManager.Instance.ClickCard();
                                boxNo = ItemUsingBox.Instance.SetItemUsingBox(this.gameObject, isActivated); //ѡ��
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
        if(isActivated)     //������Ч��
        {
            GetComponent<Outline>().enabled = true;
        }
        if(! isConsumerable) ActivateFunction();    ////�⻷��Ʒ���ڴ˵���activateFunction��������
    }
    protected void PriceFloating()    //����������ã��ڳ�ʼ���۸��Լ۸���и�������
    {
        int delta = price / 10;
        if (delta >= 2) delta -= 1;  //�𸡶�̫����
        delta = Random.Range(-delta, delta+1);
        price += delta;
    }
    public abstract void ActivateFunction();    //����Ʒ�͹⻷Ч���䱦��ʹ�øú��������
    public abstract void DisabledFunction();    //�⻷Ч���䱦�ڱ�ɾ��ʱ���ô˺���������Ʒ�ĸú����ÿռ���
    public abstract void TriggerDrawOutFunction();

}
