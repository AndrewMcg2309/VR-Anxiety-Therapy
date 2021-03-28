using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Color color;

    int score;
    public Text scoreText;
    

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast (ray, out hitInfo, 100))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            if(hitInfo.collider.tag == "Cube")
            {
                // color
                color = hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material.color;

                //make object disappear
                Destroy(hitInfo.transform.gameObject);
                
                // audio
                audioSource.Play(0);
                
                // explode
                Explode(hitInfo.transform.position, color);

                // add point
                AddScore();
            }
        }      
    }

    void AddScore()
    {
        score++;
        scoreText.text = "" + score;
    }
    void Explode(Vector3 pos, Color objectColor)
    {
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++) {
            for (int y = 0; y < cubesInRow; y++) {
                for (int z = 0; z < cubesInRow; z++) {
                    createPiece(pos.x + (x * cubeSize), pos.y + (y * cubeSize), pos.z + (z * cubeSize), objectColor);
                }
            }
        }

        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius);

        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders) 
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, pos, explosionRadius, explosionUpward);
            }
        }
    }
    // Use this for initialization
    void Start() 
    {
        //initialize score
        score = 0;

        audioSource = GetComponent<AudioSource>();
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void createPiece(float x, float y, float z, Color objectColor) 
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.layer = 8;

         // colour
        piece.GetComponent<Renderer>().material.color = objectColor;

        //set piece position and scale
        piece.transform.position = new Vector3(x, y, z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        //destoy
        Destroy(piece, 2);
    }
}