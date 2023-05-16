using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public Dictionary<bool, float> genes;
    int dnaLength;
    float distanceTravelled;

   public DNA()
    {
        genes = new Dictionary<bool, float>();
        SetRandom();
        distanceTravelled = 0f; // Initialize distance travelled to 0

    }


    public void SetRandom()
    {
        genes.Clear();
        genes.Add(false, Random.Range(-90,90));


        genes.Add(true, Random.Range(-90,91));
        dnaLength = genes.Count;
    }

    public void Combine(DNA d1, DNA d2)
    {
        int i = 0;
        Dictionary<bool, float> newGenes = new Dictionary<bool, float>();

        foreach(KeyValuePair<bool,float> g in genes)
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

    public float GetGene(bool front)
    {
        return genes[front];
    }

}


