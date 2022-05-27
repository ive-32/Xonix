using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts;


public class IcwPlayer : MonoBehaviour
{
    Rigidbody2D rg2d;
    public Tilemap floor;
    public GameObject grid;

    public float playerspeed = 10;
    private Vector2 playervelocity = Vector2.zero;
    public Vector2 startpositionbeforefloat;
    public Vector2 PlayerVelocity 
    { 
        get { return playervelocity; } 
        set { currentpos = AttachToTile(currentpos); playervelocity = value; } 
    }
    public Vector2 currentpos = Vector2.zero;
    private GameObject game;

    void Start()
    {
        game = GameObject.Find("mainGame");
        rg2d = this.GetComponent<Rigidbody2D>();
        transform.position = floor.GetCellCenterWorld(new Vector3Int(IcwGame.sizeX / 2, 1, 0));
        currentpos = transform.position;
        startpositionbeforefloat = currentpos;
    }
    private bool PlayerCanMove(Vector2 currentpos)
    {
        Vector2 checkedpos = AttachToTile(currentpos);
        bool res = true;
        res &= checkedpos.x >= 0;
        res &= checkedpos.x < IcwGame.sizeX;
        res &= checkedpos.y >= 0;
        res &= checkedpos.y < IcwGame.sizeY;

        return res;
    }
    private Vector2 AttachToTile(Vector2 currentpos)
    {
        return floor.GetCellCenterWorld(floor.WorldToCell(currentpos));
    }



    private void FixedUpdate()
    {
        Vector2 previouspos = currentpos;
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
                PlayerVelocity = vel;
                Input.ResetInputAxes();
            }
        }

        currentpos += playerspeed * Time.fixedDeltaTime * playervelocity;
        if (!PlayerCanMove(currentpos))
        {
            currentpos = previouspos;
            PlayerVelocity = Vector2.zero;
            Input.ResetInputAxes();
        }
        
        
        game.GetComponent<IcwGame>().PlayerMovingLogic(previouspos,currentpos);

        rg2d.MovePosition(currentpos);
        
    }

}
