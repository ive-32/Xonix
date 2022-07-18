using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwPlayerLogic : MonoBehaviour
{
    static int fillroutine_deeplevel;
    private List<Vector3Int> fillpoints = new List<Vector3Int>();
    private int[,] tmpfieldprojection;
    public enum EnumPlayerState { OnEarth, OnFlow }
    [System.NonSerialized] public EnumPlayerState playerstate = EnumPlayerState.OnEarth;
    public IcwGrid.FieldObjectsTypes currenttracetileobject = IcwGrid.FieldObjectsTypes.Trace;
    [System.NonSerialized] public int lives;


    private void FillFromPoint(Vector3Int startpoint, int value)
    {
        fillroutine_deeplevel++;
        if (startpoint.x >= IcwGame.sizeX || startpoint.x < 0 || startpoint.y >= IcwGame.sizeY || startpoint.y < 0) return;
        if (tmpfieldprojection[startpoint.x, startpoint.y] != 0) return;
        tmpfieldprojection[startpoint.x, startpoint.y] = value;
        Vector3 tmpvector = Vector3.up;
        for (int i = 0; i < 9; i++)
        {
            tmpvector = Quaternion.AngleAxis(45, Vector3.forward) * tmpvector;
            Vector3Int tmpvectorint = Vector3Int.RoundToInt(tmpvector);
            if (tmpfieldprojection[startpoint.x + tmpvectorint.x, startpoint.y + tmpvectorint.y] == 0)
            {
                if (fillroutine_deeplevel < 100) FillFromPoint(tmpvectorint + startpoint, value);
                else fillpoints.Add(tmpvectorint + startpoint);
            }
        }
        fillroutine_deeplevel--;
    }


    public void FillFieldAfterFlow()
    {
        tmpfieldprojection = new int[IcwGame.sizeX, IcwGame.sizeY];
        //fills captured area 
        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeY; j++)
            {   // fill tmparray field values and change tracetiles to floortiles
                if (IcwObjects.gridclass.fieldprojection[i, j] != (int)IcwGrid.FieldObjectsTypes.Empty)
                    tmpfieldprojection[i, j] = 2;
                else tmpfieldprojection[i, j] = 0;
            }
        // get contignous area for each enemy 
        fillpoints.Clear();
        foreach (Transform enemy in IcwObjects.Enemies.transform)
        {
            Vector3Int startpos = IcwObjects.floor.WorldToCell(enemy.position);
            fillpoints.Add(startpos);
        }
        while (fillpoints.Count > 0)
        {
            Vector3Int currfillpoint = fillpoints[^1];
            fillpoints.RemoveAt(fillpoints.Count - 1);
            FillFromPoint(currfillpoint, 1);
        }
        // fills areas where enemy not detected
        for (int i = 0; i < IcwGame.sizeX; i++)
            for (int j = 0; j < IcwGame.sizeY; j++)
                if (tmpfieldprojection[i, j] == 0)
                {
                    IcwObjects.gridclass.AddFieldTile(i, j);
                    IcwObjects.scoresclass.AddScores(10, IcwObjects.floor.GetCellCenterWorld(new Vector3Int(i, j, 0)));
                }
        IcwObjects.gridclass.ChangeTraceObjects(IcwGrid.FieldObjectsTypes.Field);
    }


    public void PlayerOnTile(int x, int y)
    {
        if (IcwObjects.gridclass.fieldprojection[x, y] != 0)
        {
            if (playerstate != EnumPlayerState.OnEarth)
            {   //Finish flow
                //Input.ResetInputAxes();
                IcwObjects.playerclass.PlayerVelocity = Vector2.zero;
                FillFieldAfterFlow();
            }
            playerstate = EnumPlayerState.OnEarth;
            return;
        }
        if (playerstate == EnumPlayerState.OnEarth) playerstate = EnumPlayerState.OnFlow;
        if (playerstate == EnumPlayerState.OnFlow)
        {
            IcwObjects.gridclass.AddTile(x, y, currenttracetileobject);
            IcwObjects.scoresclass.AddScores(1, IcwObjects.floor.GetCellCenterWorld(new Vector3Int(x, y, 0)));
        }
        return;
    }

    public void PlayerMovingLogic(Vector3 startpos, Vector3 endpos)
    {   //check all tiles wich was intersected by player moving vector 
        Vector3Int start2dpos = IcwObjects.floor.WorldToCell(startpos);
        Vector3Int end2dpos = IcwObjects.floor.WorldToCell(endpos);
        if (end2dpos == start2dpos) return;
        if (playerstate == EnumPlayerState.OnEarth)
            IcwObjects.playerclass.startpositionbeforefloat = IcwObjects.floor.GetCellCenterWorld(start2dpos);
        Vector3Int direction = end2dpos - start2dpos;
        for (int i = 1; i <= direction.magnitude; i++)
        {
            start2dpos += direction;
            PlayerOnTile(start2dpos.x, start2dpos.y);
        };
        
    }

    public void PlayerWasHit()
    {
        IcwObjects.playerclass.currentpos = IcwObjects.playerclass.startpositionbeforefloat;
        IcwObjects.playerclass.PlayerVelocity = Vector2.zero;
        playerstate = EnumPlayerState.OnEarth;
        IcwObjects.gridclass.ChangeTraceObjects();
        lives--;
    }


    // Start is called before the first frame update
    void Start()
    {
        tmpfieldprojection = new int[IcwGame.sizeX, IcwGame.sizeY];
        currenttracetileobject = IcwGrid.FieldObjectsTypes.Trace;
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        IcwObjects.scoresclass.lives.value = lives;
    }
}
