using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Physics.gravity = new Vector3(0, 0, 0);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Physics.gravity = new Vector3(0, -0.1f, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Physics.gravity = new Vector3(0, -25f, 0);
        }


    }
}

