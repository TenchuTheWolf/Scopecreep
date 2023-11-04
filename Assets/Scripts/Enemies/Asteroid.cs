using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : NPC
{
    public float driftSpeed;
    public List<Asteroid> spawns = new List<Asteroid>();
    

    protected override void Awake()
    {
        base.Awake();
        SetupMotion();

        if(rigidBody == null)
        {
            Debug.LogError("A rigidbody script was missing on this asteroid.");
        }
    }

    protected override void Start()
    {
        base.Start();

        if(spawnVFX == null)
        {
            return;
        }

        GameObject activeVFX = Instantiate(spawnVFX, transform.position, transform.rotation);
        Destroy(activeVFX, 2f);
    }

    private void SetupMotion()
    {

        float randomRange = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRange);
        rigidBody.AddForce(transform.up * driftSpeed, ForceMode2D.Impulse);


        float randomSpin = Random.Range(-360f, 360f);    
        rigidBody.angularVelocity = randomSpin;

    }


    protected virtual void SpawnBabies()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            Instantiate(spawns[i], transform.position, transform.rotation);
        }
    }

    public override IEnumerator DieOnDelay()
    {
        SpawnBabies();
        yield return base.DieOnDelay();
    }

}
