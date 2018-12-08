using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour {

    public TMP_Text currentBet;

    int bet = 0;

    GameObject PlayerC1, PlayerC2, BoardC1, BoardC2, BoardC3, BoardC4, BoardC5;

    int round = 0;

    List<Card> Deck;
    List<Player> players = new List<Player>(4);

    public int bigblindIndex = 0;

    public bool wait = true;

    public int lastRaiseIndex = -1;

    void Start ()
    {
        Card.LoadCards();

        Deck = Card.GetDeck();

        PlayerC1 = GameObject.Find("Player Card 1");
        PlayerC2 = GameObject.Find("Player Card 2");

        BoardC1 = GameObject.Find("Board Card 1");
        BoardC2 = GameObject.Find("Board Card 2");
        BoardC3 = GameObject.Find("Board Card 3");
        BoardC4 = GameObject.Find("Board Card 4");
        BoardC5 = GameObject.Find("Board Card 5");

        players.Add(new Player());
        players.Add(new Player());
        players.Add(new Player());
        players.Add(new Player());

        NextRound();
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

    public void NextRound()
    {
        if(round == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Card a = Deck.DrawCard();
                Card b = Deck.DrawCard();

                players[i].a = a;
                players[i].b = b;
                players[i].balance = 100;

                if(i == 0)
                {
                    UpdatePlayerHand(a,b);
                }
            }
        }

        StartCoroutine(GetBets((bigblindIndex + 1) % 4));
    }

    public IEnumerator GetBets(int startPlayer)
    {
        if(lastRaiseIndex == startPlayer)
        {
            yield return null;
        }

        if (players[startPlayer].fold)
        {
            yield return StartCoroutine(GetBets((startPlayer + 1) % 4));
        }

        if (startPlayer == 0)
        {
            //Human player
            while (wait)
            {
                yield return new WaitForFixedUpdate();
            }

            players[startPlayer].bet = bet;
        }
        else
        {
            players[startPlayer].bet = 5;
        }

        if (players[lastRaiseIndex].bet < players[startPlayer].bet)
        {
            lastRaiseIndex = startPlayer;
        }

        yield return StartCoroutine(GetBets((startPlayer + 1) % 4));
    }

    public void SetWait(bool b)
    {
        wait = b;
    }

    class Player
    {
        public int balance;
        public bool fold;
        public int bet;

        public Card a, b;
    }
}