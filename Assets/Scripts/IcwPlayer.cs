using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class IcwPlayer : MonoBehaviour
    {
        Rigidbody2D rg2d;

        [System.NonSerialized] public float playerspeed = IcwObjects.gamespeed * 4.0f;
        [System.NonSerialized] public Vector2 startpositionbeforefloat;
        [System.NonSerialized] public Vector2 currentpos = Vector2.zero;
        [System.NonSerialized] public Vector2 previoustile = Vector2.zero;
        [System.NonSerialized] public Vector2 targettile = Vector2.zero;
        [System.NonSerialized] public Vector2 inputvalues = Vector2.zero;
        [System.NonSerialized] private int usegestures;

        private Vector2 playerdirection = Vector2.zero;
        private Vector2 playervelocity = Vector2.zero;
        private Vector3Int starttilebeforemoving;

        public Vector2 currenttile
        {
            get { return AttachToTile(currentpos); }
            set { currentpos = AttachToTile(value); }
        }
        public Vector2 PlayerVelocity
        {
            get { return playervelocity; }
            set { currentpos = AttachToTile(currentpos);
                playervelocity = value;
                playerdirection = value;
                inputvalues = Vector2.zero;
                Input.ResetInputAxes();
            }
        }
        private Vector2 AttachToTile(Vector2 _apos) => IcwObjects.floor.GetCellCenterWorld(IcwObjects.floor.WorldToCell(_apos));

        private int GetFieldInfo(Vector2 _apos)
        {
            if (!PositionIsValid(_apos)) return -1;
            Vector3Int _atile = IcwObjects.floor.WorldToCell(_apos);
            return IcwObjects.gridclass.fieldprojection[_atile.x, _atile.y];
        }

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
            currenttile = new Vector2(x, y);
            transform.localPosition = currenttile;
            currentpos = currenttile;
            previoustile = currenttile;
            startpositionbeforefloat = currentpos;
            starttilebeforemoving = IcwObjects.floor.WorldToCell(currentpos);
            playerdirection = Vector2.zero;
            playervelocity = Vector2.zero;
        }

        private bool PositionIsValid(Vector2 apos)
        {
            Vector2 checkednewpos = AttachToTile(apos);

            bool res = true;
            res &= checkednewpos.x >= 0;
            res &= checkednewpos.x < IcwGame.sizeX;
            res &= checkednewpos.y >= 0;
            res &= checkednewpos.y < IcwGame.sizeY;

            return res;
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
        
        Vector2 CalcDirectionOfGesture(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                direction.y = 0;
            else
                direction.x = 0;
            direction.Normalize();
            return direction;
        }

        private Vector2 GetInputDirection()
        {
            Vector2 direction = playerdirection;
            if (usegestures != 0 && IcwObjects.gestures.HasGestures())
            {
                IcwGestures.IcwGesture gest = IcwObjects.gestures.getLastAndClear(); //.getFirst(); //
                if (gest.name == IcwGestures.GestureNames.Slide || gest.name == IcwGestures.GestureNames.Move)
                {
                    direction = CalcDirectionOfGesture(gest.direction);
                }
                if (gest.name == IcwGestures.GestureNames.Tap) direction = Vector2.zero;
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
                    direction = CalcDirectionOfGesture(new Vector2(x, y));
            }
            targettile = currenttile + playerdirection.normalized;
            if (!PositionIsValid(targettile))
            {   // player can't move to target tile  
                direction = Vector2.zero; 
                targettile = currenttile; 
            } 
            return direction;
        }
        private bool PlayerReachTargetTile()
        {
            Vector2 previouspos = currentpos;
            Vector2 newpos = currentpos + playerspeed * Time.fixedDeltaTime * PlayerVelocity;
            return Vector2.Angle(previouspos - currenttile, newpos - currenttile) > 90;

        }

        private void FixedUpdate()
        {
            playerdirection = GetInputDirection();
            if (playervelocity == Vector2.zero)
            {
                playervelocity = playerdirection; // start moving
                previoustile = currenttile;
            }

            if (PlayerReachTargetTile()) 
            {
                if (playerdirection == playervelocity)
                {   // continue moving 
                    if (playervelocity != Vector2.zero)
                    {   // check cross borders here
                        if (GetFieldInfo(currenttile) != 0 && GetFieldInfo(targettile) == 0) playerdirection = Vector2.zero;
                        if (GetFieldInfo(previoustile) == (int)IcwGrid.FieldObjectsTypes.Trace 
                            && GetFieldInfo(currenttile) != 0) playerdirection = Vector2.zero;
                    }
                }

                if (playerdirection != playervelocity)
                {   // change direction
                    currentpos = currenttile;
                    playervelocity = playerdirection;
                }
                IcwObjects.playerlogicclass.PlayerMovingLogic(previoustile, currenttile);
                previoustile = currenttile;
            };

            currentpos += playerspeed * Time.fixedDeltaTime * PlayerVelocity;
            rg2d.MovePosition(currentpos);

            return;

            /*if (Vector2.Angle(vel, PlayerVelocity) == 180)
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
            rg2d.MovePosition(currentpos);*/
        }

    }
}