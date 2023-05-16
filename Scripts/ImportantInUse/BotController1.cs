using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController1 : MonoBehaviour
{
    // Variables for the neural network
    public MyNeuralNetwork network;
    private int[] layers = new int[] { 3, 8, 8, 2 };
    
    //variables responsible for fitness of the AT agent
    public float distanceTravelled = 0f;
    public int numCollisions = 0;
    Vector3 lastPosition;

    // Other variables
    private Rigidbody rb;
    //Inputs for the neural network
    private float[] inputs = new float[3];
    private float[] outputs;
    
    Vector3 startPos;
    
    public Transform wallCheck;
    public float maxDistance = 8f;
    public int numRays = 3;
    public float raySpreadAngle = 60f;
    float rotation;
    public float rotationSpeed = 60f; // degrees per second
    private bool obstacleDetected = false; // new variable for obstacle detection
   public float fitness;
  public  Transform goal;
    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();

    private void Start()
    {
    }
    public void init(int[] layers)
    {
        network = new MyNeuralNetwork(layers);
        startPos = transform.position;

    }
    private void Update()
    {
      
          
            distanceTravelled += Vector3.Distance(startPos, transform.position);
      

    }
    /*  void FixedUpdate()
      {
          distanceTravelled +=  Time.deltaTime;

          // Update inputs
          for (int i = 0; i < numRays; i++)
          {
              float angle = (i - (numRays - 1) / 2f) / (numRays - 1) * raySpreadAngle;
              Quaternion rayRotation = Quaternion.AngleAxis(angle, Vector3.up);
              Vector3 rayDirection = rayRotation * transform.forward;
              RaycastHit hit;
              if (Physics.Raycast(wallCheck.position, rayDirection, out hit, maxDistance))
              {
                  if(hit.collider.tag == "Wall")
                  {
                      obstacleDetected = true; // set obstacleDetected to true if an obstacle is hit

                      numCollisions++;

                      // Check if the ray is too close
                      if (hit.distance <= 5f)
                      {
                          // Pass a small negative value to the NN as input
                          inputs[i] = 1f;
                      }
                      else
                      {
                          Debug.DrawRay(wallCheck.position, rayDirection * hit.distance, Color.red);

                          inputs[i] = hit.distance / maxDistance;
                          obstacleDetected = false;

                      }
                  }

              }
              else
              {
                  inputs[i] = 0f;
                  Debug.DrawRay(wallCheck.position, rayDirection * maxDistance, Color.green);
              }
          }

          // Pass inputs through the neural network to get outputs
          outputs = network.FeedForward(inputs);

          // Move the agent based on the outputs
          float acceleration = outputs[0];
          float forwardSpeed = 8f;
          if (acceleration < 0) // if acceleration is negative, multiply it by -1 to make it positive
          {
              acceleration *= -1;
          }
          transform.Translate(Vector3.forward * acceleration * forwardSpeed * Time.deltaTime);
          float rotation = outputs[1];

          if (obstacleDetected) // rotate only if an obstacle is detected
          {
              float yRotation = rotation * rotationSpeed * Time.deltaTime;
              transform.Rotate(Vector3.up, yRotation);
          }
      }*/


    void FixedUpdate()
    {
       
        if (goal == null)
        {
            goal = GameObject.FindGameObjectWithTag("Goal").transform;
        }
            float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position);
        distanceTravelled = Vector3.Distance(transform.position, startPos);

        fitness = 1 / (distanceTravelled+distanceToGoal); // the closer the bot to the goal, the higher the fitness
    

    // Update inputs
    bool rotate = false;
        for (int i = 0; i < numRays; i++)
        {
            float angle = (i - (numRays - 1) / 2f) / (numRays - 1) * raySpreadAngle;
            Quaternion rayRotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rayDirection = rayRotation * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(wallCheck.position, rayDirection, out hit, maxDistance))
            {
                if (hit.collider.tag == "Wall")
                {
                    

                    if (hit.distance <= 5f)
                    {
                        inputs[i] = 1f;
                    }
                    else
                    {
                        inputs[i] = hit.distance / maxDistance;
                    }
                    Debug.DrawRay(wallCheck.position, rayDirection * hit.distance, Color.red);
                    rotate = true;
                }

            }
            else
            {
                inputs[i] = 0f;
                Debug.DrawRay(wallCheck.position, rayDirection * maxDistance, Color.green);
            }
        }

        // Pass inputs through the neural network to get outputs
        outputs = network.FeedForward(inputs);

        // Move the agent based on the outputs
        float acceleration = outputs[0];
        float forwardSpeed = 8f;
       
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        if (rotate)
        {
            float rotation = outputs[1];
            float yRotation = 0.0f;

           
                yRotation = rotation * rotationSpeed * Time.deltaTime; // rotate left
            
         

            transform.Rotate(Vector3.up, yRotation);
        }
    }
    

    public int GetNumCollisions()
    {
        return numCollisions;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        fitness = 0f;
        distanceTravelled = 0f;
        obstacleDetected = false;
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

            Reset();
        }
    }
}
