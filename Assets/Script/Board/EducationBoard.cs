using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducationBoard : MonoBehaviour
{
    public List<Sprite> PicList = new List<Sprite>();
    public int currentPicNo = 0;
    private static int MaxPicNo = 7;
    public Image img;

    public void NextPic()
    {
        if (currentPicNo == MaxPicNo - 1)
        {
            currentPicNo = 0;   //回到第一张
        }
        else currentPicNo++;
        img.sprite = PicList[currentPicNo];
    }
    public void LastPic()
    {
        if (currentPicNo == 0)
        {
            currentPicNo = MaxPicNo-1;   //回到最后一张
        }
        else currentPicNo--;
        img.sprite = PicList[currentPicNo];
    }
}
