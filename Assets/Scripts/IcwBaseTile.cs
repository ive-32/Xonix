using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwBaseTile : MonoBehaviour
{
    public IcwGrid.FieldObjectsTypes objtype;
    [System.NonSerialized] public IcwGrid gridclass;
    [System.NonSerialized] public Vector3Int tilepos;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        gridclass = GameObject.Find("Grid").GetComponent<IcwGrid>();
        tilepos = gridclass.floor.WorldToCell(transform.position);
        if (objtype != IcwGrid.FieldObjectsTypes.Empty)
            if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY))
                gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)objtype;
    }

    protected virtual void OnDestroy()
    {
        if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY)
            && (gridclass.fieldprojection[tilepos.x, tilepos.y] == (int)objtype))
            gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)IcwGrid.FieldObjectsTypes.Empty;
    }
}