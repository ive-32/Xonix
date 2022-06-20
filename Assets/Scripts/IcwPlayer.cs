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
    [System.NonSerialized] public Vector2 inputvalues = Vector2.zero;
    [System.NonSerialized] private int usegestures;
    private Vector2 defaultvectorfortargetpos = new Vector2(-1, -1);
    private Vector2 targetpos = new Vector2(-1, -1);
    private IcwGame game;
    private IcwGestures gestures;

    private void Awake()
    {
        game = GameObject.Find("mainGame").GetComponent<IcwGame>();
        GameObject gestobject = GameObject.Find("Gestures");
        if (gestobject != null) gestures = gestobject.GetComponent<IcwGestures>();
        rg2d = this.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.position = floor.GetCellCenterWorld(new Vector3Int(IcwGame.sizeX / 2, 1, 0));
        currentpos = transform.position;
        startpositionbeforefloat = currentpos;
        usegestures = PlayerPrefs.GetInt("UseGestures");
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
    Vector2 CalcDirectionOfGesture(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction.y = 0;
        else
            direction.x = 0;
        direction.Normalize();
        return direction;
    }

    private void FixedUpdate()
    {
        Vector2 previouspos = currentpos;
        Vector2 vel = PlayerVelocity;
        
        if (usegestures!=0 && gestures.HasGestures())
        {
            IcwGestures.IcwGesture gest = gestures.getLastAndClear();//.getFirst();
            if (gest.name == IcwGestures.GestureNames.Slide || gest.name == IcwGestures.GestureNames.Move)
            {
                vel = CalcDirectionOfGesture(gest.direction);
            }
            if (gest.name == IcwGestures.GestureNames.Tap) vel = Vector2.zero;
        }
        else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (inputvalues.x != 0 || inputvalues.y != 0)
            {
                x = inputvalues.x;
                y = inputvalues.y;
            }
            if (x != 0 || y != 0)
                vel = CalcDirectionOfGesture(new Vector2(x, y));
        }

        if (Vector2.Angle(vel, PlayerVelocity) == 180)
            vel = Vector2.zero;

        if (PlayerVelocity != vel)
        {
            PlayerVelocity = vel;
            inputvalues = Vector2.zero;
            Input.ResetInputAxes();
        }

        currentpos += playerspeed * Time.fixedDeltaTime * PlayerVelocity;
        if (!PlayerCanMove(currentpos))
        {
            currentpos = previouspos;
            PlayerVelocity = Vector2.zero;
            inputvalues = Vector2.zero;
            Input.ResetInputAxes();
        }
        
        game.PlayerMovingLogic(previouspos,currentpos);
        rg2d.MovePosition(currentpos);
    }
}
