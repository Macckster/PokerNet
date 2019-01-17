using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Linq;
using PokerNet;
using System.Xml.Serialization;

namespace Visualization
{
    public partial class Form1 : Form
    {
        Deck d = new Deck();
        List<Card[]> PlayerCards = new List<Card[]>
        {
            new Card[2],
            new Card[2],
            new Card[2],
            new Card[2]
        };

        List<Card> CommunityCards = new List<Card>();

        static Random StaticRandom = new Random(Environment.TickCount);

        static int gameAmount = 500;

        string weightsPath = "C:\\GymnasieProjekt\\network.xml";
        double[][] weights;

        int RoundCounter = 0;

        int[] Balance = new int[]
        {
            100,100,100,100
        };

        bool[] folded = new bool[]
        {
            false, false, false, false
        };

        int dealer = 0;
        int lastDealer = -1;

        bool flop, turn, river, betting, compare;

        bool gameWon;

        int startBet = -1;

        int[] bets =
        {
            0,0,0,0
        };

        public Form1()
        {
            weights = GetWeights(weightsPath);
            InitializeComponent();
        }

        private void Testbtn_Click(object sender, EventArgs e)
        {
            d.cards = Deck.GenerateDeck();
            Run();
        }

        void Run()
        {
            flop = turn = river = false;
            PreFlop();
        }

        void PreFlop()
        {
            bets = new int[]
            {
                0,0,0,0
            };

            RoundCounter = 0;

            CommunityCards = new List<Card>();
            folded = new bool[]
            {
                false, false,false,false
            };

            dealer = (lastDealer + 1) % Balance.Length;
            LogClear();
            PlayerCardsLbl.Visible = true;

            Log("Beginning");

            PlayerCards[0][0] = d.Draw();
            PlayerCards[0][1] = d.Draw();

            PlayerCards[1][0] = d.Draw();
            PlayerCards[1][1] = d.Draw();

            PlayerCards[2][0] = d.Draw();
            PlayerCards[2][1] = d.Draw();

            PlayerCards[3][0] = d.Draw();
            PlayerCards[3][1] = d.Draw();

            PlayerCardOne.Image = PlayerCards[0][0];
            PlayerCardTwo.Image = PlayerCards[0][1];

            Log("Dealer is Player Nr " + dealer + " ({0})", dealer == 0 ? "You" : "AI");

            int littleBlind = (dealer + 1) % Balance.Length;

            Log("Little Blind is player Nr " + littleBlind + " ({0})", littleBlind == 0 ? "You" : "AI");

            int bigBlind = (dealer + 2) % Balance.Length;

            Log("Big Blind is player Nr " + bigBlind + " ({0})", bigBlind == 0 ? "You" : "AI");

            Log("Little Blind and Big Blind play their starting bets");

            Balance[littleBlind] -= 1;
            Balance[bigBlind] -= 2;
            bets[littleBlind] += 1;
            bets[bigBlind] += 2;

            UpdatePlayerVisuals();

            int FirstPlayer = (bigBlind + 1) % Balance.Length;

            Log("First Betting Round starting on player {0} ({1})", FirstPlayer, FirstPlayer == 0 ? "You" : "AI");

            flop = turn = river = betting = false;
            flop = true;
            betting = true;

            startBet = FirstPlayer;

            Next.Visible = true;
        }

        void Flop()
        {
            RoundCounter = 1;
            Log("Dealer Flips First Three Community Cards");

            ComCardsLbl.Visible = true;

            CommunityCards.Add(d.Draw());
            CommunityCards.Add(d.Draw());
            CommunityCards.Add(d.Draw());

            CommunityOne.Image = CommunityCards[0];
            CommunityTwo.Image = CommunityCards[1];
            CommunityThree.Image = CommunityCards[2];

            Log("Second Betting Round");

            flop = turn = river = betting = false;
            turn = true;
            betting = true;
        }

        void Turn()
        {
            RoundCounter = 2;
            Log("Dealer Flips Fourth Community Card");

            CommunityCards.Add(d.Draw());
            CommunityFour.Image = CommunityCards[3];

            Log("Third Betting Round");

            flop = turn = river = betting = false;
            river = true;
            betting = true;
        }

        void River()
        {
            RoundCounter = 3;
            Log("Dealer Flips Last Community Card");

            CommunityCards.Add(d.Draw());
            CommunityFive.Image = CommunityCards[3];

            Log("Last Betting Round");

            flop = turn = river = betting = false;
            betting = true;
            compare = true;
        }

        public void BettingRound(int a)
        {
            int lastRaiseIndex = a;

            bool first = true;

            if (bets[(a - 1) % Balance.Length] == 2)
            {
                lastRaiseIndex = (a - 1) % Balance.Length;
            }

            for (int i = a; true; i = (i + 1) % Balance.Length)
            {
                if (lastRaiseIndex == i && !first)
                {
                    Log("Betting Round Over");
                    return;
                }

                first = false;

                if (folded[i])
                {
                    continue;
                }

                int targetBet = Highest(bets);

                UpdatePlayerVisuals();

                int playerbet = 0;

                if (i == 0)
                {
                    playerbet = GetPlayerBet();

                    while (playerbet > Balance[0])
                    {
                        playerbet = GetPlayerBet();
                    }
                }
                else
                {
                    int counter = 0;
                    for (int j = 0; j < folded.Length; j++)
                    {
                        if (!folded[j])
                        {
                            counter++;
                        }
                    }
                    double[] inputs = new double[] { (double)bets[i] / Balance[i], (double)targetBet / Balance[i], SimulateChanceOfWinning(PlayerCards[i].Concat(CommunityCards).ToArray()) / 500, counter, RoundCounter };
                    var k = NeuralNet.FeedForward(inputs, weights);
                    playerbet = (int)NeuralNet.FeedForward(inputs, weights)[0];
                }

                if (playerbet == -1)
                {
                    folded[i] = true;
                    Log("Player {0} ({1}) folded", i, i == 0 ? "You" : "AI");
                    continue;
                }

                bets[i] += playerbet;
                Balance[i] -= playerbet;

                if (bets[i] > targetBet)
                {
                    Log("Player {0} ({1}) raised the bet to {2}", i, i == 0 ? "You" : "AI", bets[i]);
                    lastRaiseIndex = i;
                }

                if (bets[i] == targetBet)
                {
                    Log("Player {0} ({1}) bet {2}", i, i == 0 ? "You" : "AI", playerbet);
                }

                if (CheckFolded())
                {
                    int index = Array.IndexOf(folded, true);
                    Log("Player {0} ({1}) has won!", index, index == 0 ? "You" : "AI");

                    Balance[index] += GetBetSum(index);

                    gameWon = true;
                }
            }
        }

        int GetBetSum(int excludor)
        {
            int sum = 0;

            for (int i = 0; i < bets.Length; i++)
            {
                if (i != excludor)
                {
                    sum += bets[i];
                }
            }

            return sum;
        }

        bool CheckFolded()
        {
            int counter = 0;

            for (int i = 0; i < folded.Length; i++)
            {
                if (folded[i])
                {
                    counter++;
                }
            }

            return counter == 3;
        }

        int Highest(int[] arr)
        {
            int highest = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > highest)
                {
                    highest = arr[i];
                }
            }

            return highest;
        }

        int GetPlayerBet()
        {
            string response = Interaction.InputBox("What do you want do bet? (Cancel to fold)", "User Bet", "");

            if (response == "")
            {
                return -1;
            }

            return int.Parse(response);
        }

        double[][] GetWeights(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(double[][]));

            StreamReader reader = new StreamReader(path);
            double[][] ret = (double[][])serializer.Deserialize(reader);
            reader.Close();
            return ret;
        }


        void Log(object message, params object[] obj)
        {
            LogWindow.AppendText((string.Format(message.ToString(), obj) + "\n"));
        }

        void Log(string message, params object[] obj)
        {
            LogWindow.AppendText(string.Format(message, obj) + "\n");
        }

        void LogClear()
        {
            LogWindow.Clear();
        }

        void UpdatePlayerVisuals()
        {
            YourBetlbl.Visible = true;
            CurrentBetlbl.Visible = true;
            Balancelbl.Visible = true;

            YourBetlbl.Text = "Your Current Bet: " + bets[0];
            CurrentBetlbl.Text = "Current Bet: " + Highest(bets);
            Balancelbl.Text = "Balance: " + Balance[0];
        }

        void Next_Click(object sender, EventArgs e)
        {
            if (betting)
            {
                BettingRound(startBet);
                betting = false;
                return;
            }

            if (flop)
            {
                Flop();
                return;
            }

            if (turn)
            {
                Turn();
                return;
            }

            if (river)
            {
                River();
                return;
            }

            if (compare)
            {
                CompareCards();
            }
        }

        void CompareCards()
        {
            Log("There are still people left in the game some we compare cards");

            int[] fitnessValues = new int[4];

            for (int i = 0; i < fitnessValues.Length; i++)
            {
                if (folded[i])
                    continue;

                if (i == 0)
                {
                    fitnessValues[i] = Fitness(PlayerCards[0].Concat(CommunityCards.ToArray()).ToArray());
                }
                if (i == 1)
                {
                    fitnessValues[i] = Fitness(PlayerCards[1].Concat(CommunityCards.ToArray()).ToArray());
                }
                if (i == 2)
                {
                    fitnessValues[i] = Fitness(PlayerCards[2].Concat(CommunityCards.ToArray()).ToArray());
                }
                if (i == 3)
                {
                    fitnessValues[i] = Fitness(PlayerCards[3].Concat(CommunityCards.ToArray()).ToArray());
                }
            }

            int highest = Highest(fitnessValues);

            List<int> winners = new List<int>();

            for (int i = 0; i < fitnessValues.Length; i++)
            {
                if (fitnessValues[i] == highest)
                {
                    winners.Add(i);
                }
            }

            int prize = (GetBetSum(-1) / winners.Count);

            if (winners.Count == 1)
            {
                Log("We have one Winner! Congratulations to player {0} ({1}) you win {2}", winners[0], winners[0] == 0 ? "You" : "AI", prize);
            }

            if (winners.Count == 2)
            {
                Log("We have two Winners! Congratulations to players {0} ({1}), {2} ({3}) you win {4}", winners[0], winners[0] == 0 ? "You" : "AI", winners[1]
                    , winners[1] == 0 ? "You" : "AI", prize);
            }

            if (winners.Count == 3)
            {
                Log("We have three Winners! Congratulations to players {0} ({1}), {2} ({3}), {4} ({5})  you win {6}", winners[0], winners[0] == 0 ? "You" : "AI", winners[1]
                    , winners[1] == 0 ? "You" : "AI", winners[2], winners[2] == 0 ? "You" : "AI", prize);
            }

            if (winners.Count == 4)
            {
                Log("Everyone is a winner! Y'all get {0}", prize);
            }

            foreach (int winner in winners)
            {
                Balance[winner] += prize;
            }
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

        static List<Card> CreateDeckList(Card[] cards)
        {
            return Deck.GenerateDeck(cards);
        }

        static Card DrawCardFormDeckList(ref List<Card> deck)
        {
            int r = StaticRandom.Next(0, deck.Count);
            Card c = deck[r];
            deck.RemoveAt(r);
            return c;
        }
    }
}