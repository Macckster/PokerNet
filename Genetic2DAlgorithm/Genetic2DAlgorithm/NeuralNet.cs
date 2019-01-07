using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetic2DAlgorithm;

namespace PokerNet
{
    public static class NetSettings
     {
          public static int fitnessRounds = 12;
          public static int inputNodeCount = 5;
          public static int outPutNodeCount = 1;
          public static double minWeight = -6;
          public static double maxWeight = 6;
          public static int[] midlayerNodesCount = { 4, 5, 5, 4};
     }
    public static class NeuralNet
    {

        /// <summary>
        /// "Feed" the inputs through the network.
        /// </summary>
        /// <param name="input">Inputs you want to use</param>
        /// <returns></returns>
        public static double[] FeedForward(double[] input, double[][] weights)
        { 
            if(input.Length != NetSettings.inputNodeCount)
            {
                //You doofus you sent the wrong size inputs!
                return null;
            }

            //Split the weights and the biases as they are stored in the same array
            double[] layerWeights = weights[0];
            double[] bias = GetBias(layerWeights, NetSettings.inputNodeCount);
            layerWeights = RemoveBias(layerWeights, NetSettings.inputNodeCount);

            //Apply the weights and biases to the inputs
            double[] values = ApplyWeightsAndBias(layerWeights, input, bias);

            //Normalize the values
            values = Map(Sigmoid, values);

            //Apply weights and biases to the values for each middle layer
            for (int i = 0; i < NetSettings.midlayerNodesCount.Length; i++)
            {
                //Get the node count for this array
                int nodeCount = NetSettings.midlayerNodesCount[i];

                //Split the weights and the biases as they are stored in the same array
                layerWeights = weights[i + 1];
                bias = GetBias(layerWeights, nodeCount);
                layerWeights = RemoveBias(layerWeights, nodeCount);

                //Apply the weights and biases
                values = ApplyWeightsAndBias(layerWeights, values, bias);

                //Normalize the values
                values = Map(Sigmoid, values);
            }

            return values;
        }

        //Apply weights to some values
        static double[] ApplyWeightsAndBias(double[] weights, double[] values, double[] biases)
        {
            int nextLayerNodeCount = weights.Length / values.Length;

            double[] ret = new double[nextLayerNodeCount];

            for (int i = 0; i < nextLayerNodeCount; i++)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    ret[i] += values[j] * weights[i + j] + biases[j];
                }
            }

            return ret;
        }

        //Run a function on all values in an array
        static double[] Map(Func<double, double> function, double[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = function(arr[i]);
            }

            return arr; //Pirate
        }

        //Get the bias from some weights
        static double[] GetBias(double[] arr, int biasCount)
        {
            double[] bias = new double[biasCount];

            for (int i = 0; i < biasCount; i++)
            {
                bias[i] = arr[i];
            }

            return bias;
        }

        //Remove the bias from a array
        static double[] RemoveBias(double[] arr, int biasCount)
        {
            double[] ret = new double[arr.Length - biasCount];

            int j = 0;

            for (int i = arr.Length - 1; i >= biasCount; i--)
            {
                ret[j] = arr[i];
                j++;
            }

            return ret;
        }

        //Sigmoid function to normalize values between -1 and 1
        static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        //Generate the weight matrix aswell as set some start values
        public static double[][] GenerateNeuralNetwork()
        {
            double[][] weights = new double[1 + NetSettings.midlayerNodesCount.Length][];

            //Set up the weights in the first layer
            weights[0] = new double[NetSettings.inputNodeCount * NetSettings.midlayerNodesCount[0] + NetSettings.inputNodeCount];
            weights[weights.Length - 1] = new double[NetSettings.midlayerNodesCount.Last() * NetSettings.outPutNodeCount + NetSettings.midlayerNodesCount.Last()];
            
            //Set up the weights in the other layers
            for (int i = 1; i < weights.Length - 1; i++)
            {
                weights[i] = new double[NetSettings.midlayerNodesCount[i - 1] * NetSettings.midlayerNodesCount[i] + NetSettings.midlayerNodesCount[i - 1]];
            }

            //Not useful just here to set some inital weights and biases
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                         weights[i][j] = StaticRandom.RandDouble() * (NetSettings.maxWeight - NetSettings.minWeight) + NetSettings.minWeight;
                }
            }

            return weights;
        }
    }
}