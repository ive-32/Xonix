using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    class IcwService: MonoBehaviour
    {
        public static T GetOjectByName<T>(string _name, string path) where T : UnityEngine.Object
        {
            string[] _names = AssetDatabase.FindAssets(_name, new[] { path });
            //GameObject res = null;
            T res = default(T);

            if (_names.Length > 0) res = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(_names[0]));//,typeof(T)); //, typeof(GameObject));
            return res;
        }


        public static GameObject GetPrefabByName(string _name)
        {
            GameObject res = GetOjectByName<GameObject> (_name , "Assets/Prefabs");
            return res;
        }

        public static TileBase GetTileByName(string _name)
        {
            TileBase res = GetOjectByName<TileBase> (_name, "Assets/Tiles");
            return res; 
        }
    }

}
