using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float speedUp = 5f;

    public void Accelerate ()
    {
        if (Time.timeScale == 1F)
            Time.timeScale = speedUp * Time.timeScale;
        else
            Time.timeScale = 1F;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
