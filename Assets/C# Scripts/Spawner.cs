using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Spawner : MonoBehaviour
{
    public float radius = 10;

    public int spawnRate = 1;

    public int max = 25;


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

        // position
        cube.transform.position = transform.TransformPoint(pos);
        cube.transform.localScale = new Vector3(2,2,2);

        cube.transform.parent = this.transform;
        cube.tag = "Cube";

        Debug.Log("spawn Rate: " + spawnRate);
        
    }
  



    void OnEnable()
    {
        // coroutines for difficulty 

        StartCoroutine(Start());
        
        StartCoroutine(GameStages());

    }

    System.Collections.IEnumerator Start()
    {
        // wait for certain time
        yield return new WaitForSeconds(1);

        while(true)
        {
            Spawn();
            
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
            if (cubes.Length == max)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f / (float)spawnRate); 

        }
    }

    // medium difficulty 
    System.Collections.IEnumerator GameStages()
    {
        // medium
        yield return new WaitForSeconds(5);
        spawnRate = 2;

        // hard
        yield return new WaitForSeconds(5);
        spawnRate = 3;

        // finish
        yield return new WaitForSeconds(5);
        spawnRate = 0;
    }
}
