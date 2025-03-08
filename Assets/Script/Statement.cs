using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Statement : MonoBehaviour
{
    public string statementName;
    public string description;
    public bool hasCount;   //有无计数器
    protected int statementCount;
    public GameObject CountObj;

    float timer = 0f;
    bool startTimer = false;
    float STAY_TIME = 0.7f; //长按0.7秒算作长按
    bool beingChecked = false;   //正在被长按查看信息
    public void SetCount(int count)
    {
        if (!hasCount) return;
        else
        {
            statementCount = count;
            CountObj.GetComponent<TextMesh>().text = count.ToString();
        }
    }
    private void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
        }
        if(Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.transform == this.transform)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)startTimer = true;
                else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if(timer > STAY_TIME)   //悬停查看效果
                    {
                        beingChecked = true;
                        StatementBoard.Instance.ShowBoard(this);
                        StatementBoard.Instance.gameObject.transform.position = new Vector3(touch.position.x,touch.position.y,0 ) + new Vector3(0,0,0);
                    }
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    if(timer > STAY_TIME)   //说明此次松开是长按结束，不是单击
                    {
                        beingChecked = false;
                        StatementBoard.Instance.HideBoard();
                    }
                    //否则松开时处理单击
                    startTimer = false;
                    timer = 0f;
                }
            }
            else if(beingChecked == true)
            {
                StatementBoard.Instance.HideBoard();
                startTimer = false;
                timer = 0f;
                beingChecked = false;
            }
        }
    }
}
