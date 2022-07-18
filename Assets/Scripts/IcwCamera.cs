using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class IcwCamera : MonoBehaviour
{
    [System.NonSerialized] public ScreenOrientation currentorientation;
    

    void Start()
    {
        currentorientation = Screen.orientation;
        SetUpCameraAngle();
        SetUpCameraPosition();
    }

    void SetUpCameraAngle()
    {
        if (IcwObjects.maincamera == null) return;
        float fCameraAspect = 1.0f;
        float fCameraSize = 20;
        float minscreeensize = Mathf.Min(IcwObjects.maincamera.pixelWidth, IcwObjects.maincamera.pixelHeight);
        float maxscreeensize = Mathf.Max(IcwObjects.maincamera.pixelWidth, IcwObjects.maincamera.pixelHeight);
        fCameraAspect = minscreeensize / maxscreeensize;
        int minsize = Mathf.Min(IcwGame.sizeX, IcwGame.sizeY);
        int maxsize = Mathf.Max(IcwGame.sizeX, IcwGame.sizeY);
        
        if (IcwObjects.maincamera.pixelWidth > IcwObjects.maincamera.pixelHeight)
        {
            if (fCameraAspect < 0.5) { fCameraSize = (minsize + 3) ; }
            else { fCameraSize = (maxsize + 5) * fCameraAspect; }
        }
        else
        {
            if (fCameraAspect < 0.5) { fCameraSize = (minsize + 5) / fCameraAspect; }
            else { fCameraSize = (maxsize + 3); }

        }


        IcwObjects.maincamera.orthographicSize = fCameraSize / 2;
        
    }
    void SetUpCameraPosition()
    {
        //this.transform.localPosition = new Vector3(IcwGame.sizeX / 2.0f, IcwGame.sizeY / 2.0f, -50);
        int maxsize = Mathf.Max(IcwGame.sizeX, IcwGame.sizeY);
        int minsize = Mathf.Min(IcwGame.sizeX, IcwGame.sizeY);
        if (IcwObjects.maincamera.pixelWidth > IcwObjects.maincamera.pixelHeight)
            this.transform.localPosition = new Vector3(maxsize / 2.0f, minsize / 2.0f, -50);
        else 
            this.transform.localPosition = new Vector3(minsize / 2.0f, maxsize / 2.0f, -50);
        //*/
    }


    private void Update()
    {

        
        if (Screen.orientation != currentorientation)
        {
            /*float angle = IcwService.CalcAngleOfOrientationChange(currentorientation, Screen.orientation);
            Vector3 pivot;
            int point1 = 0;
            int point2 = 0;
            if (IcwObjects.Field.transform.rotation.eulerAngles.z == 0 || IcwObjects.Field.transform.rotation.eulerAngles.z == 180)
            {
                point1 = IcwGame.sizeX;
                point2 = IcwGame.sizeY;
            }
            if (IcwObjects.Field.transform.rotation.eulerAngles.z == 90 || IcwObjects.Field.transform.rotation.eulerAngles.z == 270)
            {
                point1 = IcwGame.sizeY;
                point2 = IcwGame.sizeX;
            }



            if (angle<0) pivot = new Vector3(point1 / 2.0f, point1 / 2.0f, 0);
            else pivot = new Vector3(point2 / 2.0f, point2 / 2.0f, 0);

            SetUpCameraAngle();
            SetUpCameraPosition();

            IcwObjects.Field.transform.RotateAround(pivot, new Vector3(0, 0, 1), angle);
          
            Vector3 pos = new Vector3(IcwObjects.playerclass.currentpos.x, IcwObjects.playerclass.currentpos.y, 0);
            pos = IcwService.RotatePointAroundPivot(pos, pivot, angle);
            IcwObjects.playerclass.currentpos.x = pos.x;
            IcwObjects.playerclass.currentpos.y = pos.y;//*/
            
            SetUpCameraAngle();
            SetUpCameraPosition();

            currentorientation = Screen.orientation;
        }//*/
    }
}

