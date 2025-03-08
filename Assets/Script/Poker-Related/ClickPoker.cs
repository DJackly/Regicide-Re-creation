using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPoker : MonoBehaviour
{
    public bool isPressed = false;
    public GameObject optionalTextBox;
    private int checkNum;
    public bool isSelected = false;
    private static float moveDis = 0.88f;
    public bool canBeClicked = false;

    private void Awake()
    {
        optionalTextBox = GameObject.Find("OptionalBox");
    }
    private void Start()
    {
        MyEventSystem.Instance.MoveCardEnd += CanClick;
        MyEventSystem.Instance.MoveCardStart += CantClick;
    }
    private void OnDisable()
    {
        MyEventSystem.Instance.MoveCardEnd -= CanClick;
        MyEventSystem.Instance.MoveCardStart -= CantClick;
    }
    private void CanClick() { canBeClicked = true; }
    private void CantClick(){ canBeClicked = false; }
    public void Click() //�°�
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;  //�������UI
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (canBeClicked && hit.transform == transform)  //���������
        {
            
            if (isSelected)   //���ǰ���������� ���ǰ ��ѡ�У�����Ҫȡ��ѡ��
            {
                SwitchPressed();
                OptionalBox.Instance.RemovePoker(this.gameObject);
            }
            else     //û�����   ����Ҫ����
            {
                SEManager.Instance.ClickCard();
                if (OptionalBox.Instance.SelectPoker(this.gameObject))   SwitchPressed();
            }

        }
    }
    public void SwitchPressed() //�л��Ƿ񱻵��״̬
    {
        //ԭ�����ﶼ��isPressed
        isSelected  = !isSelected;
        if (isSelected)  //��false���true����������
        {
            MoveInY(true);
        }
        else   //��true���false�������½�
        {
            MoveInY(false);
        }
    }
    private void MoveInY(bool isUp)
    {
        //if (isSelected) return; //��ѡ��ʱ���ƶ�
        
        if(isUp) transform.position = transform.position + new Vector3(0, moveDis, 0);
        else     transform.position = transform.position - new Vector3(0, moveDis, 0);
    }
}
