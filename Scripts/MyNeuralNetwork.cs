using System;
using System.Collections.Generic;

public class MyNeuralNetwork
{
    public int[] layers;
    public float[][] neurons;
    public float[][] biases;
    public float[][][] weights;

    public MyNeuralNetwork(int[] layers)
    {
        this.layers = layers;
        neurons = new float[layers.Length][];
        biases = new float[layers.Length][];
        weights = new float[layers.Length - 1][][];

        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
            biases[i] = new float[layers[i]];
        }

        for (int i = 0; i < layers.Length - 1; i++)
        {
            weights[i] = new float[layers[i + 1]][];
            for (int j = 0; j < layers[i + 1]; j++)
            {
                weights[i][j] = new float[layers[i]];
            }
        }

        RandomizeBiases();
        RandomizeWeights();
    }

    /* public float[] FeedForward(float[] inputs)
     {
         for (int i = 0; i < inputs.Length; i++)
         {
             neurons[0][i] = inputs[i];
         }

         for (int i = 1; i < layers.Length; i++)
         {
             for (int j = 0; j < layers[i]; j++)
             {
                 float sum = 0f;
                 for (int k = 0; k < layers[i - 1]; k++)
                 {
                     sum += weights[i - 1][j][k] * neurons[i - 1][k];
                 }
                 neurons[i][j] = (float)Math.Tanh(sum + biases[i][j]);
             }
         }

         return neurons[layers.Length - 1];
     }*/

    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                float sum = 0f;
                for (int k = 0; k < layers[i - 1]; k++)
                {
                    sum += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = -(float)Math.Tanh(-sum + biases[i][j]); // modify activation function here
            }
        }

        return neurons[layers.Length - 1];
    }
        private void RandomizeBiases()
    {
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                biases[i][j] = (float)new Random().NextDouble() * 2f - 1f;
            }
        }
    }

    private void RandomizeWeights()
    {
        for (int i = 0; i < layers.Length - 1; i++)
        {
            for (int j = 0; j < layers[i + 1]; j++)
            {
                for (int k = 0; k < layers[i]; k++)
                {
                    weights[i][j][k] = (float)new Random().NextDouble() * 2f - 1f;
                }
            }
        }
    }
}
