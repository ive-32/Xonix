using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;


public class IcwGrid : MonoBehaviour
{
    public enum FieldObjectsTypes {Empty = 0, Trace = 5, Field = 10, Border = 20};
    public GameObject fieldtileprefab;
    public GameObject tracetileprefab;
    public GameObject bordertileprefab;


    [System.NonSerialized] public int[,] fieldprojection; 

    private GameObject AddObjectAtTile(GameObject obj, Vector2Int _coord)
    {
        GameObject body = Instantiate(obj, IcwObjects.floor.GetCellCenterWorld(((Vector3Int)_coord)), Quaternion.identity);
        return body;
    }

    public void AddTile(float worldx, float worldy, FieldObjectsTypes newobject)
    {
        Vector3Int tilepos = IcwObjects.floor.WorldToCell(new Vector3(worldx, worldy, 0));
        switch ((int)newobject)
        {
            case (int)FieldObjectsTypes.Border: AddBorderTile(tilepos.x, tilepos.y); break;
            case (int)FieldObjectsTypes.Field: AddFieldTile(tilepos.x, tilepos.y); break;
            case (int)FieldObjectsTypes.Trace: AddTraceTile(tilepos.x, tilepos.y); break;
        }
    }

    public void AddBorderTile(int x, int y)
    {
        AddObjectAtTile(bordertileprefab, new Vector2Int(x, y));
    }

    public void AddFieldTile(int x, int y)
    {
        AddObjectAtTile(fieldtileprefab, new Vector2Int(x, y));
    }

    public void AddTraceTile(int x, int y)
    {
        AddObjectAtTile(tracetileprefab, new Vector2Int(x, y));
    }

    public void ClearTraceInFieldProjection(FieldObjectsTypes newvalue = FieldObjectsTypes.Empty)
    {
        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeX; j++)
                if (fieldprojection[i, j] == (int)FieldObjectsTypes.Trace) 
                    fieldprojection[i, j] = (int)newvalue;
    }

    public void SetBorders()
    {
        for (int i = 0; i < IcwGame.sizeX; i++)
        {
            for (int j = 0; j < 2; j++) AddBorderTile(i, j);
            for (int j = IcwGame.sizeY - 2; j < IcwGame.sizeY; j++) AddBorderTile(i, j);
        }

        for (int j = 2; j < IcwGame.sizeY - 2; j++)
        {
            for (int i = 0; i < 2; i++) AddBorderTile(i, j);
            for (int i = IcwGame.sizeX - 2; i < IcwGame.sizeX; i++) AddBorderTile(i, j);
        }

    }

    public void ChangeTraceObjects(IcwGrid.FieldObjectsTypes newobject = IcwGrid.FieldObjectsTypes.Empty)
    {
        int childcount = IcwObjects.TraceTiles.transform.childCount;
        for (int i = childcount - 1; i > -1; i--)
        {
            Vector3 pos = IcwObjects.TraceTiles.transform.GetChild(i).position;
            IcwObjects.TraceTiles.transform.GetChild(i).gameObject.GetComponent<IcwTraceTilePrefab>().DestroyTile();
            if (newobject != IcwGrid.FieldObjectsTypes.Empty)
                IcwObjects.gridclass.AddTile(pos.x, pos.y, newobject);
        }
    }

    public void PrepareLevel()
    {
        fieldprojection = new int[IcwGame.sizeX, IcwGame.sizeY];
        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeY; j++) fieldprojection[i, j] = 0;
        
        
        SetBorders();
        IcwObjects.playerclass.SetPlayerPos(IcwGame.sizeX / 2, 1);
    }



}
