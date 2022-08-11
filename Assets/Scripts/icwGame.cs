using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using Assets.Scripts;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class IcwGame : MonoBehaviour
{
    [System.NonSerialized] public static int sizeX = 40;
    [System.NonSerialized] public static int sizeY = 20;
    [System.NonSerialized] public float targetfilled = 0.80f;
    float CurrentFilledPercent()
    {
        float filled = IcwObjects.FieldTiles.transform.childCount;// / ((sizeX - 4.0f) * (sizeY - 4.0f));
        filled += IcwObjects.BorderTiles.transform.childCount - (sizeX - 4.0f) * 4 - sizeY * 4;  // ((sizeX - 4.0f) * (sizeY - 4.0f));
        filled /= ((sizeX - 4.0f) * (sizeY - 4.0f));
        return filled;
    }
    

    private void Start()
    {
        IcwObjects.Field.transform.position = new Vector3(0, 0, -1);
        IcwObjects.WinCanvas.SetActive(false);
        IcwObjects.GameOverCanvas.SetActive(false);
        Time.timeScale = 1;

        IcwObjects.IcwRewardedAdsclass.LoadAd();
        IcwObjects.IcwInterstitialAdsclass.LoadAd();
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
        IcwObjects.IcwInterstitialAdsclass.TryToShowAd();
        Time.timeScale = 1;
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        SceneManager.LoadScene("GameScene");
    }
    public void StartGame()
    {

        IcwObjects.BeforeLevelCanvas.SetActive(false);
        Time.timeScale = 1;
        IcwObjects.playerlogicclass.starttime = Time.realtimeSinceStartup;
    }

    void Win()
    {
        IcwObjects.WinCanvas.SetActive(true);
        string levelnum = "Level"+(PlayerPrefs.GetInt("CurrentLevel") + 1).ToString();

        int stars = 0;
        float totaltime = Time.realtimeSinceStartup - IcwObjects.playerlogicclass.starttime;
        if (IcwObjects.playerlogicclass.lives >= 3) stars++;
        if (IcwObjects.scoresclass.scores.value >= 5000) stars++;
        if (totaltime <= 180) stars++;
        PlayerPrefs.SetInt(levelnum, stars);

        IcwLevelButton levelbutton = IcwObjects.WinCanvas.transform.Find("LevelButton").GetComponent<IcwLevelButton>();
        levelbutton.FillStars(levelnum);
        levelbutton.gameObject.GetComponent<Button>().interactable = false;
        levelbutton.gameObject.transform.Find("Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = levelnum;
        Time.timeScale = 0;
    }

    void GameOver()
    {
        IcwObjects.GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGameAfterAds()
    {
        IcwObjects.GameOverCanvas.SetActive(false);
        Time.timeScale = 1;
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

