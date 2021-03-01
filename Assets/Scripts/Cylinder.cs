using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    public float rotateSpeed;

    public Vector3 rotation;
    public bool gravity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -90, -90);
        rb.useGravity = false;
        gravity = false;
        /*     Cursor.visible = false; */
    }

    // Update is called once per frame
    void Update()
    {

        /* transform.position = player.transform.position; */
        /*   transform.Rotate(new Vector3(0, 0, -130) * Time.deltaTime); */

        rotation = gameObject.transform.localEulerAngles;
        /* if (rotation.y < -80 && rotation.y > -100)
        {
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -90, -90);
        } */

        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            gravity = true;
            Time.timeScale = 1f;
        }

    }
    void FixedUpdate()
    {

        rb.angularVelocity = transform.up * -rotateSpeed;
        /* rb.AddTorque(0, 0, -0.1f, ForceMode.VelocityChange); */

        if (gravity == true)
        {
            rb.AddForce(new Vector3(0, -40, 0), ForceMode.Acceleration);
        }

    }


}
