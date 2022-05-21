using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwEnemyDestroyer : IcwEnemy
{
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        
        base.OnCollisionEnter2D(collision);
        List<ContactPoint2D> ctp2dlist = new List<ContactPoint2D>();
        int countofcontactpoints = collision.GetContacts(ctp2dlist);
        foreach(ContactPoint2D ctp2d in ctp2dlist)
        {
            Vector3Int tileposition = floor.WorldToCell(ctp2d.point - 0.1f * ctp2d.normal);
            if (!IcwGameClass.IsInField(tileposition)) continue;
            TileBase tb = floor.GetTile(tileposition);
            if (tb == null) continue;
            if (tb.name == "FloorTile") floor.SetTile(tileposition, null);
        }
    }
}
