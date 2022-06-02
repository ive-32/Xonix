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

            //if (ctp2dlist[i].collider.gameObject.name == "FloorTilePrefab*")
            if (ctp2dlist[i].collider.gameObject.transform.parent == FieldTiles.transform)
            {
                Object.Destroy(ctp2dlist[i].collider.gameObject);
                // todo add destroy effect here. Tmp prefab with animation;
            }
        }
    }
}
