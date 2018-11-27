using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Card
{
    public static Dictionary<string, Card> Cards = new Dictionary<string, Card>();

    public static string[] allCards = new string[]
       {   "2c", "2d", "2h", "2s", "3c", "3d", "3h", "3s", "4c", "4d", "4h", "4s",
           "5c", "5d", "5h", "5s", "6c", "6d", "6h", "6s", "7c", "7d", "7h", "7s",
           "8c", "8d", "8h", "8s", "9c", "9d", "9h", "9s", "10c", "10d", "10h",
           "10s", "Ac", "Ad", "Ah", "As", "Jc", "Jd", "Jh", "Js", "Kc", "Kd",
           "Kh", "Ks", "Qc", "Qd", "Qh", "Qs"
       };

    public static void LoadCards()
    {
        foreach (string s in allCards)
        {
            char suit = s.Last();

            CardSuit tempSuit = CardSuit.Heart;

            switch (suit)
            {
                case 'd':
                    tempSuit = CardSuit.Diamond;
                    break;
                case 'c':
                    tempSuit = CardSuit.Clubs;
                    break;                  
                case 's':
                    tempSuit = CardSuit.Spades;
                    break;
            }

            string value = s.Remove(s.Length - 1);

            int den = 1;

            switch (value)
            {
                case "A":
                    den = 1;
                    break;
                case "J":
                    den = 11;
                    break;
                case "K":
                    den = 13;
                    break;
                case "Q":
                    den = 12;
                    break;
                default:
                    den = int.Parse(value);
                    break;
            }

            Cards.Add(s, new Card(tempSuit, den, Resources.Load<Sprite>("Cards/" + s)));
        }
    }

    public static List<Card> GetDeck()
    {
        List<Card> deck = new List<Card>();

        foreach (string card in allCards)
        {
            deck.Add(Cards[card]);
        }

        return deck;
    }

    public static List<Card> Shuffle(List<Card> cards)
    {
        System.Random rng = new System.Random();

        int n = cards.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            Card temp = cards[n];
            cards[n] = cards[k];
            cards[k] = temp;
        }

        return cards;
    }

    public enum CardSuit
    {
        Heart = 0,
        Diamond = 1,
        Clubs = 2,
        Spades = 3
    }

    public int id;
    public int denomination;
    public CardSuit suit;

    public Sprite cardImage;

    public Card(CardSuit suit, int denomination, Sprite cardImage)
    {
        this.suit = suit;
        this.denomination = denomination;
        this.cardImage = cardImage;
        id = (int)suit * 13 + denomination - 1;
    }
}