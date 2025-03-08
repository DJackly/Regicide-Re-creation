using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance;
    //����LoadingUICanvas�ϣ����Ʊ���ͼ�ĵ��뵭��
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
        if (targetAlpha != 0) //���ֹ���UI
        {
            //UI.blocksRaycasts = true;
            //BackGround.SetActive(true);
            while (!Mathf.Approximately(UI.alpha, targetAlpha))
            {
                UI.alpha = Mathf.MoveTowards(UI.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
        }
        else  //��������UI
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
