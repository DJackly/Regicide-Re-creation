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
    public void Click() //新版
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;  //若点击到UI
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (canBeClicked && hit.transform == transform)  //点击到卡牌
        {
            
            if (isSelected)   //点击前被按过，即 点击前 被选中，则需要取消选中
            {
                SwitchPressed();
                OptionalBox.Instance.RemovePoker(this.gameObject);
            }
            else     //没被点过   则需要被点
            {
                SEManager.Instance.ClickCard();
                if (OptionalBox.Instance.SelectPoker(this.gameObject))   SwitchPressed();
            }

        }
    }
    public void SwitchPressed() //切换是否被点击状态
    {
        //原本这里都是isPressed
        isSelected  = !isSelected;
        if (isSelected)  //从false变成true，卡牌上升
        {
            MoveInY(true);
        }
        else   //从true变成false，卡牌下降
        {
            MoveInY(false);
        }
    }
    private void MoveInY(bool isUp)
    {
        //if (isSelected) return; //被选中时不移动
        
        if(isUp) transform.position = transform.position + new Vector3(0, moveDis, 0);
        else     transform.position = transform.position - new Vector3(0, moveDis, 0);
    }
}
