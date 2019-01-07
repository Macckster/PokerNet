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

        public Form1()
        {
            //weights = GetWeights(weightsPath);
            InitializeComponent();
        }

        private void Testbtn_Click(object sender, EventArgs e)
        {
            d.cards = Deck.GenerateDeck();
            PreFlop();
        }

        void Run()
        {
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

            Log("First Betting Round starting on player {0} {(1)}", FirstPlayer, FirstPlayer == 0 ? "You" : "AI");

            BettingRound(FirstPlayer, true);
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
        }

        public void Turn()
        {
            Log("Dealer Flips Fourth Community Card");

            CardsOnBoard["CommunityFour"] = d.Draw();
            CommunityFour.Image = CardsOnBoard["CommunityFour"];

            Log("Third Betting Round");
        }

        public void River()
        {
            Log("Dealer Flips Last Community Card");

            CardsOnBoard["CommunityFive"] = d.Draw();
            CommunityFive.Image = CardsOnBoard["CommunityFive"];

            Log("Last Betting Round");
        }

        public void BettingRound(int a, bool first)
        {
            if (first)
            {
                int[] bets = new int[]
                {
                    0,0,0,0
                };
            }

            for (int i = a; true; i = (i+1) % Balance.Count)
            {
                if(folded[i])
                {
                    //Player has folded
                    continue;
                }

                int playerBet = 0;

                if (a == 0)
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
                    //get ai bet;
                    playerBet = i;
                }


            }
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

        public void Log(string message, params object[] obj)
        {
            LogWindow.AppendText((string.Format(message, obj)+ "\n"));
        }

        public void LogClear()
        {
            LogWindow.Clear();
        }

        private void TestBet_Click(object sender, EventArgs e)
        {
            BettingRound(0, false);
        }
    }
}