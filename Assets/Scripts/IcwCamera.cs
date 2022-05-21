using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(IcwGameClass.sizeX / 2, IcwGameClass.sizeY / 2, -IcwGameClass.sizeX);
        Camera.main.orthographicSize = IcwGameClass.sizeX / 2 + 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
