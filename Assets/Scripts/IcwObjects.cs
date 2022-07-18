using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    class IcwObjects : MonoBehaviour
    {
        static public float gamespeed = 1.0f;
        static public IcwGame gameclass;
        static public IcwPlayer playerclass;
        static public IcwPlayerLogic playerlogicclass;
        static public Tilemap floor;
        static public IcwGrid gridclass;
        static public IcwScores scoresclass;
        //static public IcwActiveBoosters activeboosters;
        static public IcwBooster boosterclass ;
        static public Camera maincamera;
        static public IcwGestures gestures;
        static public GameObject FieldTiles;
        static public GameObject TraceTiles;
        static public GameObject BorderTiles;
        static public GameObject Enemies;
        static public GameObject BoosterTiles;
        static public GameObject Field;
        static public GameObject WinCanvas;
        static public GameObject GameOverCanvas;


        public void InitObjects()
        {
            
            gameclass = GameObject.Find("mainGame").GetComponent<IcwGame>();
            playerclass = GameObject.Find("Player").GetComponent<IcwPlayer>();
            playerlogicclass = GameObject.Find("Player").GetComponent<IcwPlayerLogic>();
            floor = GameObject.Find("FloorTileMap").GetComponent<Tilemap>();
            gridclass = GameObject.Find("Grid").GetComponent<IcwGrid>();
            scoresclass = GameObject.Find("Scores").GetComponent<IcwScores>();
            maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            //activeboosters = GameObject.Find("BonusTiles").GetComponent<IcwActiveBoosters>();
            boosterclass = GameObject.Find("BonusTiles").GetComponent<IcwBooster>();
            FieldTiles = GameObject.Find("FieldTiles");
            TraceTiles = GameObject.Find("TraceTiles");
            BorderTiles = GameObject.Find("BorderTiles");
            BoosterTiles = GameObject.Find("BonusTiles");
            Enemies = GameObject.Find("Enemies");
            Field = GameObject.Find("Field");
            gestures = GameObject.Find("Gestures").GetComponent<IcwGestures>();

            WinCanvas = GameObject.Find("UICanvas").transform.Find("WinLevel").gameObject;
            GameOverCanvas = GameObject.Find("UICanvas").transform.Find("GameOver").gameObject;

        }

        private void Awake()
        {
            InitObjects();            

        }

        private void Start()
        {

            InitObjects();

        }


    }
}
