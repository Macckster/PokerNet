using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualization
{
    static class Extensions
    {
        public static List<Card> Shuffle(this List<Card> d)
        {
            Random rng = new Random();

            int n = d.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = d[k];
                d[k] = d[n];
                d[n] = value;
            }

            return d;
        }

        public static Card Draw(this List<Card> d)
        {
            Card c = d[0];

            d.Remove(c);
            return c;
        }

        public static Card ToMarcusCard(this cardOddsSimulator.FitnessFunction.Card c)
        {
            return new Card((Card.CardSuit)c.suit, c.denomination);
        }
    }
}
