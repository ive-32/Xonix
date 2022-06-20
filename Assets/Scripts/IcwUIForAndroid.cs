using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IcwUIForAndroid : MonoBehaviour, IPointerDownHandler
{
    public static int GetUIDirectionFromName(string name)
    {
        int dir = -1;
        if (name.ToLower() == "left") dir = 0;
        if (name.ToLower() == "down") dir = 1;
        if (name.ToLower() == "right") dir = 2;
        if (name.ToLower() == "up") dir = 3;
        return dir;
    }
    //0 right 1 down 2 left 3 up
    [System.NonSerialized] public int UIArrowType = -1;
    IcwPlayer player;

    public void Start()
    {
        int usegestures = PlayerPrefs.GetInt("UseGestures");
        if (usegestures != 0) { this.gameObject.SetActive(false); return; }
        player = GameObject.Find("Player").GetComponent<IcwPlayer>();
        string directionname = this.name.ToLower().Replace("arrow","");
        UIArrowType = GetUIDirectionFromName(directionname);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIArrowType == -1) return;
        float[,] axes = {{ -1.0f, 0.0f }, { 0.0f, -1.0f }, { 1.0f, 0.0f }, { 0.0f, 1.0f }};
        player.inputvalues.x = axes[UIArrowType, 0];
        player.inputvalues.y = axes[UIArrowType, 1];
    }
}
