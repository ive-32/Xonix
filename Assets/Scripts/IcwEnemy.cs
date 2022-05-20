using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwEnemy : MonoBehaviour
{
    Rigidbody2D rg2d;
    Vector3 lastvelocity;

    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        lastvelocity = rg2d.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rg2d.velocity = Vector3.Reflect(lastvelocity, collision.contacts[0].normal);
    }

   
}
