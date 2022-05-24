using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(IcwGame.sizeX / 2, IcwGame.sizeY / 2, -IcwGame.sizeX);
        //Camera.main.orthographicSize = IcwGame.sizeX / 2 + 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
