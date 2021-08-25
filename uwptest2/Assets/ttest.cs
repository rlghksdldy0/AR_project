using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttest : MonoBehaviour
{
    public float speed = 500f;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0, +speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
    }
}