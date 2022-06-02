using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwTraceTilePrefab : IcwBaseTile
{
    protected override void Start()
    {
        objtype = IcwGrid.FieldObjectsTypes.Trace;
        base.Start();
        transform.SetParent(gridclass.TraceTiles.transform);
    }
}
