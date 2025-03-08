using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardSumCount : MonoBehaviour
{
    public static DiscardSumCount Instance;
    public TextMeshPro tmp;
    public bool hasDiamondNecklace = false;
    public bool hasHeartNecklace = false;
    public bool hasAverageDice = false;
    private void Awake()
    {
        Instance = this;
        tmp = GetComponent<TextMeshPro>();
        Unactivate();
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Unactivate()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        int sum = 0;
        List<GameObject> ls = OptionalBox.Instance.SelectedPokerList;

        hasDiamondNecklace = false;
        hasHeartNecklace = false;
        hasAverageDice = false;
        if (GameObject.Find("HeartNecklace(Clone)") != null) hasHeartNecklace = true;
        if (GameObject.Find("DiamondNecklace(Clone)") != null) hasDiamondNecklace = true;
        if (GameObject.Find("AverageDice(Clone)") != null) hasAverageDice= true;

        for (int i = 0; i < ls.Count; i++)
        {
            Poker pokerScript = ls[i].GetComponent<Poker>();
            if(hasAverageDice == true && pokerScript.cardNumber <=6 )sum += (pokerScript.cardNumber+1);
            else sum += pokerScript.cardNumber; 
            if (pokerScript.suit == cardSuit.Hearts || pokerScript.suit == cardSuit.Diamonds)
            {
                if (pokerScript.suit == cardSuit.Hearts && hasHeartNecklace) sum += 1;
                else if (pokerScript.suit == cardSuit.Diamonds && hasDiamondNecklace) sum += 1;
            }
        }
        tmp.text = "ÀÛ¼ÆÒÑÑ¡£º" + sum.ToString();
    }
}
