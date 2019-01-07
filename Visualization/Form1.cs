using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;


namespace Visualization
{
    public partial class Form1 : Form
    {
        Deck d = new Deck();
        Dictionary<string, Card> CardsOnBoard = new Dictionary<string, Card>();

        string weightsPath = "";
        double[][] weights;

        List<int> Balance = new List<int>()
        {
            100,100,100,100
        };

        bool[] folded = new bool[]
        {
            false, false, false, false
        };

        int dealer = 0;
        int pool = 0;
        int lastDealer = -1;

        bool flop, turn, river, betting, compare;

        int startBet = -1;
        bool first;

        bool gameWon = false;

        int[] bets =
        {
                0,0,0,0
        };

        public Form1()
        {
            //weights = GetWeights(weightsPath);
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

        public void PreFlop()
        {
            dealer = (lastDealer + 1) % Balance.Count;
            LogClear();
            PlayerCardsLbl.Visible = true;

            Log("Beginning");

            CardsOnBoard["PlayerCardOne"] = d.Draw();
            CardsOnBoard["PlayerCardTwo"] = d.Draw();

            CardsOnBoard["AiOneCardOne"] = d.Draw();
            CardsOnBoard["PlayerCardTwo"] = d.Draw();

            CardsOnBoard["AiTwoCardOne"] = d.Draw();
            CardsOnBoard["AiTwoCardTwo"] = d.Draw();

            CardsOnBoard["AiThreeCardOne"] = d.Draw();
            CardsOnBoard["AiThreeCardTwo"] = d.Draw();

            PlayerCardOne.Image = CardsOnBoard["PlayerCardOne"];
            PlayerCardTwo.Image = CardsOnBoard["PlayerCardTwo"];

            Log("Dealer is Player Nr " + dealer + " ({0})", dealer == 0 ? "You" : "AI");

            int littleBlind = (dealer + 1) % Balance.Count;

            Log("Little Blind is player Nr " + littleBlind + " ({0})", littleBlind == 0 ? "You" : "AI");

            int bigBlind = (dealer + 2) % Balance.Count;

            Log("Big Blind is player Nr " + bigBlind + " ({0})", bigBlind == 0 ? "You" : "AI");

            Log("Little Blind and Big Blind play their starting bets");
            
            pool += 3;
            Balance[littleBlind] -= 1;
            Balance[bigBlind] -= 2;
            Log("Pool is on {0}", pool);

            int FirstPlayer = (bigBlind + 1) % Balance.Count;

            Log("First Betting Round starting on player {0} ({1})", FirstPlayer, FirstPlayer == 0 ? "You" : "AI");

            flop = turn = river = betting = false;
            flop = true;
            betting = true;

            first = true;
            startBet = FirstPlayer;

            next.Visible = true;
        }

        public void Flop()
        {
            Log("Dealer Flips First Three Community Cards");

            ComCardsLbl.Visible = true;

            CardsOnBoard["CommunityOne"] = d.Draw();
            CardsOnBoard["CommunityTwo"] = d.Draw();
            CardsOnBoard["CommunityThree"] = d.Draw();

            CommunityOne.Image = CardsOnBoard["CommunityOne"];
            CommunityTwo.Image = CardsOnBoard["CommunityTwo"];
            CommunityThree.Image = CardsOnBoard["CommunityThree"];

            Log("Second Betting Round");

            flop = turn = river = betting = false;
            turn = true;
            betting = true;
        }

        public void Turn()
        {
            Log("Dealer Flips Fourth Community Card");

            CardsOnBoard["CommunityFour"] = d.Draw();
            CommunityFour.Image = CardsOnBoard["CommunityFour"];

            Log("Third Betting Round");

            flop = turn = river = betting = false;
            river = true;
            betting = true;
        }

        public void River()
        {
            Log("Dealer Flips Last Community Card");

            CardsOnBoard["CommunityFive"] = d.Draw();
            CommunityFive.Image = CardsOnBoard["CommunityFive"];

            Log("Last Betting Round");

            flop = turn = river = betting = false;
            river = true;
            betting = true;
            compare = true;
        }

        public void BettingRound(int a, bool first)
        {
            int winner = -1;

            if (first)
            {
                //bets[(a - 1) % Balance.Count] = 2;
                //bets[(a - 2) % Balance.Count] = 1;
            }

            bool roundoUno = true;

            for (int i = a; true; i = (i + 1) % Balance.Count)
            {
                if (folded[i])
                {
                    //Player has folded
                    continue;
                }

                if (i == a && !roundoUno)
                {
                    int target = Highest(bets);

                    bool blh = false;

                    for (int j = 0; j < bets.Length; j++)
                    {
                        if (bets[j] != target && !folded[j])
                        {
                            blh = true;
                        }
                    }

                    if (!blh)
                    {
                        Log("Betting Round Over");
                        return;
                    }
                }

                int playerBet = 0;

                if (i == 0)
                {
                    playerBet = GetPlayerBet();

                    if (playerBet == -1)
                    {
                        folded[0] = true;
                        continue;
                    }
                }
                else
                {
                    //Ai bet
                    playerBet = 1;
                }

                if (!folded[i])
                {
                    Log("Player {0} ({1}) bet {2}", i,i == 0? "You" : "AI", playerBet);

                    bets[i] += playerBet;
                }

                bool found = false;

                for (int j = 0; j < folded.Length; j++)
                {
                    if (!folded[j])
                    {
                        if (found)
                        {
                            found = false;
                            winner = -1;
                            break;
                        }
                        else
                        {
                            found = true;
                            winner = j;
                        }
                    }
                }

                if (found)
                {
                    Log("Congratulations! Player {0} ({1} has won!)", winner, winner == 0 ? "You" : "AI");
                    Balance[winner] += pool;
                    gameWon = true;
                    return;
                }

                roundoUno = false;
            }
        }

        public int Highest(int[] bets)
        {
            int highest = 0;

            for (int i = 0; i < bets.Length; i++)
            {
                if(bets[i] > highest)
                {
                    highest = bets[i];
                }
            }

            return highest;
        }

        public int GetPlayerBet()
        {
            string response = Interaction.InputBox("What do you want do bet? (Cancel to fold)", "User Bet", "");

            if(response == "")
            {
                return -1;
            }

            return int.Parse(response);
        }

        public double[][] GetWeights(string path)
        {
            StreamReader reader = new StreamReader(path);

            string player = reader.ReadToEnd();
            string[] comparted = player.Split(';');
            double[][] returnPlayer = new double[comparted.Length - 1][];
            for (int i = 0; i < comparted.Length - 1; i++)
            {
                string[] compartedLayer2 = comparted[i].Split(',');
                returnPlayer[i] = new double[compartedLayer2.Length];
                for (int f = 0; f < compartedLayer2.Length; f++)
                {
                    string d = compartedLayer2[f];
                    returnPlayer[i][f] = double.Parse(d);
                }
            }
            reader.Close();
            return returnPlayer;
        }

        public void Log(object message, params object[] obj)
        {
            LogWindow.AppendText((string.Format(message.ToString(), obj)+ "\n"));
        }

        public void Log(string message, params object[] obj)
        {
            LogWindow.AppendText(string.Format(message, obj) + "\n");
        }

        public void LogClear()
        {
            LogWindow.Clear();
        }

        private void TestBet_Click(object sender, EventArgs e)
        {
            BettingRound(0, false);
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (betting)
            {
                BettingRound(startBet, first);
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

            }
        }
    }
}