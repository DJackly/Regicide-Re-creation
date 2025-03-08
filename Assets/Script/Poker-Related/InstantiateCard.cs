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

        //Debug.Log("��ĳ��ȣ�" + smallImgList.Length);
        //Debug.Log("cardImg/pic" + poker.picNo);
        // ��ͼƬ���õ� Image ���
        cardPic.sprite = smallImgList[poker.cardNo];
        
    }
}
