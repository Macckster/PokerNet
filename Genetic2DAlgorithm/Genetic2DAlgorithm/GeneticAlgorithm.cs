using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using PokerNet;
using cardOddsSimulator;

namespace Genetic2DAlgorithm
{
    public static class StaticRandom//Thread friendly random
    {
        static int seed = 0;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int min, int max)
        {
            return random.Value.Next(min, max);
        }
        public static double RandDouble()
        {
            return random.Value.NextDouble();
        }
    }

    class GeneticAlgorithm
    {
        static readonly double mutationValue = 0.009;//The +- multiplier
        static readonly double chanceOfMutation = 0.005;//The idividual chance of mutation for each point in the array
        static readonly int population = 1000;//The population size
        static readonly int islands = 10;//The amount of islands

        static double[][] temp;

        

        static void Main(string[] args)
        {
            FitnessFunction.InstantiateDynamicArrays();
            FitnessFunction.PlayRounds();
            ////temp = DeSerializePlayer("C:\\GymnasieProjekt\\test2.txt");//GeneratePopulation(1, 5, 5, 3, 1, -100, 100)[0];
            //InstantiateDynamicArrays();
            //double[][][] pop;
            //double[][] best = null;//DeSerializePlayer("C:\\GymnasieProjekt\\test1.txt");
            //for (int i = 0; i < 100000; i++)
            //{

            //    pop = EvolveWithIslandClustersThreaded(1000, 10, 100, 500, FitnessFunctionTemp, best);//här

            //    if (true)//i % 50 == 0)
            //    {
            //        double[] fitness = FitnessFunctionTemp(pop);
            //        best = FindBestPlayer(pop, fitness);
            //        SerializePlayer(best, "C:\\GymnasieProjekt\\test1.txt");
            //        double bestFitness = 0;
            //        for (int e = 0; e < fitness.Length; e++)
            //        {
            //            bestFitness = Math.Max(fitness[e], bestFitness);
            //        }

            //        //Console.WriteLine((-(bestFitness - 8000)).ToString() + " : " + (-(bestFitness - 8000)/connections).ToString());
            //    }
            //}
        }

        

        /// <summary>
        /// Serializes a player to text format
        /// </summary>
        /// <param name="playerToSerialize">The player to serialize</param>
        /// <param name="path">The file location and name</param>
        static void SerializePlayer(double[][] playerToSerialize, string path)
        {
            StreamWriter stream = new StreamWriter(path);
            string s = "";
            for (int i = 0; i < playerToSerialize.Length; i++)
            {
                string line = "";
                for (int e = 0; e < playerToSerialize[i].Length; e++)
                {
                    line += playerToSerialize[i][e].ToString() + ((e + 1 == playerToSerialize[i].Length) ? "" : ",");
                }
                s += line + ";";
            }
            stream.Write(s);
            stream.Close();
        }
        /// <summary>
        /// Deserializes a player
        /// </summary>
        /// <param name="path">the file location and name</param>
        /// <returns>returns the player</returns>
        static double[][] DeSerializePlayer(string path)
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
        /// <summary>
        /// Creates and evolves new population using island technic
        /// </summary>
        /// <param name="generations">The total generations of evolution</param>
        /// <param name="islands">The amount of isolated islands</param>
        /// <param name="combineAfter">The amount of generations between each merge of the island populations</param>
        /// <param name="islandPopulation">The population on each island</param>
        /// <param name="fitnessFunction">The function that evaluates the fitness of the population</param>
        /// <param name="player">A player to add to the population</param>
        /// <returns>Returns the best player from each island</returns>
        static double[][][] EvolveWithIslandClusters(int generations, int islands, int combineAfter, int islandPopulation, Func<double[][][], double[]> fitnessFunction, double[][] player = null)
        {
            double[][][][] islandArray = new double[islands][][][];
            for (int i = 0; i < islands; i++)
            {
                islandArray[i] = GeneratePopulation(islandPopulation);
            }
            if (player != null)
                islandArray[0][0] = player;
            for (int g = 0; g < generations / combineAfter; g++)
            {

                for (int i = 0; i < islands; i++)
                {
                    islandArray[i] = EvolvePopulation(islandArray[i], combineAfter, fitnessFunction);
                }

                List<double[][]> combinedPop = new List<double[][]>();
                for (int i = 0; i < islands; i++)
                {
                    combinedPop.AddRange(islandArray[i]);
                }

                for (int i = 0; i < islands; i++)
                {
                    for (int f = 0; f < islandPopulation; f++)
                    {
                        int index = StaticRandom.Rand(0, combinedPop.Count);
                        islandArray[i][f] = combinedPop[index];
                        combinedPop.RemoveAt(index);
                    }
                }
            }
            double[][][] popHighFitness = new double[islands][][];
            for (int i = 0; i < islands; i++)
            {
                popHighFitness[i] = FindBestPlayer(islandArray[i], fitnessFunction(islandArray[i]));
            }

            return EvolvePopulation(popHighFitness, generations, fitnessFunction);
        }
        /// <summary>
        /// Creates and evolves new population using island technic(Threaded)
        /// </summary>
        /// <param name="generations">The total generations of evolution</param>
        /// <param name="islands">The amount of isolated islands</param>
        /// <param name="combineAfter">The amount of generations between each merge of the island populations</param>
        /// <param name="islandPopulation">The population on each island</param>
        /// <param name="fitnessFunction">The function that evaluates the fitness of the population</param>
        /// <param name="player">A player to add to the population</param>
        /// <returns>Returns the best player from each island</returns>
        static double[][][] EvolveWithIslandClustersThreaded(int generations, int islands, int combineAfter, int islandPopulation, Func<double[][][], double[]> fitnessFunction, double[][] player = null)
        {
            ConcurrentBag<double[][][]> islandArray = new ConcurrentBag<double[][][]>();
            ConcurrentBag<double[][][]> evolved = new ConcurrentBag<double[][][]>();

            for (int i = 0; i < islands; i++)
            {
                double[][][] temp = GeneratePopulation(islandPopulation);
                if (i == 0 && player != null)
                    temp[0] = player;
                evolved.Add(temp);
            }

            for (int g = 0; g < generations / combineAfter; g++)
            {
                Thread[] threadArray = new Thread[islands];
                islandArray = evolved;
                evolved = new ConcurrentBag<double[][][]>();

                for (int i = 0; i < islands; i++)
                {
                    threadArray[i] = new Thread(() => { islandArray.TryTake(out double[][][] item); evolved.Add(EvolvePopulation(item, combineAfter, fitnessFunction)); });
                    threadArray[i].Start();
                }

                for (int i = 0; i < islands; i++)
                {
                    threadArray[i].Join();
                }

                List<double[][]> combinedPop = new List<double[][]>();
                while (evolved.TryTake(out double[][][] takeOut))
                {
                    combinedPop.AddRange(takeOut);
                }

                for (int i = 0; i < islands; i++)
                {
                    double[][][] pop = new double[islandPopulation][][];
                    for (int f = 0; f < islandPopulation; f++)
                    {
                        int index = StaticRandom.Rand(0, combinedPop.Count);
                        pop[f] = combinedPop[index];
                        combinedPop.RemoveAt(index);
                    }
                    evolved.Add(pop);
                }
            }
            double[][][] popHighFitness = new double[islands][][];
            double[][][] take;
            for (int i = 0; evolved.TryTake(out take); i++)
            {
                popHighFitness[i] = FindBestPlayer(take, fitnessFunction(take));
            }
            return EvolvePopulation(popHighFitness, 1, fitnessFunction);
        }
        /// <summary>
        /// Evolves the population
        /// </summary>
        /// <param name="pop">The population</param>
        /// <param name="generations">The amount of generations</param>
        /// <param name="fitnessFunction">The function that evaluates the fitness of the population</param>
        /// <returns>An evolved population</returns>
        static double[][][] EvolvePopulation(double[][][] pop, int generations, Func<double[][][], double[]> fitnessFunction)
        {
            for (int i = 0; i < generations; i++)
            {
                double[] fitness = fitnessFunction(pop);
                pop = GenerateNewPopulation(pop, fitness);
            }

            return pop;
        }

        static double[] FitnessFunctionTemp(double[][][] pop)
        {
            double[] fitnessArray = new double[pop.Length];

            for (int i = 0; i < pop.Length; i++)
            {
                double diff = 0;

                for (int e = 0; e < pop[i].Length; e++)
                {
                    for (int f = 0; f < pop[i][e].Length; f++)
                    {
                        diff += Math.Abs(pop[i][e][f] - temp[e][f]);
                    }
                }

                fitnessArray[i] = (8000 - diff) > 0 ? 8000 - diff : 1;
            }

            return fitnessArray;
        }
        /// <summary>
        /// Generates a new generation
        /// </summary>
        /// <param name="population">The population</param>
        /// <param name="fitness">The fitness of the population</param>
        /// <returns>Returns a new generation</returns>
        static double[][][] GenerateNewPopulation(double[][][] population, double[] fitness)
        {
            double[][][] newPopulation = new double[population.Length][][];
            newPopulation[0] = FindBestPlayer(population, fitness);
            for (int i = 1; i < population.Length; i++)
                newPopulation[i] = GenerateChild(PickParent(population, fitness), PickParent(population, fitness));

            return newPopulation;
        }
        /// <summary>
        /// Finds the best player of the given population
        /// </summary>
        /// <param name="population">The population</param>
        /// <param name="fitness">The fitness of the population</param>
        /// <returns>The best player of the population</returns>
        static double[][] FindBestPlayer(double[][][] population, double[] fitness)
        {
            int index = 0;
            for (int i = 1; i < fitness.Length; i++)
            {
                if (fitness[i] > fitness[index])
                    index = i;
            }
            return population[index];
        }
        /// <summary>
        /// Generates a child based on the parents
        /// </summary>
        /// <param name="parentA">Parent A</param>
        /// <param name="parentB">Parent B</param>
        /// <returns>Child C</returns>
        static double[][] GenerateChild(double[][] parentA, double[][] parentB)
        {
            bool[][] parentALegacy = MarkArea(parentA, StepSize());

            double[][] child = new double[parentA.Length][];

            for (int i = 0; i < child.Length; i++)
            {
                child[i] = new double[parentA[i].Length];
                for (int f = 0; f < child[i].Length; f++)
                {
                    if (parentALegacy[i][f])
                        child[i][f] = parentA[i][f];
                    else
                        child[i][f] = parentB[i][f];
                }
            }

            child = MutatePlayer(child);

            return child;
        }
        /// <summary>
        /// Mutates player
        /// </summary>
        /// <param name="mutatee">The mutatee</param>
        /// <returns>The mutated player</returns>
        static double[][] MutatePlayer(double[][] mutatee)
        {
            double[][] returnPlayer = mutatee;
            for (int i = 0; i < mutatee.Length; i++)
            {
                for (int f = 0; f < mutatee[i].Length; f++)
                {
                    if (StaticRandom.RandDouble() < chanceOfMutation)
                    {
                        double newValue = ((StaticRandom.RandDouble() - 0.5) * 2) * mutationValue;

                        if (NetSettings.maxWeight < newValue)
                            returnPlayer[i][f] = NetSettings.maxWeight;
                        else if (NetSettings.minWeight > newValue)
                            returnPlayer[i][f] = NetSettings.minWeight;
                        else
                            returnPlayer[i][f] = newValue;
                    }
                }
            }
            return returnPlayer;
        }

        static double StepSize()
        {
            return (6.0 / (Convert.ToDouble(StaticRandom.Rand(0, 20)) + 1.0)) + 1.0;
        }
        /// <summary>
        /// Marks an area of a player
        /// </summary>
        /// <param name="area">The player to be marked</param>
        /// <param name="steps">The radius of the marking</param>
        /// <returns></returns>
        static bool[][] MarkArea(double[][] area, double steps)
        {

            bool[][] returnArea = new bool[area.Length][];

            for (int i = 0; i < area.Length; i++)
            {
                returnArea[i] = new bool[area[i].Length];
            }

            int xPoint = StaticRandom.Rand(0, returnArea.Length);
            int yPoint = StaticRandom.Rand(0, returnArea[xPoint].Length);

            for (int i = Math.Max(xPoint - (int)Math.Ceiling(steps), 0); i < Math.Min(xPoint + steps, returnArea.Length); i++)
            {
                for (int f = Math.Max(yPoint - (int)Math.Ceiling(steps), 0); f < Math.Min(yPoint + steps, returnArea[i].Length); f++)
                {
                    if (Math.Pow(Math.Abs(yPoint - f), 2) + Math.Pow(Math.Abs(xPoint - i), 2) <= steps * steps)
                    {
                        returnArea[i][f] = true;
                    }
                }
            }

            return returnArea;
        }
        /// <summary>
        /// Picks a random player based on fitness
        /// </summary>
        /// <param name="population">The population to pick from</param>
        /// <param name="fitness">The fitness of the population</param>
        /// <returns>A random Player</returns>
        static double[][] PickParent(double[][][] population, double[] fitness)
        {
            int totalFitness = 0;
            foreach (int value in fitness)
            {
                totalFitness += value;
            }

            double choice = StaticRandom.RandDouble() * totalFitness;
            double val = 0;

            for (int i = 0; i < population.Length; i++)
            {
                val += fitness[i];
                if (choice < val)
                    return population[i];
            }

            //något gick väldigt fel
            Console.WriteLine("Parental Abuse");
            return null;
        }
        /// <summary>
        /// Generates a completly new population
        /// </summary>
        /// <param name="populationCount">The population count</param>
        /// <param name="width">The the width of the hiden layers of the network</param>
        /// <param name="heigth">The the height of the hiden layers of the network</param>
        /// <param name="inputs">The amount of inputs of the network</param>
        /// <param name="outputs">The amount of outputs of the network</param>
        /// <param name="minValue">The minimum value that can be generated on a point of the network</param>
        /// <param name="maxValue">The maximum value that can be generated on a point of the network</param>
        /// <returns></returns>
        static double[][][] GeneratePopulation(int populationCount)
        {
            double[][][] pop = new double[populationCount][][];
            for (int i = 0; i < populationCount; i++)
            {
                pop[i] = NeuralNet.GenerateNeuralNetwork();
                //pop[i] = new double[width + 1][];
                //for (int e = 0; e < width + 1; e++)
                //{
                //     if (e == 0)
                //          pop[i][e] = new double[inputs * heigth];
                //     else if (e == width)
                //          pop[i][e] = new double[outputs * heigth];
                //     else
                //          pop[i][e] = new double[heigth * heigth];

                //     for (int f = 0; f < pop[i][e].Length; f++)
                //     {
                //          double d = StaticRandom.RandDouble() * (maxValue - minValue) + minValue;
                //          pop[i][e][f] = d;
                //     }
                //}
            }

            return pop;
        }
    }
}