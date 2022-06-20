using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IcwUIStartMenu : MonoBehaviour
{
    public Toggle toggle;
    private void Awake()
    {
        toggle = GameObject.Find("Toggle").GetComponent<Toggle>();
    }

    private void Start()
    {
        int value = PlayerPrefs.GetInt("UseGestures");
        toggle.isOn = (value != 0);
    }

    public void OnUseGestureClick()
    {
        if (toggle.isOn)
            PlayerPrefs.SetInt("UseGestures", 1);
        else
            PlayerPrefs.SetInt("UseGestures", 0);
    }

    public void OnNewGameClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

}
