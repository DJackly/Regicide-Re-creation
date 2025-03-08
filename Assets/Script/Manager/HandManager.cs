using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private float W;    //容器宽度
    private float D = 0.2f;    //卡牌之间的 初始 间隔
    private float d;    //卡牌之间的 实际 间隔

    private float n;    //卡牌数量
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
            

            if ((n-1)*d < 0.45) //宽度足够
            {
                float X = -d * 0.5f * (n - 1);    //最左边的卡的X坐标
                float Z = -0.1f;   //最左边卡的Z坐标
                float deltaZ = 0.9f / n;
                for (int i = 0; i < n; i++)
                {
                    Transform cardTrans = transform.GetChild(i);
                    cardTrans.localPosition = new Vector3(X, 0, Z-deltaZ);
                    Z -= deltaZ;
                    X += d;
                }
            }
            else  // 宽度不够
            {
                do
                {
                    d -= 0.1f * D;
                } while ((n - 1) * d >= 0.45);

                float X = -d * 0.5f * (n - 1);    //最左边的卡的X坐标
                float Z = -0.1f;   //最左边卡的Z坐标
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
