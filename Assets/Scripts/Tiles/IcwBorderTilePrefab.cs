using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class IcwBorderTilePrefab : IcwBaseTile
{
    protected override void Start()
    {
        objtype = IcwGrid.FieldObjectsTypes.Border;
        base.Start();
        transform.SetParent(IcwObjects.BorderTiles.transform);
    }

}
