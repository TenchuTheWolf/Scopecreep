using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;
    public float bulletLifetime = 2f;

    private Rigidbody2D proj;


    protected virtual void Awake()
    {
        proj = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Invoke(nameof(CleanUp), bulletLifetime);
    }

    private void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {
        Vector2 moveForce = transform.up * moveSpeed * Time.fixedDeltaTime;
        proj.AddForce(moveForce, ForceMode2D.Force);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            CleanUp();
        }
    }

    protected virtual void CleanUp()
    {
        Destroy(gameObject);
    }
}
