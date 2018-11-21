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
                    this.suit = Suit;
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
          //static void SimulateAllDecks(int boardCardsCount)
          //{
          //     //long l = Combinations(52, 2) * Combinations(50, boardCardsCount);
          //     Stopwatch st = new Stopwatch();
          //     long soFar = 0;
          //     long goal = Combinations(52, 2);
          //     st.Start();
          //     List<Task> tasks = new List<Task>();
          //     for (int i = 0; i < deck.Length; i++)
          //     {
          //          for (int j = 0; j < deck.Length;  j++)
          //          {
          //               if(deck[i].id < deck[j].id)
          //               {
          //                    Card c1 = deck[i];
          //                    Card c2 = deck[j];
          //                    Task t = new Task(() => { AllBoardPermutations(c1, c2, boardCardsCount); });
          //                    t.Start();
          //                    tasks.Add(t);
          //               }
          //               if (tasks.Count == 200)
          //               {
          //                    bool b = true;
          //                    while (b)
          //                    {
          //                         b = true;
          //                         for (int k = 0; k < tasks.Count; k++)
          //                         {
          //                              if (tasks[k].IsCompleted)
          //                              {
          //                                   b = false;
          //                                   tasks.RemoveAt(k);
          //                                   soFar++;
          //                                   k--;
          //                              }
          //                         }
          //                         //Console.WriteLine("Elapsed Time | " + st.Elapsed + " | Expected Time: " + ((st.ElapsedMilliseconds / 60000.0) / ((double)(soFar) / goal)) + " Minutes");
          //                         //Console.WriteLine("Completed : " + ((double)(soFar * 100) / goal).ToString() + "%");
          //                         //Console.WriteLine("Average Win Rate : " + AddedChance / total);
          //                    }
          //               }
          //          }
          //     }
          //     bool d = true;
          //     while (d)
          //     {
          //          d = false;
          //          for (int k = 0; k < tasks.Count; k++)
          //          {
          //               if (!tasks[k].IsCompleted)
          //               {
          //                    d = true;
          //               }
          //               else
          //               {
          //                    tasks.RemoveAt(k);
          //                    soFar++;
          //                    k--;
          //               }
          //          }

          //          Console.Clear();
          //          Console.WriteLine("Elapsed Time | " + st.Elapsed + " | Expected Time: " + ((st.ElapsedMilliseconds / 60000) / ((double)(soFar) / goal)) + " Minutes");
          //          Console.WriteLine("Completed : " + ((double)(soFar * 100) / goal).ToString() + "%");
          //          //Console.WriteLine("Average Win Rate : " + AddedChance / total);
          //     }
          //     st.Stop();
          //}
          //static double AddedChance = 0;
          //static void AllBoardPermutations(Card c1, Card c2, int count)
          //{
          //     Card[] handThingy = new Card[2 + count];
          //     handThingy[0] = c1;
          //     handThingy[1] = c2;
          //     Directory.CreateDirectory("C:\\GymnasieProjekt\\Data\\" + count + "\\" + GetNameOfCardArrayFirst(handThingy));
          //     //long f = 0;
          //     //long added = 0;
          //     foreach (var item in BoardPermutations(handThingy, 2))
          //     {
          //          //f++;
          //          //if(f % 1000 == 0)
          //          //{
          //          //     lock (deck)
          //          //     {
          //          //          total += f - added;
          //          //          added += f - added;
          //          //     }
          //          //}
          //          double chanceOfWin = SimulateChanceOfWinning(item);
          //          //lock (deck)
          //          //     AddedChance += chanceOfWin;
          //          StreamWriter writer = new StreamWriter("C:\\GymnasieProjekt\\Data\\" + count + "\\" + GetNameOfCardArrayFirst(item) + "\\" + GetNameOfCardArray(item) + ".txt");
          //          writer.Write(chanceOfWin * 100);
          //          writer.Close();
          //     }
          //     //lock (deck)
          //     //{
          //     //     total += f - added;
          //     //     added += f - added;
          //     //}
          //}

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

          static double SimulateChanceOfWinning(Card[] cards)
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
               return (double)wins / games;
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

          //static IEnumerable<Card[]> BoardPermutations(Card[] cardsSoFar, int index)
          //{
          //     if (index >= cardsSoFar.Length)
          //          yield return cardsSoFar;
          //     else
          //          foreach (Card card in deck)
          //          {
          //               Card[] cardsPermutation = cardsSoFar;
          //               cardsPermutation[index] = card;
          //               bool skip = false;
          //               for (int i = 0; i < 2; i++)
          //               {
          //                    if (cardsSoFar[i].id == card.id)
          //                    {
          //                         skip = true;
          //                         break;
          //                    }
          //               }
          //               if(!skip)
          //               {
          //                    bool validBoard = true;
          //                    if(index > 2)
          //                         if (cardsPermutation[index - 1].id >= cardsPermutation[index].id)
          //                              validBoard = false;
          //                    if (validBoard)
          //                    {
          //                         if (index == cardsPermutation.Length - 1)
          //                              yield return cardsPermutation;
          //                         else
          //                              foreach (Card[] futurePermutations in BoardPermutations(cardsPermutation, index + 1))
          //                              {
          //                                   yield return futurePermutations;
          //                              }
          //                    }
          //               }
          //          }
          //}

          

          //public static double[] TrueFitnessFunction(int[] indecies, double[][][] networks, double[] fitnessArray)
          //{
          //     int[] playerMarkers = new int[indecies.Length];
          //     int[] currentPlayerBet = new int[indecies.Length];

          //     List<Card> deck = CreateDeckList();

          //     Card[] playerHands = new Card[indecies.Length * 2];
          //     Card[] board = new Card[5];
          //     int dealer = 0;
          //     int firstPlayer = 0;
          //     for (int i = 0; i < NetSettings.fitnessRounds; i++)
          //     {
          //          while (playerMarkers[dealer % 4] == 0)
          //               dealer++;
          //          dealer = dealer % 4;
          //          for (int j = 0;  true; j++)
          //          {
          //               if (playerMarkers[(dealer + j) % 4] > 0)
          //               {
          //                    currentPlayerBet[(dealer + j) % 4] = 5;
          //                    playerMarkers[(dealer + j) % 4] -= 5;
          //                    for (int k = 0; true; k++)
          //                    {
          //                         if (playerMarkers[(dealer + j + k) % 4] > 0)
          //                         {
          //                              currentPlayerBet[(dealer + j + k) % 4] = 10;
          //                              playerMarkers[(dealer + j + k) % 4] -= 10;
          //                              for (int l = 0; true; l++)
          //                              {
          //                                   if(playerMarkers[(dealer + j + k + l) % 4] > 0)
          //                                   {
          //                                        firstPlayer = (dealer + j + k + l) % 4;
          //                                        break;
          //                                   }
          //                              }
          //                              break;
          //                         }
          //                    }
          //                    break;
          //               }
          //          }

          //          for (int j = 0; j < playerHands.Length; j++)
          //          {
          //               if(playerMarkers[j] > 0)
          //          }
          //     }
          //}

          static Card[] CreateDeck()
          {
               Card[] deck = new Card[52];
               for (int i = 0; i < 4; i++)
               {
                    for (int j = 1; j < 14;  j++)
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
                         if(add)
                              deck.Add(newCard);
                    }
               }
               return deck;
          }

          static long Combinations(int n, int r)
          {
               long result = 1;
               for (int i = n; i > (n - r); i--)
               {
                    result *= i;
               }
               for (int i = 1; i <= r; i++)
               {
                    result /= i;
               }
               return result;
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

                                   if (valueA != 0 && valueB == 0)
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