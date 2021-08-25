using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAT2 : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = GameObject.Find("Camera").transform;
    }

    void Update()
    {
        transform.LookAt(player);
    }
}
