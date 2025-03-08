using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemBoard : MonoBehaviour
{
    //挂载于游戏场景的UI 宝石面板物体上
    public static GemBoard Instance;
    public TextMeshProUGUI GemText;
    private int gemCount;

    private void Awake()
    {
        Instance = this;
    }
    public int GetCurrentGem()
    {
        return gemCount;
    }
    public void LoseGem(int num)
    {
        gemCount -= num;
        GemText.text = gemCount.ToString();
    }
    public void AddGem(int num)
    {
        gemCount += num;
        GemText.text = gemCount.ToString();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) 
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (Input.GetKey(KeyCode.M))
                {
                    AddGem(300);
                }
            }
        }
    }
}
