using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemyDestroyer : IcwEnemy
{
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("FilledFloor"))
        {
            Vector3 pos = floor.WorldToCell(collision.gameObject.transform.position);
            if (pos.x>1&&pos.y>1&&pos.x<IcwGameClass.sizeX-2&& pos.y < IcwGameClass.sizeY - 2)
            {
                Object.Destroy(collision.gameObject);
            }
        }
    }
}
