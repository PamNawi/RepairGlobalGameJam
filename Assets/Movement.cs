using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float frontSpeed = 1.0f;
    public float verticalSpeed = 1.0f;
    public float horizontalSpeed = 1.0f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical") * verticalSpeed;
        float h = Input.GetAxis("Horizontal") * horizontalSpeed;

        rb.velocity = transform.forward * frontSpeed + 
            transform.right * h + 
            transform.up * v;

    }
}
