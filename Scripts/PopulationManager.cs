using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PopulationManager : MonoBehaviour {

	public GameObject botPrefab;
	public GameObject[] startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 10;
	public float timeScale = 2;
	int generation = 1;
	public GenerateMaze maze;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect (10,25,200,30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect (10,50,200,30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
		GUI.Label(new Rect (10,75,200,30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup ();
	}


	private void Start()
	{
		for(int i = 0; i < populationSize; i++)
		{
			GameObject go = Instantiate(botPrefab, startingPos[Random.Range(0,startingPos.Length)].transform.position, Quaternion.identity);
			go.GetComponent<Brain>().Init();
			go.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
			population.Add(go);
		}
	}

     GameObject Breed(GameObject parent1, GameObject parent2)
     {
         GameObject offspring = Instantiate(botPrefab, startingPos[Random.Range(0, startingPos.Length)].transform.position, Quaternion.identity);
         Brain b = offspring.GetComponent<Brain>();

         if (Random.Range(0, 100) == 1)//mutate
         {
             b.Init();
         }
         else
         {
             b.Init();
             if (parent1.GetComponent<Brain>().coinsCollected > 0 || parent2.GetComponent<Brain>().coinsCollected > 0)
             {
                 Renderer renderer = offspring.GetComponent<Renderer>();
                 renderer.material.color = Color.green; // Change the color of the offspring to green
             }
             b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
         }

         return offspring;
     }




   void NewPopulation()
{
    List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().coinsCollected + o.GetComponent<Brain>().distanceTravelled).ToList();

    string coinsCollected = "Generation: " + generation;
    string distanceTravelled = "Distance Travelled: " + generation;

    foreach (GameObject go in sortedList)
    {
        coinsCollected += ", " + go.GetComponent<Brain>().coinsCollected;
        distanceTravelled += ", " + go.GetComponent<Brain>().distanceTravelled;
    }

    Debug.Log(coinsCollected);
    Debug.Log(distanceTravelled);

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
