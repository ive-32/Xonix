using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class IcwSuperDestroyer : IcwEnemyDestroyer
{
    
    Vector2 tmpvel;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 newpoint = rg2d.position + 0.1f * Time.fixedDeltaTime * rg2d.velocity;
        for (int i = FieldTiles.transform.childCount-1; i>-1;  i--)
        {
            if (Vector3.Distance(FieldTiles.transform.GetChild(i).position, newpoint) < 2.2f)
                Object.Destroy(FieldTiles.transform.GetChild(i).gameObject);
        }
    }

}
