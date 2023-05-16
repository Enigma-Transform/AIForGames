using UnityEngine;

public class DNAWITHMEMORY
{
    public Color color;
    public float fitness;
    public float[] memory;

    public DNAWITHMEMORY()
    {
        color = new Color(Random.value, Random.value, Random.value);
        fitness = 0;
        memory = new float[10];
        for (int i = 0; i < 10; i++)
        {
            memory[i] = Random.Range(-1f, 1f);
        }
    }

    public void Combine(DNAWITHMEMORY parent1, DNAWITHMEMORY parent2, float blendFactor)
    {
        color = Blend(parent1.color, parent2.color, blendFactor);
    }

    public void Mutate(float mutationRate)
    {
        if (Random.value < mutationRate)
        {
            color.r = Mathf.Clamp(color.r + Random.Range(-0.1f, 0.1f), 0f, 1f);
        }
        if (Random.value < mutationRate)
        {
            color.g = Mathf.Clamp(color.g + Random.Range(-0.1f, 0.1f), 0f, 1f);
        }
        if (Random.value < mutationRate)
        {
            color.b = Mathf.Clamp(color.b + Random.Range(-0.1f, 0.1f), 0f, 1f);
        }
    }

    private float Blend(float a, float b, float blendFactor)
    {
        return Mathf.Lerp(a, b, blendFactor);
    }

    private Color Blend(Color a, Color b, float blendFactor)
    {
        float r = Blend(a.r, b.r, blendFactor/2f);
        float g = Blend(a.g, b.g, blendFactor/2f);
        float b2 = Blend(a.b, b.b, blendFactor/2f);
        return new Color(r, g, b2, 1f);
    }
}
