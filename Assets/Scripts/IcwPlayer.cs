using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class IcwPlayer : MonoBehaviour
    {
        Rigidbody2D rg2d;

        [System.NonSerialized] public float playerspeed = IcwObjects.gamespeed * 4.0f;
        private Vector2 playervelocity = Vector2.zero;
        public Vector2 startpositionbeforefloat;
        public Vector2 PlayerVelocity
        {
            get { return playervelocity; }
            set {   currentpos = AttachToTile(currentpos); 
                    playervelocity = value; 
                    inputvalues = Vector2.zero;
                    Input.ResetInputAxes();
            }
        }
        public Vector2 currentpos = Vector2.zero;
        private Vector3Int starttilebeforemoving;
        [System.NonSerialized] public Vector2 inputvalues = Vector2.zero;
        [System.NonSerialized] private int usegestures;
        

        private void Awake()
        {
            rg2d = this.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            usegestures = PlayerPrefs.GetInt("UseGestures");
            starttilebeforemoving = IcwObjects.floor.WorldToCell(currentpos);
        }

        public void SetPlayerPos(int x, int y)
        {
            transform.localPosition = IcwObjects.floor.GetCellCenterLocal(new Vector3Int(x, y, 0));
            currentpos = transform.localPosition;
            startpositionbeforefloat = currentpos;
            starttilebeforemoving = IcwObjects.floor.WorldToCell(currentpos);
        }

        private bool PlayerCanMove(Vector2 _anewpos, Vector2 _aoldpos)
        {
            Vector2 checkednewpos = AttachToTile(_anewpos);

            bool res = true;
            res &= checkednewpos.x >= 0;
            res &= checkednewpos.x < IcwGame.sizeX;
            res &= checkednewpos.y >= 0;
            res &= checkednewpos.y < IcwGame.sizeY;


            return res;
        }
        private Vector2 AttachToTile(Vector2 currentpos)
        {
            return IcwObjects.floor.GetCellCenterWorld(IcwObjects.floor.WorldToCell(currentpos));
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

            if (usegestures != 0 && IcwObjects.gestures.HasGestures())
            {
                IcwGestures.IcwGesture gest = IcwObjects.gestures.getLastAndClear();//.getFirst();
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
            {
                if (IcwObjects.playerlogicclass.playerstate == IcwPlayerLogic.EnumPlayerState.OnEarth) vel = Vector2.zero;
                else vel = PlayerVelocity;
            }
            
            if (PlayerVelocity != vel)
            {   //start moving or change direction
                starttilebeforemoving = IcwObjects.floor.WorldToCell(currentpos);
                PlayerVelocity = vel;
            }
                
            if (PlayerVelocity == Vector2.zero) starttilebeforemoving = IcwObjects.floor.WorldToCell(currentpos);

            if (IcwObjects.playerlogicclass.playerstate == IcwPlayerLogic.EnumPlayerState.OnEarth && PlayerVelocity != Vector2.zero)
            {
                // if continue moving check cross border
                Vector3Int newpos = IcwObjects.floor.WorldToCell(currentpos + PlayerVelocity.normalized);
                Vector3Int oldpos = IcwObjects.floor.WorldToCell(currentpos);
                if (newpos.x > 0 && newpos.x < IcwGame.sizeX && newpos.y > 0 && newpos.y < IcwGame.sizeY)
                {
                    int newfieldvalue = IcwObjects.gridclass.fieldprojection[newpos.x, newpos.y];
                    if (newfieldvalue == 0 && starttilebeforemoving != oldpos) PlayerVelocity = Vector2.zero;
                }
            }
            
            currentpos += playerspeed * Time.fixedDeltaTime * PlayerVelocity;
            if (!PlayerCanMove(currentpos, previouspos))
            {
                currentpos = previouspos;
                PlayerVelocity = Vector2.zero;
            }

            IcwObjects.playerlogicclass.PlayerMovingLogic(previouspos, currentpos);
            rg2d.MovePosition(currentpos);
        }

    }
}