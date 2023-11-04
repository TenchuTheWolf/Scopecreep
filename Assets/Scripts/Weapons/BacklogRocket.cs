using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacklogRocket : Projectile
{
    public BacklogRocketPayload payload;
    public GameObject deathVFXPrefab;


    public void BacklogRocketDeath()
    {
        Instantiate(payload, transform.position, Quaternion.identity);
    }

    protected override void CleanUp()
    {
        BacklogRocketDeath();

        GameObject activeVFX = Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        Destroy(activeVFX, 2f);

        base.CleanUp();
    }
}
