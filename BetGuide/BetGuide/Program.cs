using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PokerNet;

namespace BetGuide
{
    class Program
    {
        static int gameAmount = 500;
        static Random StaticRandom = new Random();

        static void Main(string[] args)
        {
            string cards = Query("Enter your cards");

            Card[] c = Card.fromStringArr(cards.Split());

            int balance = int.Parse(Query("Enter your balance"));
            int currentBet = int.Parse(Query("Enter your current bet"));
            int requiredBet = int.Parse(Query("Enter the current required bet"));
            int remainingPlayers = int.Parse(Query("Enter amount of players left"));
            int roundNumber = int.Parse(Query("Enter round number"));

            double[] input = { (double)currentBet / balance, (double)requiredBet / balance, (double)SimulateChanceOfWinning(Card.fromStringArr(cards.Split())) / 500, remainingPlayers, roundNumber};
            double[][] weights = GetWeights("network.xml");

            double res = (NeuralNet.FeedForward(input, weights)[0]);

            Console.WriteLine(res * balance);
            Console.ReadKey();
        }

        static string Query(string q)
        {
            Console.WriteLine(q);
            return Console.ReadLine();
        }

        static double[][] GetWeights(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(double[][]));

            StreamReader reader = new StreamReader(path);
            double[][] ret = (double[][])serializer.Deserialize(reader);
            reader.Close();
            return ret;
        }

        static List<Card> CreateDeckList(Card[] cards)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    Card newCard = new Card((Card.CardSuit)i, j);
                    deck.Add(newCard);
                }
            }

            foreach (Card c in cards)
            {
                deck.Remove(c);
            }

            return deck;
        }

        static int SimulateChanceOfWinning(Card[] cards)
        {
            int wins = 0;
            Card[][] players = new Card[4][];
            Card[] board = new Card[5];
            for (int i = 2; i < cards.Length; i++)
            {
                board[i - 2] = cards[i];
            }
            for (int i = 0; i < 4; i++)
            {
                players[i] = new Card[2];
            }
            players[0][0] = cards[0];
            players[0][1] = cards[1];

            for (int i = 0; i < gameAmount; i++)
            {
                List<Card> deck = CreateDeckList(cards);

                for (int j = cards.Length - 2; j < board.Length; j++)
                {
                    board[j] = DrawCardFormDeckList(ref deck);
                }

                for (int j = 1; j < 4; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        players[j][k] = DrawCardFormDeckList(ref deck);
                    }
                }

                int[] points = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    Card[] c = new Card[7];
                    for (int k = 2; k < c.Length; k++)
                    {
                        c[k] = board[k - 2];
                    }
                    c[0] = players[j][0];
                    c[1] = players[j][1];
                    int d = Fitness(c);
                    points[j] = Fitness(c);
                }
                bool win = true;
                for (int j = 1; j < players.Length; j++)
                {
                    if (points[j] > points[0])
                        win = false;
                    //if (points[j] == points[0])
                    //     Console.Write("Tjo");
                }
                if (win)
                    wins++;
            }
            return wins;// / games;
        }

        static Card DrawCardFormDeckList(ref List<Card> deck)
        {
            int r = StaticRandom.Next(0, deck.Count);
            Card c = deck[r];
            deck.RemoveAt(r);
            return c;
        }

        static int Fitness(Card[] cards)
        {
            int highestCard = GetHighestCard(cards);

            if (RoyalFlush(cards))
            {
                return 90000;
            }

            Tuple<bool, int> sFlush = StraightFlush(cards);

            if (sFlush.Item1)
            {
                return 80000 + 100 * sFlush.Item2;
            }

            if (FourOFaKind(cards))
            {
                return 70000 + highestCard;
            }

            Tuple<bool, int> three = ThreeOFaKind(cards);
            Tuple<bool, int> pair = Pair(cards);

            if (three.Item1 && pair.Item1) //if it has full house
            {
                return 60000 + 100 * three.Item2 + 10 * pair.Item2;
            }

            Tuple<bool, int> flush = Flush(cards);

            if (flush.Item1)
            {
                return 50000 + 10 * flush.Item2;
            }

            Tuple<bool, int> straight = Straight(cards);

            if (straight.Item1)
            {
                return 40000 + 10 * straight.Item2;
            }

            if (three.Item1)
            {
                return 30000 + 100 * three.Item2 + highestCard;
            }

            Tuple<bool, int, int> twoPair = TwoPair(cards);

            if (twoPair.Item1)
            {
                return 20000 + twoPair.Item2 * 100 + twoPair.Item3 * 10 + highestCard;
            }

            if (pair.Item1)
            {
                return 10000 + 100 * pair.Item2 + highestCard;
            }

            return highestCard;
        }

        /// <summary>
        /// Return the highest card in the set
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        static int GetHighestCard(Card[] cards)
        {
            int highest = 2;

            foreach (Card c in cards)
            {
                //Ace can be 1 or 14, in this case it wil always be 14
                if (c.denomination == 1)
                {
                    return 14;
                }
                else
                {
                    if (c.denomination > highest)
                    {
                        highest = c.denomination;
                    }
                }
            }

            return highest;
        }

        static bool RoyalFlush(Card[] cards)
        {
            for (int i = 0; i < 4; i++) //Run once for each suit
            {
                if (cards.Any(x => x.denomination == 1 && (int)x.suit == i)) //If there is an ace
                    if (cards.Any(x => x.denomination == 13 && (int)x.suit == i)) //If there is a king
                        if (cards.Any(x => x.denomination == 12 && (int)x.suit == i)) //Ifthere is a queen
                            if (cards.Any(x => x.denomination == 11 && (int)x.suit == i)) //If there is a jack
                                if (cards.Any(x => x.denomination == 10 && (int)x.suit == i)) //if there is a 10
                                {
                                    return true; //Yes we have royal flush
                                }
            }


            return false;
        }

        static Tuple<bool, int> StraightFlush(Card[] cards)
        {
            for (int i = 0; i < 4; i++) //Run once for each suit
            {
                for (int j = 14; j >= 5; j--) //Run for each denomination
                {
                    int k = j == 14 ? 1 : j;

                    if (cards.Any(x => x.denomination == k && (int)x.suit == i))
                        if (cards.Any(x => x.denomination == j - 1 && (int)x.suit == i))
                            if (cards.Any(x => x.denomination == j - 2 && (int)x.suit == i))
                                if (cards.Any(x => x.denomination == j - 3 && (int)x.suit == i))
                                    if (cards.Any(x => x.denomination == j - 4 && (int)x.suit == i))
                                    {
                                        return Tuple.Create(true, j - 4);
                                    }
                }
            }
            return Tuple.Create(false, 0);
        }

        static Tuple<bool, int> Straight(Card[] cards)
        {
            for (int i = 14; i >= 5; i--) //Run for each card
            {
                int k = i == 14 ? 1 : i;

                if (i == 4)
                {

                }

                if (cards.Any(x => x.denomination == k))
                    if (cards.Any(x => x.denomination == i - 1))
                        if (cards.Any(x => x.denomination == i - 2))
                            if (cards.Any(x => x.denomination == i - 3))
                                if (cards.Any(x => x.denomination == i - 4))
                                {
                                    return Tuple.Create(true, i - 4);
                                }
            }
            return Tuple.Create(false, 0);
        }

        static Tuple<bool, int> Flush(Card[] cards)
        {
            int highest = 0;

            for (int i = 0; i < 4; i++) //Run for each suit
            {
                int counter = 0;

                foreach (Card c in cards)
                {
                    if ((int)c.suit == i)
                    {
                        if (c.denomination == 1)
                            highest = 14;

                        if (c.denomination > highest)
                        {
                            highest = c.denomination;
                        }

                        counter++;
                    }
                }

                if (counter >= 5) //If five cards have the same suit the we have a flush
                    return Tuple.Create(true, highest);
            }

            return Tuple.Create(false, 0);
        }

        static bool FourOFaKind(Card[] cards)
        {
            for (int i = 13; i >= 1; i--) //Run for each denomination
            {
                int counter = 0;

                foreach (Card c in cards)
                {
                    if (c.denomination == i)
                    {
                        counter++;
                    }
                }

                if (counter >= 4)
                {
                    return true;
                }
            }

            return false;
        }

        static Tuple<bool, int> ThreeOFaKind(Card[] cards)
        {
            for (int i = 13; i >= 1; i--) //Run for each denomination
            {
                int counter = 0;

                foreach (Card c in cards)
                {
                    if (c.denomination == i)
                    {
                        counter++;
                    }
                }

                if (counter == 3)
                {
                    return Tuple.Create(true, i);
                }
            }

            return Tuple.Create(false, 0);
        }

        static Tuple<bool, int, int> TwoPair(Card[] cards)
        {
            int counter = 0;

            int valueA = 0;
            int valueB = 0;

            Card cardA = null;
            Card cardB = null;

            for (int j = 1; j < 13; j++) //Run for each denomination
            {
                cardA = null;
                cardB = null;

                foreach (Card c in cards)
                {
                    if (c.denomination == j)
                    {
                        if (cardA == null)
                            cardA = c;
                        else if (cardB == null)
                        {
                            cardB = c;
                            if (valueA == 0)
                                valueA = cardA.denomination;
                            else if (valueB == 0)
                                valueB = cardA.denomination;

                            counter++;
                        }
                    }
                }
            }

            if (counter == 2) //There is two pairs
            {
                return Tuple.Create(true, valueA, valueB);
            }

            return Tuple.Create(false, 0, 0);
        }

        static Tuple<bool, int> Pair(Card[] cards)
        {
            int counter = 0;
            int value = 0;

            for (int j = 1; j < 13; j++) //Run for each denomination
            {
                Card cardA = null;
                Card cardB = null;

                bool foundPair = false;

                foreach (Card c in cards)
                {
                    if (c.denomination == j)
                    {
                        if (cardA == null)
                            cardA = c;
                        else if (cardB == null)
                        {
                            cardB = c;
                            counter++;
                            foundPair = true;
                        }
                        else
                        {
                            foundPair = false;
                            counter--; //We don't want to count those where there are more than two cards.
                        }
                    }
                }

                if (foundPair)
                {
                    value = cardA.denomination;
                }
            }

            if (counter == 1) //We have a pair
                return Tuple.Create(true, value);

            return Tuple.Create(false, 0);
        }
    }
}
