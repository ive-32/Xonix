using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwBorderTilePrefab : IcwBaseTile
{
    protected override void Start()
    {
        objtype = IcwGrid.FieldObjectsTypes.Border;
        base.Start();
        transform.SetParent(gridclass.BorderTiles.transform);
    }

}
