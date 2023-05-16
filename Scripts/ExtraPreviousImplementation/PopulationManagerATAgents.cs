using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PopulationManagerATAgents : MonoBehaviour
{
    public GameObject botPrefab;
    public GameObject startingPosition;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 10;
    public float timeScale = 2;
    int generation = 1;
    private GUIStyle guiStyle = new GUIStyle();
    public GameObject goal;
   
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.red;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        // Calculate number of rows and columns required for the population size
        int numColumns = Mathf.CeilToInt(Mathf.Sqrt(populationSize));
        int numRows = Mathf.CeilToInt((float)populationSize / numColumns);

        // Calculate the spacing between agents in the grid
        float spacingX = 4f / (numColumns - 1);
        float spacingZ = 4f / (numRows - 1);

        // Instantiate agents in the grid
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                // Calculate the position of the current agent
                float x = -2f + j * spacingX;
                float z = -2f + i * spacingZ;
                Vector3 startingPos = new Vector3(x, startingPosition.transform.position.y, z);

                GameObject bot = Instantiate(botPrefab, startingPos, this.transform.rotation);
                bot.GetComponent<BrainWithMemory>().Init();

                population.Add(bot);

                // Stop creating agents if population size is reached
                if (population.Count == populationSize)
                {
                    return;
                }
            }
        }
    }



    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 parent1Pos = parent1.transform.position;
        Vector3 parent2Pos = parent2.transform.position;

        // Calculate the position of the offspring based on the parent positions and blend factor
        Vector3 startingPos = Vector3.Lerp(parent1Pos, parent2Pos, 0.4f);

        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        BrainWithMemory brain = offspring.GetComponent<BrainWithMemory>();

        brain.Init();
        brain.dnaG.Combine(parent1.GetComponent<BrainWithMemory>().dnaG, parent2.GetComponent<BrainWithMemory>().dnaG, 0.4f);

        // Blend the parent bots' materials' colors
        Material parent1Mat = parent1.GetComponent<Renderer>().material;
        Material parent2Mat = parent2.GetComponent<Renderer>().material;
        Color blendedColor = Color.Lerp(parent1Mat.color, parent2Mat.color, 0.4f);
        offspring.GetComponent<Renderer>().material.color = blendedColor;

        return offspring;
    }



    float ColorDistance(Color c1, Color c2)
    {
        float r = c1.r - c2.r;
        float g = c1.g - c2.g;
        float b = c1.b - c2.b;
        return Mathf.Sqrt(r * r + g * g + b * b);
    }
    void NewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => ColorDistance(o.GetComponent<Renderer>().material.color, goal.GetComponent<Renderer>().material.color)).ToList();
        population.Clear();

        float diversity = 0f;
        for (int i = 0; i < sortedList.Count; i++)
        {
            for (int j = i + 1; j < sortedList.Count; j++)
            {
                diversity += ColorDistance(sortedList[i].GetComponent<Renderer>().material.color, sortedList[j].GetComponent<Renderer>().material.color);
            }
        }
        diversity /= sortedList.Count * (sortedList.Count - 1) / 2;

        while (population.Count < populationSize)
        {
            int bestCutOff = sortedList.Count / 4;
            if (bestCutOff < 2) bestCutOff = 2; // Set a minimum cutoff of 2

            for (int i = 0; i < bestCutOff - 1; i++)
            {
                for (int j = 1; j < bestCutOff; j++)
                {
                    GameObject offspring = Breed(sortedList[i], sortedList[j]);

                    float mutationRate = Mathf.Clamp01(diversity * 0.01f); // Decrease mutation rate to slow down color changes
                    offspring.GetComponent<BrainWithMemory>().dnaG.Mutate(mutationRate);

                    population.Add(offspring);

                    if (population.Count == populationSize)
                    {
                        break;
                    }

                    offspring = Breed(sortedList[j], sortedList[i]);

                    mutationRate = Mathf.Clamp01(diversity * 0.5f); // Decrease mutation rate to slow down color changes
                    offspring.GetComponent<BrainWithMemory>().dnaG.Mutate(mutationRate);

                    population.Add(offspring);

                    if (population.Count == populationSize)
                    {
                        break;
                    }
                }

                if (population.Count == populationSize)
                {
                    break;
                }
            }
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;
    }








    private void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= trialTime)
        {
            NewPopulation();
            elapsed = 0;
        }
    }
}
