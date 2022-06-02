using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcwCamera : MonoBehaviour
{
    void Start()
    {
        this.transform.position = new Vector3(IcwGame.sizeX / 2, IcwGame.sizeY / 2, -IcwGame.sizeX);
    }


}
