using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemy : MonoBehaviour
{
    protected Rigidbody2D rg2d;
    Vector3 lastvelocity;
    protected Tilemap floor;
    protected IcwGame game;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        game = GameObject.Find("mainGame").GetComponent<IcwGame>();
        rg2d = GetComponent<Rigidbody2D>();
        floor = Object.FindObjectOfType<Tilemap>();
        Vector3 vel = Random.insideUnitCircle.normalized * 5.0f;
        rg2d.velocity = vel;
    }


    private void FixedUpdate()
    {
        rg2d.velocity = rg2d.velocity.normalized * 5.0f;
        lastvelocity = rg2d.velocity;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);
        Vector2 collisionnormal = Vector2.zero;
        foreach (ContactPoint2D ctp2d in ctp2dlist)
        {
            collisionnormal += ctp2d.normal;
            Vector3Int tileposition = floor.WorldToCell(ctp2d.point - 0.1f * ctp2d.normal);
            if (floor.GetTile(tileposition) == game.tracetile)
            {   //PlayerTrace hit by Enemy
                game.PlayerWasHit();
            }
        }
        rg2d.velocity = Vector3.Reflect(lastvelocity, collisionnormal.normalized);

    }   
}
