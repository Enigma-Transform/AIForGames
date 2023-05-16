using UnityEngine;

public class Brain : MonoBehaviour
{
    public DNA dna;
    public GameObject groundedCheck;
    bool wallCheck;

    public float coinsCollected=0;
    public LayerMask ignore =6;
    bool canMove = false;
    public float distanceTravelled = 0f;


    public void Init()
    {
        dna = new DNA();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinsCollected++;
           // other.gameObject.SetActive(false);
        }

        Debug.Log(other.tag);
    }

    void Update()
    {
        wallCheck = true;
        canMove = true;
        RaycastHit hit;

        Debug.DrawRay(groundedCheck.transform.position, groundedCheck.transform.forward * 1f,Color.red);

        if(Physics.SphereCast(groundedCheck.transform.position, 0.1f, groundedCheck.transform.forward, out hit, 1f,~ignore))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                wallCheck = true;
                canMove = true;
            }
            else
            {
                wallCheck = false;
                canMove = false;
            }
        }
    }


    void FixedUpdate()
    {
        this.transform.Rotate(0, dna.genes[canMove], 0);

        if (canMove)
        {
            this.transform.Translate(0, 0f, 0.1f);

            distanceTravelled += 0.1f; // Update the distance travelled

        }
        
    }
}

