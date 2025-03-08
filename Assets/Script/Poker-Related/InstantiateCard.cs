using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class InstantiateCard : MonoBehaviour
{
    private Poker poker;
    private SpriteRenderer cardPic;
    private Sprite sprite;

    public void InstantiatePoker()
    {
        poker = gameObject.GetComponent<Poker>();
        cardPic = gameObject.GetComponent<SpriteRenderer>();
        Sprite[] smallImgList = new Sprite[16];
        smallImgList = Resources.LoadAll<Sprite>("cardImg/pic"+poker.picNo);

        //Debug.Log("表的长度：" + smallImgList.Length);
        //Debug.Log("cardImg/pic" + poker.picNo);
        // 将图片设置到 Image 组件
        cardPic.sprite = smallImgList[poker.cardNo];
        
    }
}
