using System;
using System.Text;
using UnityEngine;

public class DNAForGoalOrientedMemory
{
    private string genes;
// private NeuralNetwork neuralNetwork;

    // Constructor to create a random DNA with a neural network
    public DNAForGoalOrientedMemory()
    {
        StringBuilder sb = new StringBuilder();
        System.Random rnd = new System.Random();

        for (int i = 0; i < 10; i++)
        {
            int value = rnd.Next(3) - 1; // Generate a value of 0, 1, or -1
            sb.Append(value);
        }

        genes = sb.ToString();
        //neuralNetwork = new NeuralNetwork(new int[] { 10, 4, 1 }); // Create a neural network with 10 inputs, 4 hidden neurons, and 1 output neuron
    }

    public void Combine(DNAForGoalOrientedMemory d1, DNAForGoalOrientedMemory d2)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            char gene = i < 5 ? d1.genes[i] : d2.genes[i];
            sb.Append(gene);
        }

        genes = sb.ToString();
    }

    public void Mutate(float mutationRate)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < genes.Length; i++)
        {
            if (rnd.NextDouble() < mutationRate)
            {
                genes = genes.Substring(0, i) + (genes[i] == '0' ? '1' : '0') + genes.Substring(i + 1);
            }
        }
    }

    // Getter method to access the genes string
    public string Genes
    {
        get { return genes; }
    }

    // Method to get the output of the neural network given an input array
  
}
