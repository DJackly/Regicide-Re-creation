using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Statement : MonoBehaviour
{
    public string statementName;
    public string description;
    public bool hasCount;   //���޼�����
    protected int statementCount;
    public GameObject CountObj;

    float timer = 0f;
    bool startTimer = false;
    float STAY_TIME = 0.7f; //����0.7����������
    bool beingChecked = false;   //���ڱ������鿴��Ϣ
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
                    if(timer > STAY_TIME)   //��ͣ�鿴Ч��
                    {
                        beingChecked = true;
                        StatementBoard.Instance.ShowBoard(this);
                        StatementBoard.Instance.gameObject.transform.position = new Vector3(touch.position.x,touch.position.y,0 ) + new Vector3(0,0,0);
                    }
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    if(timer > STAY_TIME)   //˵���˴��ɿ��ǳ������������ǵ���
                    {
                        beingChecked = false;
                        StatementBoard.Instance.HideBoard();
                    }
                    //�����ɿ�ʱ������
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
