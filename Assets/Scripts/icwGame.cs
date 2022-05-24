using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;


public class IcwGame : MonoBehaviour
{
    static int fillroutine_deeplevel;
    private List<Vector3Int> fillpoints = new List<Vector3Int>();
    public enum EnumGameState { OnEarth, OnFlow }
    public static int sizeX = 30;
    public static int sizeY = 60;
    public static EnumGameState gamestate = EnumGameState.OnEarth;
    public TileBase floortile;
    public TileBase tracetile;
    public Tilemap floor;
    public GameObject grid;

    private GameObject player;
    private int[,] tmpfield = new int[sizeX, sizeY];

    private void Start()
    {
        
        player = GameObject.Find("Player");
        floor = GameObject.Find("FloorTileMap").GetComponent<Tilemap>();
        grid = GameObject.Find("Grid");
        floortile = IcwService.GetTileByName("FloorTile");
        tracetile = IcwService.GetTileByName("TraceTile");
    }

    public static List<GameObject> objects = new();

    public void PlayerWasHit()
    {
        //player.GetComponent<IcwPlayer>().currentpos = new Vector2(sizeX / 2, 1);
        player.GetComponent<IcwPlayer>().currentpos = player.GetComponent<IcwPlayer>().startpositionbeforefloat;
        player.GetComponent<IcwPlayer>().PlayerVelocity = Vector2.zero;
        gamestate = EnumGameState.OnEarth;
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            { 
                Vector3Int tmptilepos = new Vector3Int(i, j, 0);
                if (floor.GetTile(tmptilepos) == tracetile)
                {
                    floor.SetTile(tmptilepos, null);
                }
            }
    }

    private void FillFromPoint(Vector3Int startpoint, int value)
    {
        fillroutine_deeplevel++;
        if (startpoint.x >= sizeX || startpoint.x < 0 || startpoint.y >= sizeY || startpoint.y < 0) return;
        if (tmpfield[startpoint.x, startpoint.y] != 0) return;
        tmpfield[startpoint.x, startpoint.y] = value;
        Vector3 tmpvector = Vector3.up;
        for (int i = 0; i < 9; i++)
        {
            tmpvector = Quaternion.AngleAxis(45, Vector3.forward) * tmpvector;
            Vector3Int tmpvectorint = Vector3Int.RoundToInt(tmpvector);
            if (tmpfield[startpoint.x + tmpvectorint.x, startpoint.y + tmpvectorint.y] == 0)
            {
                if (fillroutine_deeplevel < 100) FillFromPoint(tmpvectorint + startpoint, value);
                else fillpoints.Add(tmpvectorint + startpoint);
            }
        }
        fillroutine_deeplevel--;
    }

    public void FillFieldAfterFlow()
    {   //fills captured area 
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {   // fill tmparray field values and change tracetiles to floortiles
                Vector3Int currtilepos = new Vector3Int(i, j, 0);
                TileBase currtb = floor.GetTile(currtilepos);
                if (currtb != null)
                {
                    tmpfield[i, j] = 2;
                    if (currtb.name == "TraceTile") { floor.SetTile(currtilepos, null); floor.SetTile(currtilepos, floortile); }
                } else tmpfield[i, j] = 0;
            }
        // get contignous area for each enemy 
        GameObject[] enemylist = GameObject.FindGameObjectsWithTag("Enemy");
        fillpoints.Clear();
        foreach(GameObject enemy in enemylist)
        {
            Vector3Int startpos = floor.WorldToCell(enemy.transform.position);
            fillpoints.Add(startpos);
        }
        while (fillpoints.Count > 0) 
        {
            Vector3Int currfillpoint = fillpoints[^1];
            fillpoints.RemoveAt(fillpoints.Count - 1);
            FillFromPoint(currfillpoint, 1); 
        }
        // fills areas where enemy not detected
        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
                if (tmpfield[i, j] == 0) floor.SetTile(new Vector3Int(i, j, 0), floortile);
    }

    public void PlayerOnTile(int x, int y)
    {
        Vector3Int playerpositionvector = new Vector3Int(x, y, 0);
        TileBase currtb = floor.GetTile(playerpositionvector);
        if (currtb != null)
        {
            if (gamestate != EnumGameState.OnEarth)
            {   //Finish flow
                Input.ResetInputAxes();
                player.GetComponent<IcwPlayer>().PlayerVelocity = Vector2.zero;
                FillFieldAfterFlow();
            }
            gamestate = EnumGameState.OnEarth;
            return;
        }
        if (IcwGame.gamestate == IcwGame.EnumGameState.OnEarth)
        {   //Start flow
            IcwGame.gamestate = IcwGame.EnumGameState.OnFlow;
        }
        floor.SetTile(playerpositionvector, tracetile);
        return;
    }
    public void PlayerMovingLogic(Vector3 startpos, Vector3 endpos)
    {   //check all tiles wich was intersected by player moving vector 
        Vector3Int start2dpos = floor.WorldToCell(startpos);
        Vector3Int end2dpos = floor.WorldToCell(endpos);
        if (end2dpos == start2dpos) return;
        if (gamestate == EnumGameState.OnEarth)
            player.GetComponent<IcwPlayer>().startpositionbeforefloat = floor.GetCellCenterWorld(start2dpos);
        Vector3Int direction = end2dpos - start2dpos;
        for (int i = 1; i <= direction.magnitude; i++) 
        {
            start2dpos += direction;
            PlayerOnTile(start2dpos.x, start2dpos.y); 
        };
    }




}

