using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatOnStop : MonoBehaviour {

    private float timeStopped;
    private bool hasPreviouslyStopped = false;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start ()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (HasStopped())
        {
            if (hasPreviouslyStopped)
            {
                if (Time.time - timeStopped > 3)
                {
                    Debug.Log("Defeat");
                }
            }
            else
            {
                hasPreviouslyStopped = true;
                timeStopped = Time.time;
            }
        }
        else
        {
            hasPreviouslyStopped = false;
        }

    }

    private bool HasStopped()
    {
        if (rb.velocity.magnitude > 0.01) return false;
        else return true;
    }
}
