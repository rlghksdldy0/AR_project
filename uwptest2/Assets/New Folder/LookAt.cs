using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Start()
    {
        player = GameObject.Find("Camera").transform;
    }

    void Update()
    {
        transform.LookAt(2 * transform.position - player.position);
    }
}
