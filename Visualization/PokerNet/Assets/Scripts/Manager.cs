using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour {

    // Use this for initialization

    public TMP_Text currentBet;

    int bet = 0;

    GameObject PlayerC1, PlayerC2, BoardC1, BoardC2, BoardC3, BoardC4, BoardC5;

	void Start ()
    {
        Card.LoadCards();
        UpdatePlayerHand(Card.Cards["2c"], Card.Cards["Ks"]);
        InitialFlip(Card.Cards["2d"], Card.Cards["Kd"], Card.Cards["Qd"]);

        PlayerC1 = GameObject.Find("Player Card 1");
        PlayerC2 = GameObject.Find("Player Card 2");

        BoardC1 = GameObject.Find("Board Card 1");
        BoardC2 = GameObject.Find("Board Card 2");
        BoardC3 = GameObject.Find("Board Card 3");
        BoardC4 = GameObject.Find("Board Card 4");
        BoardC5 = GameObject.Find("Board Card 5");
    }

    public void UpdatePlayerHand(Card a, Card b)
    {
        PlayerC1.GetComponent<SpriteRenderer>().sprite = a.cardImage;
        PlayerC2.GetComponent<SpriteRenderer>().sprite = b.cardImage;
    }

    public void InitialFlip(Card a, Card b, Card c)
    {
        BoardC1.GetComponent<SpriteRenderer>().sprite = a.cardImage;
        BoardC2.GetComponent<SpriteRenderer>().sprite = b.cardImage;
        BoardC3.GetComponent<SpriteRenderer>().sprite = c.cardImage;
    }

    public void SecondFlip(Card a)
    {
        BoardC4.GetComponent<SpriteRenderer>().sprite = a.cardImage;
    }

    public void ThirdFlip(Card a)
    {
        BoardC5.GetComponent<SpriteRenderer>().sprite = a.cardImage;
    }

    public void Clear()
    {
        bet = 0;

        PlayerC1.GetComponent<SpriteRenderer>().sprite = null;
        PlayerC2.GetComponent<SpriteRenderer>().sprite = null;

        BoardC1.GetComponent<SpriteRenderer>().sprite = null;
        BoardC2.GetComponent<SpriteRenderer>().sprite = null;
        BoardC3.GetComponent<SpriteRenderer>().sprite = null;
        BoardC4.GetComponent<SpriteRenderer>().sprite = null;
        BoardC5.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void SetBetText()
    {
        currentBet.text = "Current Bet \n" + bet;
    }

    public void UpdateBet(int adder)
    {
        bet += adder;
        if(bet < 0)
        {
            bet = 0;
        }

        SetBetText();
    }
}