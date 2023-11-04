using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : NPC
{

    public float speed = 5f;

    public float frequency;
    public float amplitude;

    protected override void Awake()
    {
        base.Awake();

    }
    private void FixedUpdate()
    {
        rigidBody.AddForce(Vector2.right * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        float sinFloat = Mathf.Sin(Time.fixedTime * frequency) * amplitude;

        rigidBody.AddForce(Vector2.up * sinFloat, ForceMode2D.Impulse);
    }

    private void Move()
    {

    }

    private void SlowPlayer()
    {

    }




}
