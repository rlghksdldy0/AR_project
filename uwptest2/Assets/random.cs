using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
    public GameObject[] randomob;
    public Transform parent;
    private float spawnRangeX = 200f;
    private float spawnRangeY = 200f;
    private float spawnposZ = 200f;

    public int i = 1;
    public float a = 1;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("object_test", 1, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(i< 1000000)
            {
                i++;
                Vector3 spwanPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), Random.Range(-spawnposZ, spawnposZ));
                int animalIndex = Random.Range(0, randomob.Length);
                GameObject inst = Instantiate(randomob[animalIndex], spwanPos, Quaternion.identity, parent);
                inst.transform.localScale = new Vector3(a, a, a);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (parent.childCount.Equals(0))
            {
                i--;
                i = 0;
                return;
            }
                
            Destroy(parent.GetChild(0).gameObject);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(a < 100)
            a= a+ 0.001f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(a>1)
                a = a - 0.001f;
        }


    }

    public void object_test()
    {
        Vector3 spwanPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), Random.Range(-spawnposZ, spawnposZ));
        int animalIndex = Random.Range(0,randomob.Length);
        Instantiate(randomob[animalIndex], spwanPos, randomob[animalIndex].transform.rotation);
    }
}
