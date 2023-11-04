using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechDebtMeteor : Asteroid
{
    public float growthRate;
    public float maxSize;
    public float startSize;
    public Asteroid miniDebt;
    private int spawnCount;

    protected override void Awake()
    {
        base.Awake();
        startSize = transform.localScale.x;
        StartCoroutine(MeteorGrowth());
    }

    public IEnumerator MeteorGrowth()
    {

        while (transform.localScale.x < maxSize)
        {
            float growth = Mathf.MoveTowards(transform.localScale.x, maxSize, Time.deltaTime * growthRate);
            transform.localScale = new Vector3(growth, growth, growth);
            float currentSize = transform.localScale.x;
            float growthRatio = (currentSize - startSize) / (maxSize - startSize);
            UpdateChildList(growthRatio);
            yield return new WaitForEndOfFrame();
        }

    }

    private void UpdateChildList(float growthRatio)
    {
        int result = growthRatio switch
        {
            > 0.1f and < 0.2f => 2,
            > 0.2f and < 0.3f => 3,
            > 0.3f and < 0.4f => 4,
            > 0.4f and < 0.5f => 5,
            > 0.5f and < 0.6f => 6,
            > 0.6f and < 0.7f => 7,
            > 0.7f and < 0.8f => 8,
            > 0.8f and < 0.9f => 9,
            > 0.9f and < 1f => 10,
            _ => 1
        };
        spawnCount = result;
    }

    protected override void SpawnBabies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(miniDebt, transform.position, transform.rotation);
        }
    }

    public override IEnumerator DieOnDelay()
    {
        SpawnBabies();
        yield return base.DieOnDelay();
    }


}
