using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance;
    //绑定在LoadingUICanvas上，控制背景图的淡入淡出
    //public GameObject BackGround;
    private CanvasGroup UI;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UI = GetComponent<CanvasGroup>();
    }
    public void StartFading(int alpha)
    {
        StartCoroutine(Fade(alpha));
    }
    IEnumerator Fade(int targetAlpha)
    {
        float time = 1.2f;
        float speed = Mathf.Abs(UI.alpha - targetAlpha) / time;
        if (targetAlpha != 0) //出现过场UI
        {
            //UI.blocksRaycasts = true;
            //BackGround.SetActive(true);
            while (!Mathf.Approximately(UI.alpha, targetAlpha))
            {
                UI.alpha = Mathf.MoveTowards(UI.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
        }
        else  //结束过场UI
        {
            while (!Mathf.Approximately(UI.alpha, targetAlpha))
            {
                UI.alpha = Mathf.MoveTowards(UI.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            //UI.blocksRaycasts = false;
           // BackGround.SetActive(false);
        }
    }
    public void ForceToCloseUI()
    {
        UI.alpha = 0;
        //UI.blocksRaycasts = false;
    }
}
