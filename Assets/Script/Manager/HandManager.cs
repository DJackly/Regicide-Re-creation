using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private float W;    //�������
    private float D = 0.2f;    //����֮��� ��ʼ ���
    private float d;    //����֮��� ʵ�� ���

    private float n;    //��������
    void Start()
    {
        W = this.GetComponent<SpriteRenderer>().bounds.size.x;
        //W = this.GetComponent<RectTransform>().rect.width;
        d = D;
    }

    // Update is called once per frame
    public void SortYourHand()
    {
        if (transform.childCount != 0)
        {  
            d = D;
            n = transform.childCount;
            

            if ((n-1)*d < 0.45) //����㹻
            {
                float X = -d * 0.5f * (n - 1);    //����ߵĿ���X����
                float Z = -0.1f;   //����߿���Z����
                float deltaZ = 0.9f / n;
                for (int i = 0; i < n; i++)
                {
                    Transform cardTrans = transform.GetChild(i);
                    cardTrans.localPosition = new Vector3(X, 0, Z-deltaZ);
                    Z -= deltaZ;
                    X += d;
                }
            }
            else  // ��Ȳ���
            {
                do
                {
                    d -= 0.1f * D;
                } while ((n - 1) * d >= 0.45);

                float X = -d * 0.5f * (n - 1);    //����ߵĿ���X����
                float Z = -0.1f;   //����߿���Z����
                float deltaZ = 0.9f / n;

                for (int i = 0; i < n; i++)
                {
                    Transform cardTrans = transform.GetChild(i);
                    cardTrans.localPosition = new Vector3(X, 0, Z - deltaZ);
                    Z -= deltaZ;
                    X += d;
                }
            }
        }

    }
}
