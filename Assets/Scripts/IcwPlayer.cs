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
    private Vector2 defaultvectorfortargetpos = new Vector2(-1, -1);
    private Vector2 targetpos = new Vector2(-1, -1);
    private GameObject game;
    private Vector2 starttouch;
    private Vector2 currenttouch;
    private Vector2 directiongesture;

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
    Vector2 CalcDirectionOfGesture(Vector2 startpos, Vector2 endpos, float sensitivity = 20)
    {
        Vector2 direction = Vector2.zero;
        if (Vector2.Distance(startpos, endpos) < sensitivity) return direction;
        if (Mathf.Abs(endpos.x - startpos.x) > Mathf.Abs(endpos.y - startpos.y))
            direction.x = (endpos.x - startpos.x);
        else
            direction.y = (endpos.y - startpos.y);
        direction.Normalize();
        return direction;
    }

    bool DetectGestures()
    {
        if (Input.touchCount == 0) return false;

        Touch touch = Input.GetTouch(0);
        {

            float dpi = Screen.dpi;
            if (dpi == 0) dpi = Mathf.Min(Screen.width, Screen.height) / 3;
            float mingestvalue = Screen.dpi * 0.05f;
            Vector2 endtouch = touch.position;
            if (touch.phase == TouchPhase.Began)
            {

                directiongesture = Vector2.zero;
                starttouch = endtouch;
                currenttouch = endtouch;
                Debug.LogWarning("Began " + starttouch.ToString());
                return true;
            }
            Vector2 direction = CalcDirectionOfGesture(currenttouch, endtouch, mingestvalue);

            /* if (touch.phase == TouchPhase.Stationary)
            {
                targetpos = AttachToTile(currentpos + direction * Vector2.Distance(currenttouch, endtouch));
                currenttouch = endtouch;
                directiongesture = direction;
                return true;
            }*/

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                Debug.LogWarning("Ended endpoint " + endtouch.ToString());
                Debug.LogWarning("Ended direction " + direction.ToString());
                if (directiongesture != direction && directiongesture != Vector2.zero) starttouch = endtouch;
                if (direction == Vector2.zero) return true;
                targetpos = defaultvectorfortargetpos;
                currenttouch = endtouch;
                directiongesture = direction;
                return true;
            }


        }
        return false;
    }


    private void FixedUpdate()
    {
        Vector2 previouspos = currentpos;
        if (DetectGestures())
        {
            float x = directiongesture.x;
            float y = directiongesture.y;
            if (targetpos != defaultvectorfortargetpos)
            {
                if (AttachToTile(currentpos) == targetpos) { PlayerVelocity = Vector2.zero; targetpos = defaultvectorfortargetpos; } 
                else
                    PlayerVelocity = directiongesture;
            } else
                PlayerVelocity = directiongesture;
            //if (Vector2.Angle(directiongesture, PlayerVelocity) == 180) { directiongesture = Vector2.zero; PlayerVelocity = Vector2.zero; }
        }
        else
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

                if (playervelocity != vel)
                {
                    PlayerVelocity = vel;
                    Input.ResetInputAxes();
                }
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
