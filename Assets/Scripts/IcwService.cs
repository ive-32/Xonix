using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

using System.Linq;

namespace Assets.Scripts
{
    class IcwService: MonoBehaviour
    {
        public static T GetObjectByName<T>(string _name, string path) where T : UnityEngine.Object
        {
            /*string[] _names = Resources.Load(_name, new[] { path });
            T res = default;

            if (_names.Length > 0) res = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(_names[0]));//,typeof(T)); //, typeof(GameObject));*/
            T res = Resources.Load<T>(_name);
            return res;
        }


        public static GameObject GetPrefabByName(string _name)
        {
            GameObject res = GetObjectByName<GameObject> (_name , "Assets/Prefabs");
            return res;
        }

        public static TileBase GetTileByName(string _name)
        {
            TileBase res = GetObjectByName<TileBase> (_name, "Assets/Tiles");
            return res; 
        }
        public static bool IsInField(Vector3Int coords)
        {
            bool res = (coords.x > 1 && coords.x < IcwGame.sizeX - 2 && coords.y > 1 && coords.y < IcwGame.sizeY - 2);
            return res;
        }

        public static GameObject[] GetAllObjectsByTag(Vector3 position, string _atag)
        {
            return Physics.OverlapSphere(position, 2)
                        .Select(c => c.gameObject )
                        //.Where(c => c.gameObject.tag == _atag)
                        .ToArray();
        }
    }


}
