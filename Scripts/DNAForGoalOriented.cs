using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DNAForGoalOriented
{
    public Dictionary<string, float> genes;
    public List<float> pathTaken;
    int dnaLength;

    public DNAForGoalOriented()
    {
        genes = new Dictionary<string, float>();
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();
        genes.Add("rotation", Random.Range(-90, 90));
        genes.Add("distanceToMove", Random.Range(0f, 1f));
        dnaLength = genes.Count;
    }

     public void Combine(DNAForGoalOriented d1, DNAForGoalOriented d2)
     {
         int i = 0;
         Dictionary<string, float> newGenes = new Dictionary<string, float>();

         foreach (KeyValuePair<string, float> g in genes)
         {
             if (i < dnaLength / 2)
             {
                 newGenes.Add(g.Key, d1.genes[g.Key]);
             }
             else
             {
                 newGenes.Add(g.Key, d2.genes[g.Key]);
             }
             i++;
         }
         genes = newGenes;
     }



    public void Mutate(float mutationRate, float mutationAmount)
    {
        if (genes.Count == 0)
        {
            return;
        }

        foreach (KeyValuePair<string, float> g in genes)
        {
            if (Random.Range(0f, 1f) < mutationRate && genes.ContainsKey(g.Key))
            {
                genes[g.Key] += Random.Range(-mutationAmount, mutationAmount);
            }
        }
    }
    public float GetGene(string key)
    {
        return genes[key];
    }
   
}
