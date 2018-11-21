using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int moneyPool = 0;

            int[] playerBalance = { 100,100,100, 100 };
            int[] playerBet = new int[4];
            bool[] isPlayerOut = new bool[4];

            int dealer = 0;
            int bigBlindBet = 0;

            int[] players;

            int LastRaiseIndex = 0;
            
            int highestCurrentBet;
            bool allowHigherBet = true;

            int bet = 0;

            int PlayerIndex = 0;

            int i;
            int j;
            //The most boring expression ever, also remember to remove this 
            string[] board = new string[7];

            while (true)
            {
                allowHigherBet = true;
                playerBet = new int[playerBet.Length];
                isPlayerOut = new bool[isPlayerOut.Length];
                moneyPool = 0;
                players = GetPlayers(playerBalance);

                bigBlindBet = Math.Min(playerBalance[(dealer + 2) % players.Length], 2);
                moneyPool += bigBlindBet + 1;

                playerBalance[(dealer + 1) % players.Length] -= 1;
                playerBalance[(dealer + 2) % players.Length] -= bigBlindBet;

                highestCurrentBet = bigBlindBet;


                //Round for loop
                for (i = 0; i < 4; i++)
                {
                    LastRaiseIndex = (dealer + 2) % 4;
                    if(i == 1)
                    {
                        //Add two cards
                    }
                    else if(i == 2)
                    {
                        //add one card
                    }
                    else if(i == 3)
                    {
                        //Add one card
                    }
                    
                    for (j = (dealer + 3) % players.Length; true; j++)
                    {
                        PlayerIndex = j % players.Length;
                        if(!isPlayerOut[PlayerIndex])
                        {
                            if (LastRaiseIndex == PlayerIndex)
                                break;

                            //Get bet from index
                            bet = GetBet();
                            if (!allowHigherBet)
                                bet = highestCurrentBet;

                            if(bet >= highestCurrentBet)
                            {
                                playerBet[playerBet] = bet;
                                playerBalance[PlayerIndex] -= bet;
                                allowHigherBet = (playerBalance[PlayerIndex] != 0); 
                                moneyPool += bet;
                                if (bet > highestCurrentBet)
                                {
                                    highestCurrentBet = bet;
                                    LastRaiseIndex = PlayerIndex;
                                }
                            }
                            else
                            {
                                isPlayerOut[PlayerIndex] = true;
                            }
                        }
                    }
                }

                //GIVE THE CASH
            }
        }
        
        static int GetBet()
        {
            return 420691337;
        }

        static int[] GetPlayers(int[] playerBalance)
        {
            List<int> players = new List<int>();

            for (int i = 0; i < playerBalance.Length; i++)
            {
                if(playerBalance[i] > 0)
                {
                    players.Add(i);
                }
            }

            return players.ToArray();
        }
    }
}
