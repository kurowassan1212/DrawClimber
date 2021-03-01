using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigger : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 1.1)
        {
            transform.localScale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, Time.deltaTime * speed);
        }
        else
        {
            this.enabled = false;
        }

    }
}
