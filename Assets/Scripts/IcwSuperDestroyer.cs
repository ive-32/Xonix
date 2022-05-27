using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class IcwSuperDestroyer : IcwEnemy
{
    void DestroyTiles(Collision2D collision)
    {
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);
        Vector2 collisionnormal = Vector2.zero;
        foreach (ContactPoint2D ctp2d in ctp2dlist)
        {
            Vector2 tilecenter = ctp2d.point - 0.8f * ctp2d.normal;
            Vector3Int tileposition = floor.WorldToCell(new Vector3(tilecenter.x, tilecenter.y, 0));// ctp2d.point - 0.7f * ctp2d.normal);
            if (!IcwService.IsInField(tileposition))
            {
                collisionnormal += ctp2d.normal;
                continue;
            }
            TileBase tb = floor.GetTile(tileposition);
            if (tb == null) continue;
            if (tb.name == "FloorTile") game.PlaceFloorTile(tileposition.x, tileposition.y, null);
        }
        //if (collisionnormal != Vector2.zero)
        //    enemyvelocity = Vector2.Reflect(enemyvelocity, collisionnormal.normalized);//*/
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 tmpvel = enemyvelocity;
        base.OnCollisionEnter2D(collision);
        enemyvelocity = tmpvel;
        //DestroyTiles(collision);
        
        List<ContactPoint2D> ctp2dlist = new();
        collision.GetContacts(ctp2dlist);

        Vector2 collisionnormal = Vector2.zero;
        foreach (ContactPoint2D ctp2d in ctp2dlist)
        {
            Vector2 tilecenter = ctp2d.point - 1.0f * ctp2d.normal;
            Vector3Int tileposition = floor.WorldToCell(new Vector3(tilecenter.x, tilecenter.y, 0));// ctp2d.point - 0.7f * ctp2d.normal);
            if (!IcwService.IsInField(tileposition))
            {
                collisionnormal += ctp2d.normal;
                continue;
            }
        }
        if (collisionnormal != Vector2.zero)
            enemyvelocity = Vector2.Reflect(enemyvelocity, collisionnormal.normalized);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        DestroyTiles(collision);
    }

}
