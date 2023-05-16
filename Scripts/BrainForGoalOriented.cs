using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainForGoalOriented : MonoBehaviour
{
    public DNAForGoalOriented dnaG;
    public GameObject groundedCheck;
    bool wallCheck = false;

    public LayerMask ignore = 6;
    bool canMove = false;
    public float speed =5f;
    public void Init()
    {
        dnaG = new DNAForGoalOriented();

    }


    void Update()
    {

        RaycastHit hit;

        Debug.DrawRay(groundedCheck.transform.position, groundedCheck.transform.forward * 1f, Color.red);

        if (Physics.SphereCast(groundedCheck.transform.position, 0.1f, groundedCheck.transform.forward, out hit, 1f, ~ignore))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                wallCheck = true;
            }
            else
            {
                wallCheck = false;
            }
        }
    }


    void FixedUpdate()
    {

        if (wallCheck)
        {
            this.transform.Rotate(0, dnaG.genes["rotation"], 0);

        }

        this.transform.Translate(0, 0, dnaG.genes["distanceToMove"]*Time.deltaTime *speed);


    }
}

