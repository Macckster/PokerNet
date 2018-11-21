using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNet_2._0
{
    static class NeuralSettings
     {
          public int inputNodeCount;
          public int outPutNodeCount;
          public int[] midlayerNodesCount;
     }
    class NeuralNet
    {
        public double[][] weights;

        /// <summary>
        /// Create a new Neural Network
        /// </summary>
        /// <param name="inputNodeCount">Amount of input nodes</param>
        /// <param name="outPutNodeCount">Amount of output nodes</param>
        /// <param name="midlayerNodesCount">Array of the amount of nodes in the middle layers</param>
        public NeuralNet()
        {
            weights = GenerateStartWeights();
        }

        /// <summary>
        /// "Feed" the inputs through the network.
        /// </summary>
        /// <param name="input">Inputs you want to use</param>
        /// <returns></returns>
        public double[] FeedForward(double[] input)
        { 
            if(input.Length != inputNodeCount)
            {
                //You doofus you sent the wrong size inputs!
                return null;
            }

            //Split the weights and the biases as they are stored in the same array
            double[] layerWeights = weights[0];
            double[] bias = GetBias(layerWeights, inputNodeCount);
            layerWeights = RemoveBias(layerWeights, inputNodeCount);

            //Apply the weights and biases to the inputs
            double[] values = ApplyWeightsAndBias(layerWeights, input, bias);

            //Normalize the values
            values = Map(Sigmoid, values);

            //Apply weights and biases to the values for each middle layer
            for (int i = 0; i < midlayerNodesCount.Length; i++)
            {
                //Get the node count for this array
                int nodeCount = midlayerNodesCount[i];

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
        double[] ApplyWeightsAndBias(double[] weights, double[] values, double[] biases)
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
        double[] Map(Func<double, double> function, double[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = function(arr[i]);
            }

            return arr; //Pirate
        }

        //Get the bias from some weights
        double[] GetBias(double[] arr, int biasCount)
        {
            double[] bias = new double[biasCount];

            for (int i = 0; i < biasCount; i++)
            {
                bias[i] = arr[i];
            }

            return bias;
        }

        //Remove the bias from a array
        double[] RemoveBias(double[] arr, int biasCount)
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
        double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        //Generate the weight matrix aswell as set some start values
        public double[][] GenerateStartWeights()
        {
            double[][] weights = new double[1 + midlayerNodesCount.Length][];

            //Set up the weights in the first layer
            weights[0] = new double[inputNodeCount * midlayerNodesCount[0] + inputNodeCount];
            weights[weights.Length - 1] = new double[midlayerNodesCount.Last() * outPutNodeCount + midlayerNodesCount.Last()];
            
            //Set up the weights in the other layers
            for (int i = 1; i < weights.Length - 1; i++)
            {
                weights[i] = new double[midlayerNodesCount[i - 1] * midlayerNodesCount[i] + midlayerNodesCount[i - 1]];
            }

            //Not useful just here to set some inital weights and biases
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    weights[i][j] = 1;
                }
            }

            return weights;
        }
    }
}