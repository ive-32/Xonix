using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwBaseTile : MonoBehaviour
{
    public IcwGrid.FieldObjectsTypes objtype;
    [System.NonSerialized] public Vector3Int tilepos;

    protected virtual void Start()
    {
        tilepos = IcwObjects.floor.WorldToCell(transform.position);
        if (objtype != IcwGrid.FieldObjectsTypes.Empty)
            if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY))
                IcwObjects.gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)objtype;
    }

    public virtual void Update()
    {

    }
    public virtual void DestroyTile()
    {
        Object.Destroy(this.gameObject);
        if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY)
            && (IcwObjects.gridclass.fieldprojection[tilepos.x, tilepos.y] == (int)objtype))
            IcwObjects.gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)IcwGrid.FieldObjectsTypes.Empty;
    }

}
