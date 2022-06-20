using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class IcwEnemyDestroyer : IcwEnemy
{

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        
        base.OnCollisionEnter2D(collision);
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);
        
        for (int i = ctp2dlist.Count-1; i>-1; i--)
        {
            if (ctp2dlist[i].collider.gameObject.transform.parent == FieldTiles.transform)
            {
                ctp2dlist[i].collider.gameObject.GetComponent<IcwFloorTilePrefab>().DestroyTile();
                //Object.Destroy(ctp2dlist[i].collider.gameObject);
            }
            else agro++;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (agro > 10 && this.GetType().FullName == "IcwEnemyDestroyer" ) // only EnemyDestroyer can generate SuperDestroyer
        {
            //Debug.LogWarning(this.name + " | " + this.GetType().FullName);
            GameObject body = Instantiate(game.enemypool[IcwGame.EnemyByName("EnemySuperDestroyer")]);
            body.transform.position = transform.position;
            IcwEnemy tmpenobj = body.GetComponent<IcwEnemy>();
            tmpenobj.rg2d.velocity = rg2d.velocity;
            tmpenobj.saveMyParameters = true;
            tmpenobj.timetolive = 5;
            tmpenobj.parent = this.gameObject;
            agro = 0;
            this.gameObject.SetActive(false);
        }
    }
}
