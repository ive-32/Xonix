using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


public class IcwGrid : MonoBehaviour
{
    public GameObject basetile;
    public Tilemap floor;
    // Start is called before the first frame update
    public void AddFloor(Vector2Int _coord)
    {
        Addtile(basetile, _coord);
    }
    private GameObject Addtile(GameObject tile, Vector2Int _coord)
    {
        IcwGameClass.GameTile gametile;
        gametile = new IcwGameClass.GameTile
        {
            coords = _coord,
            body = Instantiate(tile, floor.GetCellCenterWorld(((Vector3Int)_coord)), Quaternion.identity)
        };
        IcwGameClass.objects.Add(gametile);
        return gametile.body;
    }

    private GameObject GetPrefabByName(string tilename)
    {
        string[] tilenames = AssetDatabase.FindAssets(tilename, new[] { "Assets/Prefabs" });
        GameObject res = null;

        if (tilenames.Length > 0) res = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(tilenames[0]), typeof(GameObject));
        return res;
    }


    void Start()
    {
        for (int i = 0; i < IcwGameClass.sizeX ; i++)
        {
            for (int j = 0; j < 2; j++) Addtile(basetile, new Vector2Int(i, j));
            for (int j = IcwGameClass.sizeY - 2; j < IcwGameClass.sizeY; j++) Addtile(basetile, new Vector2Int(i, j));
        }
        
        for (int j = 2; j < IcwGameClass.sizeY - 2; j++)
        {
            for (int i = 0; i < 2; i++) Addtile(basetile, new Vector2Int(i, j));
            for (int i = IcwGameClass.sizeX - 2; i < IcwGameClass.sizeX ; i++) Addtile(basetile, new Vector2Int(i, j));
        }

        {
            GameObject enemy = GetPrefabByName("Enemy");
            Vector2Int enemypos = new(Random.Range(3, IcwGameClass.sizeX - 3), Random.Range(3, IcwGameClass.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
            Rigidbody2D rg2d = bd.GetComponent<Rigidbody2D>();
            Vector3 vel = Random.insideUnitCircle.normalized * 5.0f;
            rg2d.velocity = vel;
        }
        {
            GameObject enemy = GetPrefabByName("EnemyDestroyer");
            Vector2Int enemypos = new(Random.Range(3, IcwGameClass.sizeX - 3), Random.Range(3, IcwGameClass.sizeY - 3));
            GameObject bd = Addtile(enemy, enemypos);
            Rigidbody2D rg2d = bd.GetComponent<Rigidbody2D>();
            Vector3 vel = Random.insideUnitCircle.normalized * 5.0f;
            rg2d.velocity = vel;
        }

    }



}
