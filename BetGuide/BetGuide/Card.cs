using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace BetGuide
{
    class Card
    {
        public enum CardSuit
        {
            Heart = 0,//0
            Diamond = 1,//1
            Clubs = 2,//2
            Spades = 3//3
        }

        public int denomination;
        public CardSuit suit;

        public int id { get { return (int)suit * 13 + denomination - 1; }}

        public static Card[] fromStringArr(string[] cards)
        {
            List<Card> cardList = new List<Card>();

            foreach (string s in cards)
            {
                char suitChar = s[0];
                string den = s.Remove(0, 1);

                CardSuit suit = CardSuit.Clubs;

                switch (suitChar)
                {
                    case 'h':
                        suit = Card.CardSuit.Heart;
                        break;
                    case 'd':
                        suit = Card.CardSuit.Diamond;
                        break;
                    case 's':
                        suit = Card.CardSuit.Spades;
                        break;
                }

                int value = 0;

                switch (den.ToUpper())
                {
                    case "A":
                        value = 1;
                        break;
                    case "J":
                        value = 11;
                        break;
                    case "Q":
                        value = 12;
                        break;
                    case "K":
                        value = 13;
                        break;
                    default:
                        value = int.Parse(den);
                        break;
                }

                cardList.Add(new Card(suit, value));
            }

            return cardList.ToArray();
        }

        public Card(CardSuit suit, int denomination)
        {
            this.suit = suit;
            this.denomination = denomination;
        }

        public Card()
        {

        }
    }
}
