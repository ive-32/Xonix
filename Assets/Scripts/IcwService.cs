using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace Assets.Scripts
{
    class IcwService: MonoBehaviour
    {


        

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * (point - pivot) + pivot;
        }
        public static void RotateChildsAroundPoint(GameObject gm, Vector3 pivot, float angle)
        {
            
            gm.transform.RotateAround(pivot, new Vector3(0, 0, 1), angle);
        }


        public static Vector2 orientationtovector(ScreenOrientation orientation)
        {
            switch (orientation) 
            {
                case (ScreenOrientation.LandscapeLeft): return Vector2.left;
                case (ScreenOrientation.LandscapeRight): return Vector2.right;
                case (ScreenOrientation.Portrait): return Vector2.up;
                case (ScreenOrientation.PortraitUpsideDown): return Vector2.down;
            }
            return Vector2Int.zero;
        }
        public static float CalcAngleOfOrientationChange(ScreenOrientation oldorientation, ScreenOrientation newtorientation)
        {
            Vector2 oldvector = orientationtovector(oldorientation);
            Vector2 newvector = orientationtovector(newtorientation);

            return Vector2.SignedAngle(oldvector, newvector);
        }

        public static Vector2 AttachToTile(Vector2 currentpos)
        {
            return IcwObjects.floor.GetCellCenterWorld(IcwObjects.floor.WorldToCell(currentpos));
        }
        public static Vector3 AttachToTile(Vector3 currentpos)
        {
            return IcwObjects.floor.GetCellCenterWorld(IcwObjects.floor.WorldToCell(currentpos));
        }


    }




}
