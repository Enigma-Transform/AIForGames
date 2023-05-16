using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    // The movement speed of the bot
    public float speed = 10.0f;

    // The DNA of the bot
    private float[] DNA;

    // The fitness of the bot
    private float fitness;

    // The target coin
   // private Coin coin;

    // Start is called before the first frame update
    void Start()
    {
        // Find the target coin
    //    coin = FindObjectOfType<Coin>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when the bot collides with a coin
    private void OnTriggerEnter(Collider other)
    {
    }

}
