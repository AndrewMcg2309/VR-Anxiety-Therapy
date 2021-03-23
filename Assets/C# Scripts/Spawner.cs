using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Spawner : MonoBehaviour
{
    public float radius = 10;

    public int spawnRate = 5;

    public int max = 10;


    void Spawn()
    {
        // object
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        // color + emmision   
        Color baseColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        cube.GetComponent<Renderer>().material.color = baseColor;

        // position
        Vector3 pos = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        cube.AddComponent<Rigidbody>();
        cube.transform.position = transform.TransformPoint(pos);
        cube.transform.localScale = new Vector3(2,2,2);

        cube.transform.parent = this.transform;
        cube.tag = "Cube";
    }


    void Start()
    {
        //Invoke("Spawn", 5);
    }

    void OnEnable()
    {

        StartCoroutine(SpawnCoroutine());
    }

    int count = 0;

    System.Collections.IEnumerator SpawnCoroutine()
    {
        // wait for certain time
        yield return new WaitForSeconds(5);

        while(true)
        {
            Spawn();
            
            /*
            count ++;
            if (transform.childCount == max)
            {
                break;
            }
            */
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
            if (cubes.Length == max)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f / (float)spawnRate); 
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
