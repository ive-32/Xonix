using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcwPlayer : MonoBehaviour
{
    Rigidbody2D rg2d;
    public Tilemap floor;
    public GameObject grid;

    public float playerspeed = 10;
    Vector2 playervelocity = Vector2.zero;
    Vector2 currentpos = Vector2.zero;

    void Start()
    {
        rg2d = this.GetComponent<Rigidbody2D>();
        transform.position = floor.GetCellCenterWorld(new Vector3Int(IcwGameClass.sizeX / 2, 1, 0));
        currentpos = transform.position;
    }
    private bool CheckCurrentPos(Vector2 currentpos)
    {
        Vector2 checkedpos = FixCurrentPos(currentpos);
        bool res = true;
        res &= checkedpos.x >= 0;
        res &= checkedpos.x < IcwGameClass.sizeX;
        res &= checkedpos.y >= 0;
        res &= checkedpos.y < IcwGameClass.sizeY;

        return res;
    }
    private Vector2 FixCurrentPos(Vector2 currentpos)
    {
        return floor.GetCellCenterWorld(floor.WorldToCell(currentpos));
    }

    private void TryToFill(Vector2 coord)
    {
        TileBase currtb = floor.GetTile(floor.WorldToCell(coord));
        if (currtb != null) return;
        grid.GetComponent<IcwGrid>().AddFloor(Vector2Int.FloorToInt(coord));
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if ((x != 0) || (y != 0))
        {
            Vector2 vel = Vector2.zero;

            if (Mathf.Abs(x) > Mathf.Abs(y)) vel = new Vector2(x, 0).normalized;
            if (Mathf.Abs(y) > Mathf.Abs(x)) vel = new Vector2(0, y).normalized;

            if (Vector2.Angle(vel, playervelocity) == 180) 
                vel = Vector2.zero;

            if (playervelocity!=vel)
            {
                currentpos = FixCurrentPos(currentpos);
                playervelocity = vel;
                Input.ResetInputAxes();
            }
        }

        currentpos += playerspeed * Time.fixedDeltaTime * playervelocity;
        if (!CheckCurrentPos(currentpos))
        {
            currentpos -= playerspeed * Time.fixedDeltaTime * playervelocity;
            currentpos = FixCurrentPos(currentpos);
            playervelocity = Vector2.zero;
            Input.ResetInputAxes();
        }

        TryToFill(currentpos);

        rg2d.MovePosition(currentpos);
        
    }

}
