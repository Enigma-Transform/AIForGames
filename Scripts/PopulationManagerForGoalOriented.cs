using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PopulationManagerForGoalOriented : MonoBehaviour
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

    private void Awake()
    {
        goal = GameObject.FindGameObjectWithTag("Goal");
        startingPosition = GameObject.FindGameObjectWithTag("StartPos");
    }
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
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startingPos = new Vector3(startingPosition.transform.position.x + Random.Range(-2, 2),
                                    startingPosition.transform.position.y,
                                    startingPosition.transform.position.z + Random.Range(-2, 2));

            GameObject bot = Instantiate(botPrefab, startingPos, this.transform.rotation);
            bot.GetComponent<BrainForGoalOriented>().Init();

            population.Add(bot);
        }
    }


    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(startingPosition.transform.position.x + Random.Range(-2, 2),
                                           startingPosition.transform.position.y,
                                           startingPosition.transform.position.z + Random.Range(-2, 2));
        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        BrainForGoalOriented brain = offspring.GetComponent<BrainForGoalOriented>();


        if (Random.Range(0, 100) == 1) // mutation
        {
            brain.Init();
            brain.dnaG.Combine(parent1.GetComponent<BrainForGoalOriented>().dnaG, parent2.GetComponent<BrainForGoalOriented>().dnaG);
            brain.dnaG.Mutate(0.01f, 0.1f);
        }
        else
        {
            brain.Init();
            brain.dnaG.Combine(parent1.GetComponent<BrainForGoalOriented>().dnaG, parent2.GetComponent<BrainForGoalOriented>().dnaG);

        }
        return offspring;
    }

    
 
    void NewPopulation()
    {
        // List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().coinsCollected + o.GetComponent<Brain>().distanceTravelled).ToList();
        List<GameObject> sortedList = population.OrderBy(o => Vector3.Distance(o.transform.position, goal.transform.position)).ToList();

        population.Clear();

        while (population.Count < populationSize)
        {
            int bestCutOff = sortedList.Count / 4;
            if (bestCutOff < 2) bestCutOff = 2; // Set a minimum cutoff of 2

            for (int i = 0; i < bestCutOff - 1; i++)
            {
                for (int j = 1; j < bestCutOff; j++)
                {
                    
                        population.Add(Breed(sortedList[i], sortedList[j]));
                        
                    if (population.Count == populationSize)
                    {
                        break;
                    }

                    population.Add(Breed(sortedList[j], sortedList[i]));

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
