using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Visualization
{
    class Deck
    {
        public static Dictionary<string, Card> AllCards = new Dictionary<string, Card>();

        public List<Card> cards = new List<Card>();

        public static void Setup()
        {
            string dir = "cards";

            string[] cards = Directory.GetFiles(dir);

            foreach (string s in cards)
            {
                string name = Path.GetFileName(s);
                name = name.Remove(name.Length - 4);

                char suitChar = name[name.Length - 1];
                string den = name.Remove(name.Length - 1);

                Card.CardSuit suit = Card.CardSuit.Clubs;

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

                switch (den)
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

                AllCards.Add(name, new Card(new Bitmap(s), name, value, suit));
            }
        }

        public Card Draw()
        {
            return cards.Draw();
        }

        public static List<Card> GenerateDeck()
        {
            List<Card> cards = AllCards.Values.ToList();

            return cards.Shuffle();
        }

        public static List<Card> GenerateDeck(Card[] remcards)
        {
            List<Card> cards = AllCards.Values.ToList();

            foreach (Card c in remcards)
            {
                cards.Remove(c);
            }

            return cards.Shuffle();
        }
    }

    class Card
    {
        public enum CardSuit
        {
            Heart = 0,//0
            Diamond = 1,//1
            Clubs = 2,//2
            Spades = 3//3
        }

        public Bitmap Image;
        private string name;

        public int denomination;
        public CardSuit suit;

        public int id { get { return (int)suit * 13 + denomination - 1; }}

        public Card(Bitmap image, string name, int denomination, CardSuit suit) : this(image, name)
        {
            this.denomination = denomination;
            this.suit = suit;
        }

        public Card(Bitmap image, string name) : this(image)
        {
            this.name = name;
        }

        public Card(Bitmap image)
        {
            Image = image;
        }

        public Card(CardSuit suit, int denomination)
        {
            this.suit = suit;
            this.denomination = denomination;
        }

        public Card()
        {

        }

        public static implicit operator Image(Card v) { return v.Image; }
        public static implicit operator string(Card v) { return v.ToString(); }

        public override string ToString()
        {
            return name;
        }
    }
}
