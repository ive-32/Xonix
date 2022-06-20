using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwCamera : MonoBehaviour
{
    void Start()
    {
        this.transform.position = new Vector3(IcwGame.sizeX / 2.0f, IcwGame.sizeY / 2.0f, -IcwGame.sizeX);
    }


}
