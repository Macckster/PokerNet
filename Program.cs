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

            int dealer = 0;
            int bigBlindBet = 0;

            int LastRaiseIndex = 0;

            int[] playerIndexArray = {0, 1, 2, 3};
            
            int highestCurrentBet;
            bool allowHigherBet = true;

            int bet = 0;

            int PlayerIndex = 0;

            int i;
            int j;
            //The most boring expression ever, also remember to remove this 
            string[] board = new string[7];

            for(int h = 0; h < 10; h++)
            {
                dealer++;
                allowHigherBet = true;
                playerBet = new int[playerBet.Length];
                moneyPool = 0;
                playerIndexArray = GetPlayers(playerBalance);

                bigBlindBet = Math.Min(playerBalance[playerIndexArray[(dealer + 2) % playerIndexArray.Length]], 2);
                moneyPool += bigBlindBet + 1;

                playerBalance[playerIndexArray[(dealer + 1) % playerIndexArray.Length]] -= 1;
                playerBalance[playerIndexArray[(dealer + 2) % playerIndexArray.Length]] -= bigBlindBet;

                highestCurrentBet = bigBlindBet;


                //Round for loop
                for (i = 0; i < 4; i++)
                {
                    LastRaiseIndex = (dealer + 2) % playerIndexArray.Length;
                    if (i == 1)
                    {
                        //Add two cards
                    }
                    else if (i == 2)
                    {
                        //add one card
                    }
                    else if (i == 3)
                    {
                        //Add one card
                    }

                    for (j = (dealer + 3) % playerIndexArray.Length; true; j++)
                    {
                        PlayerIndex = j % playerIndexArray.Length;
                        if (playerBet[playerBet[PlayerIndex]] != -1)
                        {
                            if (LastRaiseIndex == PlayerIndex)
                                break;

                            //Get bet from index
                            bet = GetBet();

                            if (bet <= playerBalance[playerIndexArray[PlayerIndex]])
                            {
                                if (!allowHigherBet)
                                    bet = highestCurrentBet;

                                if (bet >= highestCurrentBet)
                                {
                                    playerBet[playerIndexArray[playerBet]] = bet;
                                    playerBalance[playerIndexArray[PlayerIndex]] -= bet;
                                    allowHigherBet = (playerBalance[playerIndexArray[PlayerIndex]] != 0);
                                    moneyPool += bet;
                                    if (bet > highestCurrentBet)
                                    {
                                        highestCurrentBet = bet;
                                        LastRaiseIndex = PlayerIndex;
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
