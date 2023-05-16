using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SmartPopulationScript : MonoBehaviour
{
    public GameObject goal;

    public GameObject agentPrefab;
    public GameObject startingPos;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 10;
    public float timeScale = 2;
    int generation = 1;
    NeuralNetwork neuralNetwork;
    public Transform startingPosition;
    GUIStyle guiStyle = new GUIStyle();
    private int[] layers = new int[] { 3, 8, 8, 2 };

    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }


    private void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Quaternion randomRotation = Random.rotation;
            Quaternion randomYRotation = new Quaternion(0, randomRotation.y, 0, randomRotation.w);
            Vector3 startingPos = new Vector3(startingPosition.transform.position.x + Random.Range(-2, 2),
                                           startingPosition.transform.position.y,
                                           startingPosition.transform.position.z + Random.Range(-2, 2));
            GameObject go = Instantiate(agentPrefab, startingPos,randomYRotation);
            go.GetComponent<BotController1>().init(layers);
            population.Add(go);
        }
    }

    /*private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Quaternion rotation;

        Vector3 startingPos = new Vector3(0, 0, 0);
        GameObject offspring = Instantiate(agentPrefab, startingPos, Quaternion.identity);


        if (Random.value < 0.5f)
        {
            rotation = parent1.GetComponent<BotController1>().currentRotation;
            offspring.GetComponent<BotController1>().currentRotation = rotation;

        }
        else
        {
            rotation = parent2.GetComponent<BotController1>().currentRotation;
            offspring.GetComponent<BotController1>().currentRotation = rotation;

        }
        return offspring;
    }*/
    /* private MyNeuralNetwork Crossover(MyNeuralNetwork parent1, MyNeuralNetwork parent2)
     {
         MyNeuralNetwork offspring = new MyNeuralNetwork(parent1.layers);

         for (int i = 0; i < parent1.weights.Length; i++)
         {
             for (int j = 0; j < parent1.weights[i].Length; j++)
             {
                 for (int k = 0; k < parent1.weights[i][j].Length; k++)
                 {
                     if (Random.value < 0.5f)
                     {
                         offspring.weights[i][j][k] = parent1.weights[i][j][k];
                     }
                     else
                     {
                         offspring.weights[i][j][k] = parent2.weights[i][j][k];
                     }
                 }
             }
         }

         for (int i = 0; i < parent1.biases.Length; i++)
         {
             for (int j = 0; j < parent1.biases[i].Length; j++)
             {
                 if (Random.value < 0.5f)
                 {
                     offspring.biases[i][j] = parent1.biases[i][j];
                 }
                 else
                 {
                     offspring.biases[i][j] = parent2.biases[i][j];
                 }
             }
         }

         return offspring;
     }*/
    private MyNeuralNetwork UniformCrossover(MyNeuralNetwork parent1, MyNeuralNetwork parent2, float mutationRate, float mutationRange)
    {
        MyNeuralNetwork offspring = new MyNeuralNetwork(parent1.layers);

        for (int i = 0; i < parent1.weights.Length; i++)
        {
            for (int j = 0; j < parent1.weights[i].Length; j++)
            {
                for (int k = 0; k < parent1.weights[i][j].Length; k++)
                {
                    if (Random.value < 0.5f)
                    {
                        offspring.weights[i][j][k] = parent1.weights[i][j][k];
                    }
                    else
                    {
                        offspring.weights[i][j][k] = parent2.weights[i][j][k];
                    }

                    // Add Gaussian mutation
                    if (Random.value < mutationRate)
                    {
                        offspring.weights[i][j][k] += Random.Range(-mutationRange, mutationRange);
                    }
                }
            }
        }

        for (int i = 0; i < parent1.biases.Length; i++)
        {
            for (int j = 0; j < parent1.biases[i].Length; j++)
            {
                if (Random.value < 0.5f)
                {
                    offspring.biases[i][j] = parent1.biases[i][j];
                }
                else
                {
                    offspring.biases[i][j] = parent2.biases[i][j];
                }

                // Add Gaussian mutation
                if (Random.value < mutationRate)
                {
                    offspring.biases[i][j] += Random.Range(-mutationRange, mutationRange);
                }
            }
        }

        return offspring;
    }

    void NewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<BotController1>().numCollisions).ThenBy(o => Vector3.Distance(goal.transform.position,o.transform.position)).ToList();
       /* List<GameObject> sortedList = population
            .OrderBy(o => o.GetComponent<BotController1>().fitness)
            .ThenBy(o => o.GetComponent<BotController1>().numCollisions)
            .ToList();*/
        population.Clear();

        while (population.Count < populationSize)
        {
            int bestCutOff = Mathf.RoundToInt(sortedList.Count * 0.04f);

            for (int i = 0; i < bestCutOff - 1; i++)
            {
                for (int j = 1; j < bestCutOff; j++)
                {
                    MyNeuralNetwork nn1 = sortedList[i].GetComponent<BotController1>().network;
                    MyNeuralNetwork nn2 = sortedList[j].GetComponent<BotController1>().network;
                    MyNeuralNetwork nnOffspring = UniformCrossover(nn1, nn2,0.1f,0.1f);

                    GameObject offspring = Instantiate(agentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    offspring.GetComponent<BotController1>().init(nnOffspring.layers);
                    offspring.GetComponent<BotController1>().network = nnOffspring;
                    population.Add(offspring);

                    if (population.Count == populationSize)
                    {
                        break;
                    }

                    nnOffspring = UniformCrossover(nn2, nn1,0.1f, 0.1f);

                    offspring = Instantiate(agentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    offspring.GetComponent<BotController1>().network = nnOffspring;
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

