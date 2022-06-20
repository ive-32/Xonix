using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwBaseTile : MonoBehaviour
{
    public IcwGrid.FieldObjectsTypes objtype;
    [System.NonSerialized] public IcwGrid gridclass;
    [System.NonSerialized] public Vector3Int tilepos;

    protected virtual void Awake()
    {
        gridclass = GameObject.Find("Grid").GetComponent<IcwGrid>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        tilepos = gridclass.floor.WorldToCell(transform.position);
        if (objtype != IcwGrid.FieldObjectsTypes.Empty)
            if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY))
                gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)objtype;
    }

    public virtual void DestroyTile()
    {
        Object.Destroy(this.gameObject);
        if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY)
            && (gridclass.fieldprojection[tilepos.x, tilepos.y] == (int)objtype))
            gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)IcwGrid.FieldObjectsTypes.Empty;
    }

    /*protected virtual void OnDestroy()
    {
        if ((tilepos.x > -1 && tilepos.x < IcwGame.sizeX) && (tilepos.y > -1 && tilepos.y < IcwGame.sizeY)
            && (gridclass.fieldprojection[tilepos.x, tilepos.y] == (int)objtype))
            gridclass.fieldprojection[tilepos.x, tilepos.y] = (int)IcwGrid.FieldObjectsTypes.Empty;
    }*/
}
