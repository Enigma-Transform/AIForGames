using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainForATMemoryAgents : MonoBehaviour
{
    // Public variables to control movement and rotation
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 10.0f;
    public GameObject goal;
    public NeuralNetwork neuralNetwork;
    public float fitness;
    public int layers = 1;
    public int neurons = 10;
    // Method to initialize the agent with a given DNA and starting position

    private void Awake()
    {
        goal = GameObject.FindGameObjectWithTag("Goal");
        
    }

    public void InitAgent()
    {
        neuralNetwork = new NeuralNetwork();
        neuralNetwork.Init(layers, neurons);
    }
    public void ResetNN(NeuralNetwork NN)
    {
        neuralNetwork = NN;
        
    }
    private void Start()
    {
       
    }

    void FixedUpdate()
    {
        // Get the current values for a, b, and c from the bot's position and rotation
        float a = transform.position.z;
        float b = transform.rotation.eulerAngles.y;
        float c = 1f; // set this to whatever value you want

        // Call the RunNetwork function with the current input values
        if (neuralNetwork != null)
        {
            (float output1, float output2) = neuralNetwork.RunNetwork(a, b, c);
            Move(output1, output2);

        }
        else
        {
            Move(1f, 45f);
        }

        // Use the outputs to move and rotate the bot


        fitness = 1 / Vector3.Distance(transform.position, goal.transform.position);

    }

    // Function to move and rotate the bot
    void Move(float zInput, float xInput)
    {
        // Move the bot on the Z-axis
        transform.Translate(Vector3.forward * zInput * moveSpeed * Time.deltaTime);

        // Rotate the bot on the Y-axis
        transform.Rotate(Vector3.up, xInput * rotateSpeed * Time.deltaTime);
    }
}
