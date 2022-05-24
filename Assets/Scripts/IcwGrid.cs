using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Assets.Scripts;


public class IcwGrid : MonoBehaviour
{
    public GameObject basetile;
    public Tilemap floor;
    private TileBase floortile;

    // Start is called before the first frame update
    public void AddFloor(Vector2Int _coord)
    {
        AddFloor(_coord.x, _coord.y);
    }

    public void AddFloor(int x, int y)
    {
        floor.SetTile(new Vector3Int(x, y, 0), floortile);
    }


    private GameObject Addtile(GameObject tile, Vector2Int _coord)
    {
        GameObject body = Instantiate(tile, floor.GetCellCenterWorld(((Vector3Int)_coord)), Quaternion.identity);
        return body;
    }

    


    void Start()
    {
        floortile = IcwService.GetTileByName("Floor");
        for (int i = 0; i < IcwGame.sizeX ; i++)
        {
            for (int j = 0; j < 2; j++) AddFloor(i, j); 
            for (int j = IcwGame.sizeY - 2; j < IcwGame.sizeY; j++) AddFloor(i, j); 
        }
        
        for (int j = 2; j < IcwGame.sizeY - 2; j++)
        {
            for (int i = 0; i < 2; i++) AddFloor(i, j);
            for (int i = IcwGame.sizeX - 2; i < IcwGame.sizeX ; i++) AddFloor(i, j);
        }

        {
            GameObject enemy = IcwService.GetPrefabByName("Enemy");
            Vector2Int enemypos = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
        }

        {
            GameObject enemy = IcwService.GetPrefabByName("EnemyDestroyer");
            Vector2Int enemypos = new(Random.Range(3, IcwGame.sizeX - 3), Random.Range(3, IcwGame.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
        }

    }



}
