using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwGestures : MonoBehaviour
{
    public enum GestureNames { Tap, Slide, Move };
    public class IcwGesture
    {
        private IcwGestures parent;
        public GestureNames name;
        public Vector2 position = Vector2.zero;
        public Vector2 direction = Vector2.zero;
        public float velocity = 0;
        public IcwGesture() { }
        public IcwGesture(GestureNames _name, IcwGestures _parent, Vector2 _pos = default, Vector2 _dir = default, float _vel = 0)
        {
            parent = _parent; 
            name = _name; 
            position = _pos; 
            direction = _dir; 
            velocity = _vel;
            /*GameObject gm = new GameObject(name.ToString() + " | " + direction.ToString() + " | " + velocity.ToString());
            gm.transform.parent = parent.transform;
            Object.Destroy(gm, 1);*/
        }
    }

    private Vector2 starttouch, previoustouch, currtouch;
    private float starttime, currenttime, endtime;
    private bool complexgesture;
    private bool OnGesture = false;
    public Vector2 direction;
    public List<IcwGesture> gesturelist = new List<IcwGesture>();
    [System.NonSerialized] public float positionsensitivity = 3.0f;
    [System.NonSerialized] public float timesensitivity = 0.3f;
    [System.NonSerialized] public float slideedge = 600;

    public bool HasGestures() { return gesturelist.Count > 0; }
    public IcwGesture getLastAndClear() 
    {
        if (gesturelist.Count == 0) return null;
        IcwGesture gest = gesturelist[gesturelist.Count - 1];
        gesturelist.Clear(); 
        return gest; 
    }

    public IcwGesture getFirst()
    {
        if (gesturelist.Count == 0) return null;
        IcwGesture gest = gesturelist[0];
        gesturelist.RemoveAt(0);
        return gest;
    }

   
    public Vector2 CalcDirectionOfGesture(Vector2 startpos, Vector2 endpos)
    {
        Vector2 direction = Vector2.zero;
        if (Vector2.Distance(startpos, endpos) < positionsensitivity) return direction;
        if (Mathf.Abs(endpos.x - startpos.x) > Mathf.Abs(endpos.y - startpos.y))
            direction.x = (endpos.x - startpos.x);
        else
            direction.y = (endpos.y - startpos.y);
        direction.Normalize();
        return direction;
    }

    private void Start()
    {
        float dpi = Screen.dpi;
        if (dpi == 0) dpi = Mathf.Min(Screen.currentResolution.height, Screen.currentResolution.width) / 3;
        positionsensitivity = dpi / 100;
    }
    bool DetectGestures()
    {

        if (Input.touchCount == 0)
        {
            if (OnGesture) gesturelist.Add(new IcwGesture(GestureNames.Tap, this, starttouch));
            OnGesture = false;
            return false;
        }

        Touch touch = Input.GetTouch(0);
        endtime = Time.realtimeSinceStartup;
        {

            currtouch = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                OnGesture = true;
                starttouch = currtouch;
                previoustouch = currtouch;
                starttime = endtime;
                currenttime = endtime;
                complexgesture = false;
                return true;
            }

            float distance = Vector2.Distance(starttouch, currtouch);
            float _vel = distance / (endtime - currenttime);
            float complexgesturecoeff = 3.0f;

            if (touch.phase == TouchPhase.Ended)
            {
                OnGesture = false;

                if (!complexgesture && distance < positionsensitivity)
                {
                    gesturelist.Add(new IcwGesture(GestureNames.Tap, this, currtouch));
                    return true; 
                }
                if (distance > positionsensitivity * (complexgesture ? complexgesturecoeff : 1) )
                {
                    if (_vel > slideedge)
                        gesturelist.Add(new IcwGesture(GestureNames.Slide, this, currtouch, currtouch - starttouch, _vel));
                    else
                        gesturelist.Add(new IcwGesture(GestureNames.Move, this, currtouch, currtouch - starttouch, _vel));
                }
                return true;
            }

            if (touch.phase == TouchPhase.Stationary)
            {
                if (endtime - currenttime > timesensitivity && distance > positionsensitivity * complexgesturecoeff)
                {
                    gesturelist.Add(new IcwGesture(GestureNames.Move, this, currtouch, currtouch - starttouch, _vel));
                    currenttime = endtime;
                    starttouch = currtouch;
                    complexgesture = true;
                }
                return true;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (distance > positionsensitivity * complexgesturecoeff)
                {
                    if (Vector2.Angle(previoustouch - starttouch,currtouch - previoustouch) > 45)
                    {
                        gesturelist.Add(new IcwGesture(GestureNames.Move, this, currtouch, currtouch - starttouch, _vel));
                        currenttime = endtime;
                        starttouch = currtouch;
                        previoustouch = starttouch;
                        complexgesture = true;
                    }
                    previoustouch = currtouch;
                }
            }
        }
        return false;
    }

    
    // Update is called once per frame
    void Update()
    {
        DetectGestures();    
    }
}
