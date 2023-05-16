using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class JumpingTest : MonoBehaviour
{
    public float maxHeight = 5f;

    private float jumpHeight; // Customize jump height here
    [SerializeField] private Transform groundedCheck; // Ground check transform
    [SerializeField] private float groundedRadius = 0.1f; // Radius of the grounded check sphere

    private Rigidbody rb;
    public bool isGrounded;
    public bool isJumping;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Calculate jump height based on mass and other Rigidbody settings
        float gravity = Physics.gravity.magnitude;
        float mass = rb.mass;
        float rigidbodyDrag = rb.drag;
        jumpHeight = (2 * gravity * mass * maxHeight) / rigidbodyDrag;

    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, 0.2f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
            }
        }
        else
        {
            if (!isJumping && isGrounded)
            {
                isJumping = true;
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }
}



