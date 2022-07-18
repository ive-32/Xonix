using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using Assets.Scripts;


public class IcwGame : MonoBehaviour
{

    
    public GameObject[] enemypool;
    [System.NonSerialized] public static int sizeX = 40;
    [System.NonSerialized] public static int sizeY = 20;
    [System.NonSerialized] public float targetfilled = 0.80f;
    float CurrentFilledPercent() => IcwObjects.FieldTiles.transform.childCount / ((sizeX - 4.0f) * (sizeY - 4.0f));

    static Dictionary<string, int> enemynames = new Dictionary<string, int>()
        {{"enemy", 0}, {"enemydestroyer", 1}, {"enemysuperdestroyer", 2}};

    public static int EnemyByName(string name)
    {
        int value = -1;
        enemynames.TryGetValue(name.ToLower(), out value);
        return value;
    }
    

    private void Start()
    {
        IcwObjects.Field.transform.position = new Vector3(0, 0, -1);
        IcwObjects.WinCanvas.SetActive(false);
        IcwObjects.GameOverCanvas.SetActive(false);
        Time.timeScale = 1;

        SetUpSize(false);
        IcwLevels lvl = this.GetComponent<IcwLevels>();
        lvl.LoadLevelInfo();
        lvl.StartLevel();
    }

    public void SetUpSize(bool ingame)
    {
        //if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        if (IcwObjects.maincamera.pixelWidth < IcwObjects.maincamera.pixelHeight)
        {
            sizeX = 20; sizeY = 40; 
        } else
        { 
            sizeX = 40; sizeY = 20; 
        }
        
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        IcwLevels.levelnum++; 
        SceneManager.LoadScene("GameScene");
    }

    void Win()
    {
        IcwObjects.WinCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    void GameOver()
    {
        IcwObjects.GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }


    private void Update()
    {
        if (CurrentFilledPercent() >= targetfilled) Win();
        if (IcwObjects.playerlogicclass.lives <= 0) GameOver();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                SceneManager.LoadScene("TitleScreenScene");
                return;
            }
        }
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("TitleScreenScene");
                return;
            }
        }

    }

    private void OnGUI()
    {
        IcwObjects.scoresclass.completed.value = Mathf.FloorToInt(CurrentFilledPercent()*100);
    }


}

