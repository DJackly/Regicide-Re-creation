using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poker : MonoBehaviour
{
    public int cardNumber;
    public int picNo;  //´óÍ¼±àºÅ
    public int cardNo;  //Ð¡Í¼±àºÅ
    public cardSuit suit;
    public bool isInHand;

    public string cardName;
    public PokerSO pokerSO;

    public void setName()
    {
        cardName = suit.ToString() + cardNumber.ToString();
    }
}

