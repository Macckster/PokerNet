using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Genetic2DAlgorithm;
using PokerNet;

namespace cardOddsSimulator
{
    public class FitnessFunction
    {
        public enum CardSuit
        {
            Heart = 0,//0
            Diamond = 1,//1
            Clubs = 2,//2
            Spades = 3//3
        }

        public class Card
        {
            public int id;
            public int denomination;
            public CardSuit suit;
            public Card(CardSuit Suit, int denomination)
            {
                suit = Suit;
                this.denomination = denomination;
                id = (int)Suit * 13 + denomination - 1;
            }

            public override string ToString()
            {
                if (suit == CardSuit.Heart)
                    return "H" + denomination;
                if (suit == CardSuit.Clubs)
                    return "C" + denomination;
                if (suit == CardSuit.Spades)
                    return "S" + denomination;
                if (suit == CardSuit.Diamond)
                    return "D" + denomination;
                return null;
            }
        }

        //public static Card[] deck;
        public static int players = 4;
        public static int games = 100;

        public static byte[][] knownCards2Chance;
        public static byte[][][][][] knownCards5Chance;
        public static byte[][][][][][] knownCards6Chance;
        public static byte[][][][][][][] knownCards7Chance;

        public static void InstantiateDynamicArrays()
        {
            knownCards2Chance = new byte[51][];
            for (int i = 0; i < knownCards2Chance.Length; i++)
            {
                knownCards2Chance[i] = new byte[51 - i];
            }

            knownCards5Chance = new byte[48][][][][];
            for (int i = 0; i < knownCards5Chance.Length; i++)
            {
                knownCards5Chance[i] = new byte[48 - i][][][];
                for (int j = 0; j < knownCards5Chance[i].Length; j++)
                {
                    knownCards5Chance[i][j] = new byte[48 - i - j][][];
                    for (int k = 0; k < knownCards5Chance[i][j].Length; k++)
                    {
                        knownCards5Chance[i][j][k] = new byte[48 - i - j - k][];
                        for (int l = 0; l < knownCards5Chance[i][j][k].Length; l++)
                        {
                            knownCards5Chance[i][j][k][l] = new byte[48 - i - j - k - l];
                        }
                    }
                }
            }

            int d = 0;
            knownCards6Chance = new byte[47][][][][][];
            for (int i = 0; i < knownCards6Chance.Length; i++)
            {
                knownCards6Chance[i] = new byte[47 - i][][][][];
                for (int j = 0; j < knownCards6Chance[i].Length; j++)
                {
                    knownCards6Chance[i][j] = new byte[47 - i - j][][][];
                    for (int k = 0; k < knownCards6Chance[i][j].Length; k++)
                    {
                        knownCards6Chance[i][j][k] = new byte[47 - i - j - k][][];
                        for (int l = 0; l < knownCards6Chance[i][j][k].Length; l++)
                        {
                            knownCards6Chance[i][j][k][l] = new byte[47 - i - j - k - l][];
                            for (int m = 0; m < knownCards6Chance[i][j][k][l].Length; m++)
                            {
                                knownCards6Chance[i][j][k][l][m] = new byte[47 - i - j - k - l - m];

                            }
                        }
                    }
                }
            }

            knownCards7Chance = new byte[46][][][][][][];
            for (int i = 0; i < knownCards7Chance.Length; i++)
            {
                knownCards7Chance[i] = new byte[46 - i][][][][][];
                for (int j = 0; j < knownCards7Chance[i].Length; j++)
                {
                    knownCards7Chance[i][j] = new byte[46 - i - j][][][][];
                    for (int k = 0; k < knownCards7Chance[i][j].Length; k++)
                    {
                        knownCards7Chance[i][j][k] = new byte[46 - i - j - k][][][];
                        for (int l = 0; l < knownCards7Chance[i][j][k].Length; l++)
                        {
                            knownCards7Chance[i][j][k][l] = new byte[46 - i - j - k - l][][];
                            for (int m = 0; m < knownCards7Chance[i][j][k][l].Length; m++)
                            {
                                knownCards7Chance[i][j][k][l][m] = new byte[46 - i - j - k - l - m][];
                                for (int n = 0; n < knownCards7Chance[i][j][k][l][m].Length; n++)
                                {
                                    knownCards7Chance[i][j][k][l][m][n] = new byte[46 - i - j - k - l - m - n];
                                    d += knownCards7Chance[i][j][k][l][m][n].Length;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static int[] PlayRounds(double[][][] networks)
        {

            List<Card> board = new List<Card>();
            List<Card> deck = CreateDeckList();
            

            Card[][] playerCards = new Card[4][];

            int moneyPool = 0;

            int[] playerBalance = { 100, 100, 100, 100 };
            int[] playerBet = new int[4];

            int playerCount = 4;

            int dealer = 0;
            int bigBlindBet = 0;

            int LastRaiseIndex = 0;

            int[] playerIndexArray = { 0, 1, 2, 3 };

            int highestCurrentBet;
            bool allowHigherBet = true;

            int bet = 0;

            int PlayerIndex = 0;

            int i;
            int j;

            for (int h = 0; h < 10; h++)
            {
                playerCount = 4;
                deck = CreateDeckList();
                board.Clear();
                dealer++;
                allowHigherBet = true;
                playerBet = new int[playerBet.Length];
                moneyPool = 0;
                playerIndexArray = GetPlayers(playerBalance);

                bigBlindBet = Math.Min(playerBalance[playerIndexArray[(dealer + 2) % playerIndexArray.Length]], 2);
                playerBet[playerIndexArray[(dealer + 2) % playerIndexArray.Length]] = bigBlindBet;
                playerBet[playerIndexArray[(dealer + 1) % playerIndexArray.Length]] = 1;
                moneyPool += bigBlindBet + 1;

                playerBalance[playerIndexArray[(dealer + 1) % playerIndexArray.Length]] -= 1;
                playerBalance[playerIndexArray[(dealer + 2) % playerIndexArray.Length]] -= bigBlindBet;

                highestCurrentBet = bigBlindBet;

                for (int k = 0; k < 4; k++)
                {
                    playerCards[k] = new Card[2];
                    playerCards[k][0] = DrawCardFormDeckList(ref deck);
                    playerCards[k][1] = DrawCardFormDeckList(ref deck);
                }

                int minBalance = 100000;
                for (int b = 0; b < playerBalance.Length; b++)
                {
                    minBalance = Math.Min(playerBalance[b], minBalance);
                }

                //Round for loop
                for (i = 0; i < 4; i++)
                {
                    if (playerCount == 1)
                        break;
                    LastRaiseIndex = (dealer + 2) % playerIndexArray.Length;
                    if (i == 1)
                    {
                        for (int g = 0; g < 3; g++)
                        {
                            board.Add(DrawCardFormDeckList(ref deck));
                        }
                    }
                    else if (i == 2)
                    {
                        board.Add(DrawCardFormDeckList(ref deck));
                    }
                    else if (i == 3)
                    {
                        board.Add(DrawCardFormDeckList(ref deck));
                    }
                    bool ignoreLastRaiseIndex = true;
                    for (j = (dealer + 3) % playerIndexArray.Length; true; j++)
                    {
                        PlayerIndex = j % playerIndexArray.Length;

                        if (LastRaiseIndex == PlayerIndex)
                            if (!ignoreLastRaiseIndex)
                                break;
                            else
                            {
                                ignoreLastRaiseIndex = false;
                                LastRaiseIndex = (dealer + 3) % playerIndexArray.Length;
                            }

                        if (playerBet[playerIndexArray[PlayerIndex]] != -1)
                        {
                            //Get bet from index
                            bet = GetBet(
                                networks[playerIndexArray[PlayerIndex]], playerBalance[playerIndexArray[PlayerIndex]],
                                board.ToArray(), playerCards[playerIndexArray[PlayerIndex]],
                                playerBet[playerIndexArray[PlayerIndex]],
                                playerCount,
                                i,
                                highestCurrentBet);
                            bet = Math.Min(bet, minBalance);
                            if (bet <= playerBalance[playerIndexArray[PlayerIndex]])
                            {
                                if (!allowHigherBet)
                                    bet = highestCurrentBet;

                                if (bet >= highestCurrentBet)//GRAAAGAGAH!!!!
                                {
                                    moneyPool += bet - playerBet[playerIndexArray[PlayerIndex]];
                                    playerBalance[playerIndexArray[PlayerIndex]] -= bet - playerBet[playerIndexArray[PlayerIndex]];//FIXXXXX!
                                    playerBet[playerIndexArray[PlayerIndex]] = bet;
                                    allowHigherBet = (playerBalance[playerIndexArray[PlayerIndex]] != 0);
                                    if (bet > highestCurrentBet)
                                    {
                                        highestCurrentBet = bet;
                                        LastRaiseIndex = PlayerIndex;
                                        ignoreLastRaiseIndex = false;
                                    }
                                }
                                else
                                {
                                    playerBet[playerIndexArray[PlayerIndex]] = -1;
                                    playerCount--;
                                }

                            }
                            else
                            {
                                playerBet[playerIndexArray[PlayerIndex]] = -1;
                                playerCount--;
                            }
                        }
                    }
                }

                List<int> indexToGive = new List<int>{0};
                int bestScore = -1;
                for (int k = 0; k < playerIndexArray.Length; k++)
                {
                    if(playerBet[playerIndexArray[k]] != -1)
                    {
                        List<Card> plaCards = new List<Card>();
                        plaCards.AddRange(board);
                        plaCards.AddRange(playerCards[playerIndexArray[k]]);
                        int fitness = Fitness(plaCards.ToArray());
                        if (fitness > bestScore)
                        {
                            indexToGive.Clear();
                            indexToGive.Add(playerIndexArray[k]);
                            bestScore = fitness;
                        }
                        else if (fitness == bestScore)
                            indexToGive.Add(playerIndexArray[k]);
                    }
                }

                foreach (var item in indexToGive)
                {
                    playerBalance[item] += moneyPool / indexToGive.Count;
                }
            }

            return playerBalance;
        }

        public static int GetBet(double[][] player, int balance, Card[] b, Card[] p, int bet, int playerCount, int round, int highBet)
        {
            List<Card> cards = new List<Card>(b);
            cards.AddRange(p);
            cards.Sort(delegate (Card c1, Card c2) { return c1.id - c2.id; });
            double winChance = ChanceOfWin(cards.ToArray());
            int balanceBeforeBet = balance + bet;
            double[] inputs = new double[5];
            inputs[0] = (double)bet / balanceBeforeBet;
            inputs[1] = (double)highBet / balanceBeforeBet;
            inputs[2] = winChance;
            inputs[3] = playerCount;
            inputs[4] = round;
            //bool isPlayer = true;
            //for (int i = 0; i < player.Length / 2; i++)
            //{
            //    if (player[i][0] != 0)
            //        isPlayer = false;
            //}
            //if(isPlayer)
            //    return int.Parse(Console.ReadLine());
            //else
            //{

            //}
            return (int)(NeuralNet.FeedForward(inputs, player)[0] * balanceBeforeBet);
        }

        public static double ChanceOfWin(Card[] cards)
        {
            int i = 0;
            if (cards.Length == 2)
                i = knownCards2Chance[cards[0].id][cards[1].id - cards[0].id - 1];
            else if (cards.Length == 5)
                i = knownCards5Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1];
            else if (cards.Length == 6)
                i = knownCards6Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1][cards[5].id - cards[4].id - 1];
            else if (cards.Length == 7)
                i = knownCards7Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1][cards[5].id - cards[4].id - 1][cards[6].id - cards[5].id - 1];
            if(i - 1 == -1)
            {
                byte wins = (byte)(SimulateChanceOfWinning(cards) + 1);
                if (cards.Length == 2)
                    knownCards2Chance[cards[0].id][cards[1].id - cards[0].id - 1] = wins;
                else if (cards.Length == 5)
                    knownCards5Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1] = wins;
                else if (cards.Length == 6)
                    knownCards6Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1][cards[5].id - cards[4].id - 1] = wins;
                else if (cards.Length == 7)
                    knownCards7Chance[cards[0].id][cards[1].id - cards[0].id - 1][cards[2].id - cards[1].id - 1][cards[3].id - cards[2].id - 1][cards[4].id - cards[3].id - 1][cards[5].id - cards[4].id - 1][cards[6].id - cards[5].id - 1] = wins;
                return (double)(wins - 1) / games;
            }
            return (double)(i - 1) / games;
        }

        static int[] GetPlayers(int[] playerBalance)
        {
            List<int> players = new List<int>();

            for (int i = 0; i < playerBalance.Length; i++)
            {
                if (playerBalance[i] > 0)
                {
                    players.Add(i);
                }
            }

            return players.ToArray();
        }

        static string GetNameOfCardArrayFirst(Card[] cards)
        {
            return cards[0].ToString() + cards[1].ToString();
        }

        static string GetNameOfCardArray(Card[] cards)
        {
            string s = "";
            foreach (Card card in cards)
            {
                s += card.ToString();
            }
            return s;
        }

        public static int SimulateChanceOfWinning(Card[] cards)
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

            for (int i = 0; i < games; i++)
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
            int r = StaticRandom.Rand(0, deck.Count);
            Card c = deck[r];
            deck.RemoveAt(r);
            return c;
        }

        static bool DoubleCheck(Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                for (int j = 0; j < cards.Length; j++)
                {
                    if (j != i)
                        if (cards[i].id == cards[j].id)
                            return false;
                }
            }
            if (cards[0].id >= cards[1].id)
                return false;
            for (int i = 3; i < cards.Length; i++)
            {
                if (cards[i - 1].id >= cards[i].id)
                    return false;
            }
            return true;
        }

        static Card[] CreateDeck()
        {
            Card[] deck = new Card[52];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    Card newCard = new Card((CardSuit)i, j);
                    deck[newCard.id] = newCard;
                }
            }
            return deck;
        }

        static List<Card> CreateDeckList()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    Card newCard = new Card((CardSuit)i, j);
                    deck.Add(newCard);
                }
            }
            return deck;
        }

        static List<Card> CreateDeckList(Card[] cards)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    Card newCard = new Card((CardSuit)i, j);
                    bool add = true;
                    for (int k = 0; k < cards.Length; k++)
                    {
                        if (newCard.id == cards[k].id)
                            add = false;
                    }
                    if (add)
                        deck.Add(newCard);
                }
            }
            return deck;
        }

        /// <summary>
        /// Calculate the fitness of a set of cards
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
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