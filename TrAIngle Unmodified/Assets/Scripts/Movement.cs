using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float thrust;
    public float torque;
    private float multiplier = 1000;
    public bool debug = false;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        torque *= multiplier * 10;
        thrust *= multiplier;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * thrust);
        if (debug)
        {
            if (Input.GetKey(KeyCode.A) == true) ApplyTorque(torque);
            if (Input.GetKey(KeyCode.D) == true) ApplyTorque(-torque);
        }
    }

    public void ApplyTorque(float force)
    {
        rb.AddTorque(torque * force);
    }
}