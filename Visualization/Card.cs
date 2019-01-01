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
                AllCards.Add(name, new Card(new Bitmap(s), name));
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
    }

    class Card
    {
        public Bitmap Image;
        private string name;

        public Card(Bitmap image, string name) : this(image)
        {
            this.name = name;
        }

        public Card(Bitmap image)
        {
            Image = image;
        }

        public Card()
        {

        }

        public static implicit operator Image(Card v) { return v.Image; }
    }
}
