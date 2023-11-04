using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : Pickup
{
    public int currencyValue = 1;
    public float driftSpeed = 10f;

    protected override void Start()
    {
        base.Start();
  
        Rigidbody2D myBody = GetComponent<Rigidbody2D>();
        float randomRange = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRange);
        myBody.AddForce(transform.up * driftSpeed, ForceMode2D.Impulse);
    }


    protected override void Collect()
    {
        base.Collect();
        Player.playerInstance.AddCurrency(currencyValue);
        AudioManager.PlaySound("Design Doc Pickup SFX", .75f);
    }
}
