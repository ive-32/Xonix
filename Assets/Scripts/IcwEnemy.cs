using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemy : MonoBehaviour
{
    protected Rigidbody2D rg2d;
    Vector3 lastvelocity;
    protected Tilemap floor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        floor = Object.FindObjectOfType<Tilemap>();
    }


    private void FixedUpdate()
    {
        lastvelocity = rg2d.velocity;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        rg2d.velocity = Vector3.Reflect(lastvelocity, collision.contacts[0].normal);
    }

   
}
