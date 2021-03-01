using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlayer : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject cylinder;
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        /* rb = GetComponent<Rigidbody>(); */

        /*   Physics.gravity = new Vector3(0, 0f, 0); */
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = cylinder.transform.position;
        /* if (Input.GetMouseButtonDown(0))
        {
            Physics.gravity = new Vector3(0, -0.1f, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
 */

    }

}
