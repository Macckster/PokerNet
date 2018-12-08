﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheGame
{
    static class Game
    {
        static Manager man;

        public static IEnumerator Play(GameObject cam)
        {
            man = cam.GetComponent<Manager>();

            List<Card> deck = Card.GetDeck();

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

                    if(i == 0)
                    {
                        man.UpdatePlayerHand(deck.DrawCard(), deck.DrawCard());
                        //Note draw card for ai aswell
                    }

                    if (i == 1)
                    {
                        man.InitialFlip(deck.DrawCard(), deck.DrawCard(), deck.DrawCard());
                    }
                    else if (i == 2)
                    {
                        man.SecondFlip(deck.DrawCard());
                    }
                    else if (i == 3)
                    {
                        man.ThirdFlip(deck.DrawCard());
                    }

                    for (j = (dealer + 3) % playerIndexArray.Length; true; j++)
                    {
                        PlayerIndex = j % playerIndexArray.Length;
                        if (playerBet[playerIndexArray[PlayerIndex]] != -1)
                        {
                            if (LastRaiseIndex == PlayerIndex)
                                break;

                            //Get bet from index

                            if (bet <= playerBalance[playerIndexArray[PlayerIndex]])
                            {
                                if (!allowHigherBet)
                                    bet = highestCurrentBet;

                                if (bet >= highestCurrentBet)
                                {
                                    playerBet[playerIndexArray[PlayerIndex]] = bet;
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

            yield return null;
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
