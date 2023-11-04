using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffEdges : MonoBehaviour
{

    public float bounceMultiplier = 2f;

    public float minBound = 0.02f;
    public float maxBound = 0.98f;

    private Rigidbody2D rigidBody;

    public bool reorientOnBounce = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        float drag = rigidBody.drag;

        if(drag <= 0)
        {
            bounceMultiplier = 1;
        }
    }

    private void FixedUpdate()
    {
        Bounce();
    }

    private void Bounce()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        if (position.x < minBound)
        {
            rigidBody.velocity = Vector3.Reflect(rigidBody.velocity * bounceMultiplier, Vector3.right);
            RotationToFaceVelocity();
        }

        if (position.x > maxBound)
        {
            rigidBody.velocity = Vector3.Reflect(rigidBody.velocity * bounceMultiplier, Vector3.left);
            RotationToFaceVelocity();
        }

        if (position.y < minBound)
        {
            rigidBody.velocity = Vector3.Reflect(rigidBody.velocity * bounceMultiplier, Vector3.up);
            RotationToFaceVelocity();
        }

        if (position.y > maxBound)
        {
            rigidBody.velocity = Vector3.Reflect(rigidBody.velocity * bounceMultiplier, Vector3.down);
            RotationToFaceVelocity();

        }


    }

    public void RotationToFaceVelocity()
    {
        if(reorientOnBounce == false)
        {
            return;
        }
        transform.up = rigidBody.velocity.normalized;
    }
}
