using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PokerSO : ScriptableObject
{
    public int cardNumber;
    public int picNo;
    public int cardNo;
    public cardSuit suit;
    public bool isInHand;


}

[Serializable]
public enum cardSuit
{
    Spades, Hearts, Diamonds, Clubs, Jokers
}