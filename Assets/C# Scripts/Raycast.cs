using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Raycast : MonoBehaviour
{
    // eplosion
    public float cubeSize = 0.2f;
    public int cubesInRow = 5;
    float cubesPivotDistance;
    Vector3 cubesPivot;
    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;
    // audio
    AudioSource audioSource;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast (ray, out hitInfo, 100))
        {
            Debug.Log("1");
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            if(hitInfo.collider.tag == "Cube")
            {
                //make object disappear
                Destroy(hitInfo.transform.gameObject);
                // audio
                audioSource.Play(0);

                Explode(hitInfo.transform.position);
            }
        }      
    }
    void Explode(Vector3 pos)
    {
        Debug.Log("2");
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++) {
            for (int y = 0; y < cubesInRow; y++) {
                for (int z = 0; z < cubesInRow; z++) {
                    createPiece(pos.x + (x * cubeSize), pos.y + (y * cubeSize), pos.z + (z * cubeSize));
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        Debug.Log("4");
        foreach (Collider hit in colliders) 
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, pos, explosionRadius, explosionUpward);
            }
        }
        Debug.Log("5");
    }
    // Use this for initialization
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }
    void createPiece(float x, float y, float z) {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = new Vector3(x, y, z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        Destroy(piece, 2);
    }
}