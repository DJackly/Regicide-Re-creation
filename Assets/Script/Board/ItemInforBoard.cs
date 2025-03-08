using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInforBoard : MonoBehaviour
{
    public static ItemInforBoard Instance { get; private set; }
    public GameObject Title;
    public GameObject Descr;
    public GameObject Story;
    public GameObject SellPrice;
    public GameObject DetectingBlock; //���ڼ���Ƿ񳬳���Ļ�ұ�Ե
    public bool isBeyondScreen = false;

    private void OnEnable()
    {
        isBeyondScreen = false;
    }
    private void Update()
    {
        if (! JudgmentUiInScreen(DetectingBlock.GetComponent<RectTransform>())) isBeyondScreen = true;
    }
    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowBoard(Item itemScript, bool isActivated)
    {
        gameObject.SetActive(true);
        Title.GetComponent<TextMeshProUGUI>().text = itemScript.itemName;
        Descr.GetComponent<TextMeshProUGUI>().text = itemScript.itemDescr;
        Story.GetComponent<TextMeshProUGUI>().text = itemScript.itemStory;
        if(isActivated) SellPrice.GetComponent<TextMeshProUGUI>().text = "���ۼ۸�" + (itemScript.price/2).ToString();
        else SellPrice.GetComponent<TextMeshProUGUI>().text = "����۸�" + itemScript.price.ToString();
    }
    public void HideBoard()
    {
        gameObject.SetActive(false);
    }

    bool JudgmentUiInScreen(RectTransform rect) //�ж�UI�Ƿ�����Ļ��
    {
        bool isInView;
        Vector3 worldPos = rect.transform.position;
        float leftX = worldPos.x - rect.sizeDelta.x / 2;
        float rightX = worldPos.x + rect.sizeDelta.x / 2;
        if (leftX >= 0 && rightX <= Screen.width)
        {
            isInView = true;
        }
        else isInView = false;
        return isInView;
    }
}
