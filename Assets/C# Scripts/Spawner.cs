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
        cube.AddComponent<Collider>();

        // position
        cube.transform.position = transform.TransformPoint(pos);
        cube.transform.localScale = new Vector3(2,2,2);

        cube.transform.parent = this.transform;
        cube.tag = "Cube";
    }


    void OnEnable()
    {
        StartCoroutine(SpawnCoroutine());
    }

    System.Collections.IEnumerator SpawnCoroutine()
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
}
